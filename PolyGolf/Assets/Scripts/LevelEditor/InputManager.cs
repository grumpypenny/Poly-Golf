using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public event Action OnClicked;
    public event Action OnRemoved;
    public event Action OnRotateCounterClockwise;
    public event Action OnRotateClockwise;
    public event Action OnIncreaseHeight;
    public event Action OnDecreaseHeight;
    public event Action OnExit;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetMouseButton(1))
        {
            OnRemoved?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnRotateCounterClockwise?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnRotateClockwise?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnIncreaseHeight?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnDecreaseHeight?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool isPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedLevelPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
