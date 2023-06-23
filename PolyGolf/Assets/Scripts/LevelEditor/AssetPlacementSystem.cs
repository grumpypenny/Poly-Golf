using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private LevelAssetsDatabase database;

    [SerializeField]
    private PreviewSystem previewSystem;

    [SerializeField]
    private AssetPlacer assetPlacer;

    IPlacementState placementState;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private LevelEditorGridData assetGridData;
    private LevelEditorGridData assetData;


    private void Start()
    {
        StopPlacement();
        assetGridData = new();
        assetData = new();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        placementState = new PlacementState(id, grid, previewSystem, database, assetGridData, assetData, assetPlacer);
        inputManager.OnClicked += PlaceAsset;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceAsset()
    {
        if (inputManager.isPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        placementState.OnAction(gridPosition);
    }

     private void StopPlacement()
     {
        if (placementState == null)
        {
            return;
        }

        placementState.EndState();
        inputManager.OnClicked -= PlaceAsset;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        placementState = null;
     }

    private void Update()
    {
        if (placementState == null)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            placementState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}
