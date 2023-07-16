using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class LevelLoader : MonoBehaviour
{

    [SerializeField]
    private LevelAssetsDatabase assetsDatabase;

    [SerializeField]
    private AssetPlacer assetPlacer;

    public bool combineMesh = false;

    public GameObject scrollContentParent;
    public GameObject loadItemPrefab;

    public GameObject loadLevelModal;
    public TextMeshProUGUI errorText;

    private string filePath;

    private void Start()
    {
        CloseLoadModal();
    }

    public void SetFilePath(string filePath)
    {
        this.filePath = filePath;
    }

    public void OpenLoadModal()
    {
        // clear old items
        foreach (Transform child in scrollContentParent.transform)
        {
            Destroy(child.gameObject);
        }

        try
        {
            string basePath = Application.persistentDataPath;
            string[] files = Directory.GetFiles(basePath, "*.json");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                GameObject item = Instantiate(loadItemPrefab);
                item.GetComponent<LoadLevelItem>().SetFilePath(filePath);
                item.GetComponent<LoadLevelItem>().SetFileName(fileName);
                item.GetComponent<LoadLevelItem>().SetLevelLoader(this);
                item.transform.SetParent(scrollContentParent.transform);
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }

        loadLevelModal.SetActive(true);
    }

    public void CloseLoadModal()
    {
        loadLevelModal.SetActive(false);
        errorText.text = string.Empty;
    }

    public void LoadLevel()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            errorText.text = "Select a file to load.";
        }
        else
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                LevelSaveSnapshot levelSave = JsonConvert.DeserializeObject<LevelSaveSnapshot>(json);
                PlaceAssets(levelSave);
            }
            CloseLoadModal();
        }
    }

    private void PlaceAssets(LevelSaveSnapshot levelSaveSnapshot)
    {
        foreach (AssetPlacementSnapshot assetPlacement in levelSaveSnapshot.assets)
        {
            AssetData asset = assetsDatabase.assets[assetPlacement.assetId];
            assetPlacer.PlaceAsset(asset, assetPlacement.position, assetPlacement.rotation, assetPlacement.height);
        }
    }
}
