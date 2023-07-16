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

    public bool addMeshCollider = false;

    public GameObject scrollContentParent;
    public GameObject loadItemPrefab;

    public GameObject loadLevelModal;
    public TextMeshProUGUI errorText;

    public GameObject assetParentPrefab;
    private GameObject assetParent;
    private List<int> placedObjects = new();

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
        catch
        {
            errorText.text = "Error loading save files!";
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
            try
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    LevelSaveSnapshot levelSave = JsonConvert.DeserializeObject<LevelSaveSnapshot>(json);
                    PlaceAssets(levelSave);
                    
                    if (addMeshCollider)
                    {
                        CombineMeshes();
                    }
                }
                CloseLoadModal();
            }
            catch
            {
                errorText.text = "Invalid save file!";
            }
        }
    }

    private void PlaceAssets(LevelSaveSnapshot levelSaveSnapshot)
    {
        // destroy previously placed objs
        foreach(int index in placedObjects)
        {
            assetPlacer.RemoveAsset(index);
        }
        placedObjects.Clear();

        if (assetParent == null)
        {
            assetParent = Instantiate(assetParentPrefab, Vector3.zero, Quaternion.identity);
        }

        foreach (AssetPlacementSnapshot assetPlacement in levelSaveSnapshot.assets)
        {
            AssetData asset = assetsDatabase.assets[assetPlacement.assetId];
            int idx = assetPlacer.PlaceAsset(asset, assetPlacement.position, assetPlacement.rotation, assetPlacement.height);
            placedObjects.Add(idx);
            assetPlacer.GetPlacedObject(idx).transform.parent.SetParent(assetParent.transform);
        }
    }

    private void CombineMeshes()
    {
        if (assetParent == null)
        {
            Debug.Log("Asset parent is null!");
            return;
        }

        List<MeshFilter> sourceMeshFilter = new();
        foreach (int idx in placedObjects)
        {
            MeshFilter mesh = assetPlacer.GetPlacedObject(idx).GetComponent<MeshFilter>();
            if (mesh != null)
            {
                sourceMeshFilter.Add(mesh);
            }
        }

        Debug.Log(sourceMeshFilter.Count);

        CombineInstance[] combine = new CombineInstance[sourceMeshFilter.Count];
        for (int i = 0; i < sourceMeshFilter.Count; i++)
        {
            combine[i].mesh = sourceMeshFilter[i].mesh;
            combine[i].transform = sourceMeshFilter[i].transform.localToWorldMatrix;
        }

        Mesh combineMesh = new Mesh();
        combineMesh.CombineMeshes(combine);
        assetParent.GetComponent<MeshFilter>().mesh = combineMesh;

    }
}
    