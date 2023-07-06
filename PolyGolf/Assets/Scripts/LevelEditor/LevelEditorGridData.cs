using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class LevelEditorGridData
{

    Dictionary<Vector3Int, AssetPlacementData> placedAssets = new();

    public AssetPlacementData AddObjectAt(AssetData asset, int assetId, Vector3Int gridPosition, int rotation, int height, Vector2 objectSize, int idx)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, rotation, height, objectSize);
        AssetPlacementData data = new AssetPlacementData(positionsToOccupy, idx, asset, assetId, gridPosition, rotation, height);

        foreach (var pos in positionsToOccupy)
        {
            if (placedAssets.ContainsKey(pos))
            {
                throw new Exception("Invalid placement.");
            }
            placedAssets[pos] = data;
        }
        return data;
    }

    public AssetPlacementData RemoveObjectAt(Vector3Int gridPosition, int height)
    {
        Vector3Int processed = new Vector3Int(gridPosition.x, height, gridPosition.z);

        if (!placedAssets.ContainsKey(processed))
        {
            return null;
        }

        AssetPlacementData data = placedAssets[processed];
        foreach (var pos in placedAssets[processed].occupiedPositions)
        {
            placedAssets.Remove(pos);
        }
        return data;
    }


    public int GetRepresentationIndex(Vector3Int gridPosition, int height)
    {
        Vector3Int processed = new Vector3Int(gridPosition.x, height, gridPosition.z);
        if (!placedAssets.ContainsKey(processed))
        {
            return -1;
        }
        return placedAssets[processed].idx;
    }


    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, int rotation, int height, Vector2 objectSize)
    {
        List<Vector3Int> returnVal = new();

        // rotation is index of sin/cos table
        // 0 -> 0 deg, 1 -> 90 deg, 2 -> 180 deg, 3 -> 270 deg
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

    public IReadOnlyList<AssetPlacementData> GetPlacedAssets()
    {
        return placedAssets.Select(kvp => kvp.Value).ToList().AsReadOnly();
    }
}

[Serializable]
public class AssetPlacementData
{
    public AssetData asset { get; private set; }

    public int assetId { get; private set; }

    public Vector3Int position { get; private set; }
    public int rotation { get; private set; }
    public int height { get; private set; }

    public List<Vector3Int> occupiedPositions;

    public int idx { get; private set; }

    public AssetPlacementData(List<Vector3Int> occupiedPositions, int idx, AssetData asset, int assetId, Vector3Int position, int rotation, int height)
    {
        this.occupiedPositions = occupiedPositions;
        this.idx = idx;
        this.assetId = assetId;
        this.asset = asset;
        this.position =  position;
        this.rotation = rotation;
        this.height = height;
    }
}