using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public bool clicked;
    private int itemId;
    private GameObject itemModel;
    private LevelEditorManager manager;

    void Start()
    {
        clicked = false;
    }

    public void ClickItem()
    {
        clicked = true;
        Debug.Log(manager);
        manager.currentSelectedItemId = itemId;
    }

    public void SetId(int newId)
    {
        itemId = newId;
    }

    public int GetId()
    {
        return itemId;
    }

    public void SetSprite(Sprite newSprite)
    {
        Image spriteImage = transform.GetChild(1).GetComponent<Image>();
        spriteImage.sprite = newSprite;
    }

    public void SetItemModel(GameObject obj)
    {
        itemModel = obj;
    }

    public GameObject GetItemModel()
    {
        return itemModel;
    }

    public void SetLevelEditorManager(LevelEditorManager newManager)
    {
        manager = newManager;
    }
}
