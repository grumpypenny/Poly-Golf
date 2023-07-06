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

    [SerializeField]
    private PlacementHistory placementHistory;

    [SerializeField]
    private LevelSaver levelSaver;

    IPlacementState placementState;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    private int lastDetectedRotation = -1;
    private int lastAssetHeight = -1;

    private LevelEditorGridData assetData;
    private int currentAssetRotation = 0;
    private int currentAssetHeight = 0;

    private void Start()
    {
        StopPlacement();
        assetData = new();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        placementState = new PlacementState(id, grid, previewSystem, database, assetData, assetPlacer, placementHistory);
        inputManager.OnClicked += PlaceAsset;
        inputManager.OnRemoved += RemoveAsset;
        inputManager.OnRotateClockwise += RotateAssetClockwise;
        inputManager.OnRotateCounterClockwise += RotateAssetCounterClockwise;
        inputManager.OnIncreaseHeight += IncreaseAssetHeight;
        inputManager.OnDecreaseHeight += DecreaseAssetHeight;
        inputManager.OnUndo += Undo;
        inputManager.OnRedo += Redo;
        inputManager.OnSave += SaveLevel;
        inputManager.OnExit += Exit;
    }

    private void PlaceAsset()
    {
        if (inputManager.isPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        placementState.OnAction(gridPosition, currentAssetRotation, currentAssetHeight);
    }

    private void RemoveAsset()
    {
        if (inputManager.isPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        placementState.OnRemove(gridPosition, currentAssetHeight);
    }

    public void Undo()
    {
        placementState.Undo();
    }

    public void Redo()
    {
        placementState.Redo();
    }

    public void SaveLevel()
    {
        levelSaver.OpenSaveModal();
        levelSaver.SetGridData(assetData);
    }

    public void Exit()
    {
        levelSaver.CloseSaveModal();
        StopPlacement();
    }

    private void StopPlacement()
     {
        if (placementState == null)
        {
            return;
        }

        placementState.EndState();
        inputManager.OnClicked -= PlaceAsset;
        inputManager.OnRemoved -= RemoveAsset;
        inputManager.OnExit -= Exit;
        inputManager.OnRotateClockwise -= RotateAssetClockwise;
        inputManager.OnRotateCounterClockwise -= RotateAssetCounterClockwise;
        inputManager.OnIncreaseHeight -= IncreaseAssetHeight;
        inputManager.OnDecreaseHeight -= DecreaseAssetHeight;
        inputManager.OnUndo -= Undo;
        inputManager.OnRedo -= Redo;
        inputManager.OnSave -= SaveLevel;
        lastDetectedPosition = Vector3Int.zero;
        lastDetectedRotation = -1;
        lastAssetHeight = -1;
        placementState = null;
     }

    private void RotateAssetCounterClockwise()
    {
        currentAssetRotation -= 1;
        if (currentAssetRotation < 0)
        {
            currentAssetRotation = 3;
        }
    }

    private void RotateAssetClockwise()
    {
        currentAssetRotation = (currentAssetRotation + 1) % 4;
    }

    private void IncreaseAssetHeight()
    {
        currentAssetHeight = Mathf.Clamp(currentAssetHeight + 1, 0, 3);
    }

    private void DecreaseAssetHeight()
    {
        currentAssetHeight = Mathf.Clamp(currentAssetHeight - 1, 0, 3);
    }

    private void Update()
    {
        if (placementState == null || inputManager.isPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedLevelPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition || lastDetectedRotation != currentAssetRotation || lastAssetHeight != currentAssetHeight)
        {
            placementState.UpdateState(gridPosition, currentAssetRotation, currentAssetHeight);
            lastDetectedPosition = gridPosition;
            lastDetectedRotation = currentAssetRotation;
            lastAssetHeight = currentAssetHeight;
        }
    }
}
