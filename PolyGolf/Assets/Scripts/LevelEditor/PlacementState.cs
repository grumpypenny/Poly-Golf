using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IPlacementState
{
    private int selectedObjectIdx = -1;
    Grid grid;
    PreviewSystem previewSystem;
    LevelAssetsDatabase database;
    LevelEditorGridData assetGridData;
    LevelEditorGridData assetData;
    AssetPlacer assetPlacer;
    PlacementHistory placementHistory;

    public PlacementState(int index,
                          Grid grid,
                          PreviewSystem previewSystem,
                          LevelAssetsDatabase database,
                          LevelEditorGridData assetGridData,
                          LevelEditorGridData assetData,
                          AssetPlacer assetPlacer,
                          PlacementHistory placementHistory)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.assetGridData = assetGridData;
        this.assetData = assetData;
        this.assetPlacer = assetPlacer;
        this.placementHistory = placementHistory;

        selectedObjectIdx = index;
        previewSystem.StartShowingPlacementPreview(database.assets[selectedObjectIdx].Prefab, database.assets[selectedObjectIdx].Size);
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition, int rotation, int height)
    {
        bool isPlacementValid = CheckPlacementValidity(gridPosition, rotation, height, selectedObjectIdx);
        if (!isPlacementValid)
        {
            return;
        }

        int index = assetPlacer.PlaceAsset(database.assets[selectedObjectIdx], grid.CellToWorld(gridPosition), rotation, height);

        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        AssetPlacementData placementData = selectedData.AddObjectAt(database.assets[selectedObjectIdx], gridPosition, rotation, height, database.assets[selectedObjectIdx].Size, index);

        placementHistory.UpdateHistory(new PlacementHistoryState(placementData, PlacementType.Placement));
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), rotation, height, false);
    }

    public void OnRemove(Vector3Int gridPosition, int height)
    {
        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;

        int idx = selectedData.GetRepresentationIndex(gridPosition, height);
        if (idx == -1)
        {
            return;
        }

        AssetPlacementData placementData = selectedData.RemoveObjectAt(gridPosition, height);
        placementHistory.UpdateHistory(new PlacementHistoryState(placementData, PlacementType.Removal));
        assetPlacer.RemoveAsset(idx);
    }

    public void Undo()
    {
        PlacementHistoryState state = placementHistory.Undo();
        HandleHistoryChange(state, true);
    }

    public void Redo()
    {
        PlacementHistoryState state = placementHistory.Redo();
        HandleHistoryChange(state, false);
    }

    private void HandleHistoryChange(PlacementHistoryState state, bool IsUndo)
    {
        if (state == null)
        {
            return;
        }

        if ((IsUndo && state.placementType == PlacementType.Removal) || (!IsUndo && state.placementType == PlacementType.Placement))
        {
            int index = assetPlacer.PlaceAsset(state.placementData.asset, state.placementData.position, state.placementData.rotation, state.placementData.height);
            LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
            selectedData.AddObjectAt(state.placementData.asset, state.placementData.position, state.placementData.rotation, state.placementData.height, state.placementData.asset.Size, index);
        }
        else if ((IsUndo && state.placementType == PlacementType.Placement) || (!IsUndo && state.placementType == PlacementType.Removal))
        {
            LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
            int idx = selectedData.GetRepresentationIndex(state.placementData.position, state.placementData.height);
            if (idx == -1)
            {
                return;
            }
            selectedData.RemoveObjectAt(state.placementData.position, state.placementData.height);
            assetPlacer.RemoveAsset(idx);
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int rotation, int height, int selectedObjectIdx)
    {
        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        return selectedData.CanPlaceAssetAt(gridPosition, rotation, height, database.assets[selectedObjectIdx].Size);
    }

    public void UpdateState(Vector3Int gridPosition, int rotation, int height)
    {
        bool isPlacementValid = CheckPlacementValidity(gridPosition, rotation, height, selectedObjectIdx);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), rotation, height, isPlacementValid);
    }
}
