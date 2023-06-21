using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;

    [SerializeField]
    GameObject cellIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private LevelAssetsDatabase database;
    private int selectedObjectIdx = -1;

    [SerializeField]

    private void Start()
    {
        StopPlacement();   
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        selectedObjectIdx = id;
        cellIndicator.SetActive(true);
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
        GameObject assetParent = Instantiate(new GameObject());
        GameObject assetObject = Instantiate(database.assets[selectedObjectIdx].Prefab);
        assetObject.transform.SetParent(assetParent.transform);
        assetParent.transform.position = grid.CellToWorld(gridPosition);
        assetObject.transform.localPosition = new Vector3(0.5f, 0f, 0.5f);
    }

    private void StopPlacement()
    {
        selectedObjectIdx = -1;
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceAsset;
        //inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIdx < 0)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
