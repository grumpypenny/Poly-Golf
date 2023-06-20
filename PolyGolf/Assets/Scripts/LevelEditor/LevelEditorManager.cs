using System.Collections.Generic;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{

    public List<Sprite> levelItemSprites;
    public List<GameObject> levelItemModels;
    public GameObject levelItemPrefab;
    public GameObject scrollViewContent;
    public int currentSelectedItemId;

    private List<LevelItem> levelItems;

    void Start()
    {
        levelItems = new List<LevelItem>();

        // Clear scroll content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate scroll content with assets
        for (int i = 0; i < levelItemSprites.Count; i++)  
        {
            GameObject levelItem = Instantiate(levelItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            levelItem.GetComponent<LevelItem>().SetId(i);
            levelItem.GetComponent<LevelItem>().SetSprite(levelItemSprites[i]);
            levelItem.GetComponent<LevelItem>().SetItemModel(levelItemModels[i]);
            levelItem.GetComponent<LevelItem>().SetLevelEditorManager(this);
            levelItem.transform.SetParent(scrollViewContent.transform);
            levelItems.Add(levelItem.GetComponent<LevelItem>());
        }
    }

    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.y = 0;

        if (Input.GetMouseButtonDown(0) && levelItems[currentSelectedItemId].clicked)
        {
            levelItems[currentSelectedItemId].clicked = false;
            Instantiate(levelItemModels[currentSelectedItemId], worldPosition, Quaternion.identity);
        }
        
    }
}
