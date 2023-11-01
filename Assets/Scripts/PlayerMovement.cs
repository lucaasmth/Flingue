using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Image dashCooldownImage;

    private Camera _cam;
    private Rigidbody _rb;
    private Vector3 _movement;
    private Vector3 _mousePos;
    private PlayerHealth _health;
    private GameMaster _gameMaster;

    private bool _canDash;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _health = GetComponent<PlayerHealth>();
        _gameMaster = FindObjectOfType<GameMaster>();
        
        _canDash = true;
    }

    void Update()
    {
        if (_health.IsDead || _gameMaster.IsPaused || _gameMaster.IsGameFinished) return;

        if (!IsGrounded())
        {
            _rb.AddForce(Vector3.down * 4000f);
        }
        
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void FixedUpdate()
    {
        if (_health.IsDead || _gameMaster.IsPaused || _gameMaster.IsGameFinished) return;
        
        Ray cameraRay = _cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            _mousePos = cameraRay.GetPoint(rayLength);
        }
        transform.LookAt(new Vector3(_mousePos.x, transform.position.y, _mousePos.z));
        
        _rb.AddForce(_movement * moveSpeed * 2000f);
    }

    private IEnumerator Dash()
    {
        const float duration = 1f;
        _canDash = false;
        moveSpeed = 30f;
        yield return new WaitForSeconds(.1f);
        moveSpeed = 8f;
        dashCooldownImage.fillAmount = 1f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            dashCooldownImage.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        _canDash = true;
    }
}