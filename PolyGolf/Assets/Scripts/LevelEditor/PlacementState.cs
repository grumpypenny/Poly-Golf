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

    public PlacementState(int index,
                          Grid grid,
                          PreviewSystem previewSystem,
                          LevelAssetsDatabase database,
                          LevelEditorGridData assetGridData,
                          LevelEditorGridData assetData,
                          AssetPlacer assetPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.assetGridData = assetGridData;
        this.assetData = assetData;
        this.assetPlacer = assetPlacer;

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
        selectedData.AddObjectAt(gridPosition, rotation, height, database.assets[selectedObjectIdx].Size, index);

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

        selectedData.RemoveObjectAt(gridPosition, height);
        assetPlacer.RemoveAsset(idx);
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
