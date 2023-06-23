using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    
    public float rotationSpeed = 50f;
    public float zoomSpeed = 20f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private void FixedUpdate()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;
        newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        Debug.Log(newSize);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newSize, Time.deltaTime * zoomSpeed);
    }
}
