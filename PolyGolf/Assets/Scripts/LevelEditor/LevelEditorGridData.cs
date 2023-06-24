using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorGridData
{

    Dictionary<Vector3Int, AssetPlacementData> placedAssets = new();

    public void AddObjectAt(Vector3Int gridPosition, int rotation, int height, Vector2 objectSize, int assetId)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, rotation, height, objectSize);
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

    public void RemoveObjectAt(Vector3Int gridPosition, int height)
    {
        Vector3Int processed = new Vector3Int(gridPosition.x, height, gridPosition.z);
        foreach (var pos in placedAssets[processed].occupiedPositions)
        {
            placedAssets.Remove(pos);
        }
    }

    public int GetRepresentationIndex(Vector3Int gridPosition, int height)
    {
        Vector3Int processed = new Vector3Int(gridPosition.x, height, gridPosition.z);
        if (!placedAssets.ContainsKey(processed))
        {
            return -1;
        }
        return placedAssets[processed].assetId;
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, int rotation, int height, Vector2 objectSize)
    {
        List<Vector3Int> returnVal = new();

        int[] sin = new int[4] { 0, 1, 0, -1 };
        int[] cos = new int[4] { 1, 0, -1, 0 };

        for (int x = 0; x < objectSize.x; x++) 
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                int x_prime = x * cos[rotation] - y * -sin[rotation];
                int y_prime = x * -sin[rotation] + y * cos[rotation];
                returnVal.Add(gridPosition + new Vector3Int(x_prime, height, y_prime));
            }
        }
        return returnVal;
    }

    public bool CanPlaceAssetAt(Vector3Int gridPosition, int rotation, int height, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, rotation, height, objectSize);
        foreach(var pos in positionsToOccupy)
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