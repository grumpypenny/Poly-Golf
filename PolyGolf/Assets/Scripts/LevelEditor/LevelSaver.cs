using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSaver : MonoBehaviour
{
    private LevelEditorGridData assetData;

    public GameObject saveLevelModal;

    public TextMeshProUGUI errorText;

    private string fileName = "";

    private void Start()
    {
        CloseSaveModal();
    }

    public void UpdateFileName(string newName)
    {
        fileName = newName;
    }

    public void OpenSaveModal()
    {
        saveLevelModal.SetActive(true);
    }

    public void CloseSaveModal()
    {
        saveLevelModal.SetActive(false);
        errorText.text = string.Empty;
    }

    public void SaveLevel()
    {
        IReadOnlyList<AssetPlacementData> placedAssets = assetData.GetPlacedAssets();

        if (placedAssets.Count == 0)
        {
            errorText.text = "No assets placed.";
            return;
        }

        LevelSaveSnapshot levelSaveSnapshot = new();
        foreach (var asset in placedAssets)
        {
            levelSaveSnapshot.assets.Add(new AssetPlacementSnapshot(asset.assetId, asset.position, asset.rotation, asset.height));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        string data = JsonUtility.ToJson(levelSaveSnapshot);
        string path = Application.persistentDataPath + $"/{fileName}.json";
        System.IO.File.WriteAllText(path, data);
        Debug.Log($"Saved level to {path}");

        CloseSaveModal();
    }

    public void SetGridData(LevelEditorGridData assetData)
    {
        this.assetData = assetData;
    }
}

[Serializable]
public class LevelSaveSnapshot
{
    public List<AssetPlacementSnapshot> assets = new();
}


[Serializable]
public class AssetPlacementSnapshot
{
    public int assetId;
    public Vector3Int position;
    public int rotation;
    public int height;

    public AssetPlacementSnapshot(int assetId, Vector3Int position, int rotation, int height)
    {
        this.assetId = assetId;
        this.position = position;
        this.rotation = rotation;
        this.height = height;
    }
}


