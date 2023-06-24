using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    
    public float rotationSpeed = 50f;
    public float zoomSpeed = 20f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    public Vector2 minPan;
    public Vector2 maxPan;
    private bool isPanning = false;
    private Vector3 mouseOrigin;
    private Vector3 panDifference;

    private void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // rotate camera
        transform.Rotate(0, h * rotationSpeed * Time.deltaTime, 0);
        
        // zoom camera
        float newSize = mainCamera.orthographicSize - v * zoomSpeed;
        newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newSize, Time.deltaTime * zoomSpeed);

    }

    private void LateUpdate()
    {
        // pan camera
        if (Input.GetMouseButton(2))
        {
            panDifference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!isPanning)
            {
                isPanning = true;
                mouseOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            isPanning = false;
        }
        if (isPanning)
        {
            transform.position = mouseOrigin - panDifference;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPan.x, maxPan.x), transform.position.y, Mathf.Clamp(transform.position.z, minPan.y, maxPan.y));
    }
}
