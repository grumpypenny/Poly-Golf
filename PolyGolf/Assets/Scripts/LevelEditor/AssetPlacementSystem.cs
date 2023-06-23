using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private LevelAssetsDatabase database;
    private int selectedObjectIdx = -1;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private LevelEditorGridData assetGridData, assetData;

    private List<GameObject> placedAssets = new();

    private void Start()
    {
        StopPlacement();
        assetGridData = new();
        assetData = new();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        selectedObjectIdx = id;
        preview.StartShowingPlacementPreview(database.assets[selectedObjectIdx].Prefab, database.assets[selectedObjectIdx].Size);
        inputManager.OnClicked += PlaceAsset;
        //inputManager.OnExit += StopPlacement;
    }

    private void PlaceAsset()
    {
        if (inputManager.isPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool isPlacementValid = CheckPlacementValidity(gridPosition, selectedObjectIdx);
        if (!isPlacementValid)
        {
            return;
        }

        GameObject assetParent = Instantiate(new GameObject());
        GameObject assetObject = Instantiate(database.assets[selectedObjectIdx].Prefab);
        assetObject.transform.SetParent(assetParent.transform);
        assetParent.transform.position = grid.CellToWorld(gridPosition);
        assetObject.transform.localPosition = new Vector3(0.5f + ((database.assets[selectedObjectIdx].Size.x - 1) * 0.5f),
                                                          0f,
                                                          0.5f + ((database.assets[selectedObjectIdx].Size.y - 1) * 0.5f));
        placedAssets.Add(assetObject);

        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        selectedData.AddObjectAt(gridPosition, database.assets[selectedObjectIdx].Size, selectedObjectIdx);

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);    
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIdx)
    {
        LevelEditorGridData selectedData = selectedObjectIdx == 0 ? assetGridData : assetData;
        return selectedData.CanPlaceAssetAt(gridPosition, database.assets[selectedObjectIdx].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIdx = -1;
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceAsset;
        //inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        if (selectedObjectIdx < 0)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            bool isPlacementValid = CheckPlacementValidity(gridPosition, selectedObjectIdx);

            mouseIndicator.transform.position = mousePosition;

            preview.UpdatePosition(grid.CellToWorld(gridPosition), isPlacementValid);
            
            lastDetectedPosition = gridPosition;
        }
    }
}
