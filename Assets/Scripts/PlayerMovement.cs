using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    
    private Camera _cam;
    private Rigidbody _rb;
    private Vector3 _movement;
    private Vector3 _mousePos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");

        Ray cameraRay = _cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            _mousePos = cameraRay.GetPoint(rayLength);
        }
        transform.LookAt(new Vector3(_mousePos.x, transform.position.y, _mousePos.z));
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * (moveSpeed * Time.deltaTime));
    }
}
