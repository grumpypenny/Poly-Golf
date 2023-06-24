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
    [field: SerializeField] private float maxDrag = 0.25f;
    [field: SerializeField] private float minDrag = 0.05f;

    // the amount of force with max power
    [field: SerializeField] private float baseForce = 1f;

    private Camera cam;
    private Vector3 pointer;

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
        SetSliderValue();
    }

    private void FixedUpdate()
    {
        Putt();
    }

    private void Putt()
    {



    }

    private void SetSliderValue()
    {
        if (Input.GetMouseButton(0))
        {
            float amount = Vector3.SqrMagnitude(transform.position - pointer) / maxDrag;
            if (amount > minDrag)
            {
                slider.value = amount;
            }
            else
            {
                slider.value = 0f;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // launch the ball
            Vector3 force = transform.forward.normalized * baseForce * slider.value;
            force.y = 0f;
            rb.AddForce(force, ForceMode.Impulse);
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
