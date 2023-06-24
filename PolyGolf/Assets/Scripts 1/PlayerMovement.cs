using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Slider slider;

    // the max distance one has to drag for max power
    [field: SerializeField] private float maxDrag = 4f;

    private Camera cam;
    private Vector3 pointer;

    // these variables are used to track mouse drag distance
    private Vector3 downPointer;
    private Vector3 upPointer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        cam = Camera.main;
        pointer = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        
        Charge();
    
    
    }

    private void Charge()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            upPointer = pointer;
            downPointer = pointer;
        }
        if (Input.GetMouseButton(0))
        {
            upPointer = pointer;

            float amount = Vector3.SqrMagnitude(upPointer - downPointer) / maxDrag;
            slider.value = amount;
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            // launch the ball
            slider.value = 0;
        }

    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100f, mask))
        {
            pointer = hit.point;
            pointer.y = transform.position.y;
        }
        transform.LookAt(pointer);
    }
}
