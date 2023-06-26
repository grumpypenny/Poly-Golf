using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelAssetsDatabase : ScriptableObject
{
    public List<AssetData> assets;
}

[Serializable]
public class AssetData
{ 
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public Sprite Sprite { get; private set; }

    [field: SerializeField]
    public AssetUIFilter FilterTag { get; private set; }
}

[Serializable]
public enum AssetUIFilter
{
    All = 0,
    Turf = 1,
    CurvedTurf = 2,
    Ramp = 3,
    Hole = 4,
    Misc = 5
}