using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialsInstance;

    private Vector2Int currentAssetSize = Vector2Int.zero;

    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        previewObject.name = "Preview";
        PreparePreview(previewObject);
        PrepareCursor(size);
        currentAssetSize = size;
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            //cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        currentAssetSize = Vector2Int.zero;
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, int rotation, int height, bool validity)
    {
        MovePreview(position, rotation, height);
        RotatePreview(rotation);
        MoveCursor(position, height);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        //cellIndicatorRenderer.material.color = c;
        //c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void MoveCursor(Vector3 position, int height)
    {
        cellIndicator.transform.position = position + (Vector3.up * height * 0.5f);
    }

    private void MovePreview(Vector3 position, int rotation, int height)
    {
        // I don't know how to make this better

        Vector2 lengthOffset = Vector2.zero;

        if (currentAssetSize.y > 1)
        {
            switch (rotation)
            {
                case 0:
                    lengthOffset = Vector2.zero;
                    break;
                case 1:
                    lengthOffset = new Vector2(0.5f, -0.5f);
                    break;
                case 2:
                    lengthOffset = new Vector2(-0.5f, -1f);
                    break;
                case 3:
                    lengthOffset = new Vector2(-1f, -0.5f);
                    break;
            }
        }

        int[] sin = new int[4] { 0, 1, 0, -1 };
        int[] cos = new int[4] { 1, 0, -1, 0 };

        previewObject.transform.position = position + (Vector3.up * previewYOffset);
        previewObject.transform.position += new Vector3(0.5f + ((currentAssetSize.x - 1) * 0.5f) + lengthOffset.x,
                                                        (height * 0.5f) + previewYOffset,
                                                        0.5f + ((currentAssetSize.y - 1) * 0.5f) + lengthOffset.y);
    }

    private void RotatePreview(int rotation)
    {
        previewObject.transform.eulerAngles = Vector3.up * rotation * 90f;
    }
}
