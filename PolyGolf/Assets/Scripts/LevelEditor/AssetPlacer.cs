using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedAssets = new();

    public int PlaceObject(AssetData asset, Vector3 position)
    {
        GameObject assetParent = Instantiate(new GameObject());
        GameObject assetObject = Instantiate(asset.Prefab);
        assetObject.transform.SetParent(assetParent.transform);
        assetParent.transform.position = position;
        assetObject.transform.localPosition = new Vector3(0.5f + ((asset.Size.x - 1) * 0.5f),
                                                          0f,
                                                          0.5f + ((asset.Size.y - 1) * 0.5f));
        placedAssets.Add(assetObject);
        return placedAssets.Count - 1;
    }
}
