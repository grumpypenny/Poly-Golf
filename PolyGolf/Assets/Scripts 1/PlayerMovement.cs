using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private LayerMask mask;

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
        ////if (Input.GetMouseButton(0))
        ////{
        ////Gizmos.color = Color.yellow;
        ////Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), 20f);
        //Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        ////Vector3 flatInput = cam.ScreenToWorldPoint(Input.mousePosition);
        ////flatInput.y = 0;

        ////Vector3 relative = flatInput - transform.position;
        ////relative.y = 0;
        ////Debug.DrawRay(transform.position, relative, Color.blue);
        ////}
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100f, mask))
        {
            pointer = hit.point;
            pointer.y = transform.position.y;
        }
        Vector3 direction = pointer - transform.position;
        transform.LookAt(pointer);
        //transform.eulerAngles = direction;
        Debug.DrawLine(transform.position, pointer, Color.red);
        Debug.DrawRay(transform.position, direction, Color.yellow);
    }

    private void FixedUpdate()
    {

    }

    //private void OnDrawGizmos()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), 20f);
    //        Debug.Log("drawing point");
    //    }
    //}
}
