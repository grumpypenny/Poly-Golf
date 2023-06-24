using System.Collections.Generic;
using UnityEngine;

public class AssetPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedAssets = new();

    public int PlaceAsset(AssetData asset, Vector3 position, int rotation, float height)
    {
        // I don't know how to make this better
        Vector2 lengthOffset = Vector2.zero;
        if (asset.Size.y > 1)
        {
            switch (rotation)
            {
                case 0:
                    lengthOffset = Vector2.zero;
                    break;
                case 1:
                    lengthOffset = new Vector2(0.5f, -0.5f);
                    break;
                case 2:
                    lengthOffset = new Vector2(0f, -1f);
                    break;
                case 3:
                    lengthOffset = new Vector2(-0.5f, -0.5f);
                    break;
            }
        }

        GameObject assetParent = new GameObject();
        GameObject assetObject = Instantiate(asset.Prefab);
        assetParent.name = asset.Prefab.name;
        assetObject.name = asset.Prefab.name;
        assetObject.transform.SetParent(assetParent.transform);
        assetParent.transform.position = position + (Vector3.up * height * 0.5f);
        assetObject.transform.localPosition = new Vector3(0.5f + ((asset.Size.x - 1) * 0.5f) + lengthOffset.x,
                                                          0f,
                                                          0.5f + ((asset.Size.y - 1) * 0.5f) + lengthOffset.y);
        assetObject.transform.localEulerAngles = new Vector3(0f, rotation * 90f, 0f);
        placedAssets.Add(assetObject);
        return placedAssets.Count - 1;
    }

    public void RemoveAsset(int idx)
    {
        if (placedAssets.Count <= idx || placedAssets[idx] == null)
        {
            return;
        }

        GameObject parent = placedAssets[idx].transform.parent.gameObject;
        Destroy(placedAssets[idx]);
        Destroy(parent);
        placedAssets[idx] = null;
    }
}
