using System.Collections.Generic;
using UnityEngine;

public class LevelEditorUIManager : MonoBehaviour
{

    public List<Sprite> levelItemSprites;
    public GameObject levelItemUIPrefab;
    public GameObject scrollViewContent;

    private List<LevelAssetUI> levelItems;

    void Start()
    {
        levelItems = new List<LevelAssetUI>();

        // Clear scroll content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate scroll content with assets
        for (int i = 0; i < levelItemSprites.Count; i++)  
        {
            GameObject levelItem = Instantiate(levelItemUIPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            levelItem.GetComponent<LevelAssetUI>().assetId = i;
            levelItem.GetComponent<LevelAssetUI>().SetSprite(levelItemSprites[i]);
            levelItem.transform.SetParent(scrollViewContent.transform);
            levelItems.Add(levelItem.GetComponent<LevelAssetUI>());
        }
    }
}
