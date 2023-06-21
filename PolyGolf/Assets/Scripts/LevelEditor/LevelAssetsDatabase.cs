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
}