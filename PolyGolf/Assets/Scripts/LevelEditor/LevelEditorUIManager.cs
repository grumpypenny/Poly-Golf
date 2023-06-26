using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorUIManager : MonoBehaviour
{

    [SerializeField]
    private LevelAssetsDatabase database;

    [SerializeField]
    private GameObject levelItemUIPrefab;

    [SerializeField]
    private GameObject scrollViewContent;

    private List<LevelAssetUI> levelItems;
    private AssetUIFilter currentFilter = AssetUIFilter.All;

    private void Start()
    {
        levelItems = new List<LevelAssetUI>();
        UpdateAssetView();
    }

    private void UpdateAssetView()
    {
        levelItems.Clear();

        // Clear scroll content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate scroll content with assets
        for (int i = 0; i < database.assets.Count; i++)
        {
            if (currentFilter == AssetUIFilter.All || database.assets[i].FilterTag == currentFilter)
            {
                GameObject levelItem = Instantiate(levelItemUIPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                levelItem.GetComponent<LevelAssetUI>().assetId = i;
                levelItem.GetComponent<LevelAssetUI>().SetSprite(database.assets[i].Sprite);
                levelItem.transform.SetParent(scrollViewContent.transform);
                levelItems.Add(levelItem.GetComponent<LevelAssetUI>());
            }
        }
    }

    public void UpdateFilterIndex(int index)
    {
        currentFilter = (AssetUIFilter)index;
        UpdateAssetView();
    }
}
