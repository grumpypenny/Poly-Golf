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

    public void OnAction(Vector3Int gridPosition)
    {
        bool isPlacementValid = CheckPlacementValidity(gridPosition, selectedObjectIdx);
        if (!isPlacementValid)
        {
            return;
        }

        int index = assetPlacer.PlaceObject(database.assets[selectedObjectIdx], grid.CellToWorld(gridPosition));

        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        selectedData.AddObjectAt(gridPosition, database.assets[selectedObjectIdx].Size, index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIdx)
    {
        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        return selectedData.CanPlaceAssetAt(gridPosition, database.assets[selectedObjectIdx].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool isPlacementValid = CheckPlacementValidity(gridPosition, selectedObjectIdx);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), isPlacementValid);
    }
}
