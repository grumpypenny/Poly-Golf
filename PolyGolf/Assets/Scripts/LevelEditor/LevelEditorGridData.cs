using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorGridData
{

    Dictionary<Vector3Int, AssetPlacementData> placedAssets = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2 objectSize, int assetId)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        AssetPlacementData data = new AssetPlacementData(positionsToOccupy, assetId);
        foreach (var pos in positionsToOccupy)
        {
            if (placedAssets.ContainsKey(pos))
            {
                throw new Exception("Invalid placement.");
            }
            placedAssets[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2 objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++) 
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceAssetAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach(var pos in positionToOccupy)
        {
            if (placedAssets.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }
}

public class AssetPlacementData
{
    public List<Vector3Int> occupiedPositions;

    public int assetId { get; private set; }

    public AssetPlacementData(List<Vector3Int> occupiedPositions, int assetId)
    {
        this.occupiedPositions = occupiedPositions;
        this.assetId = assetId;
    }
}