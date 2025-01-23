using UnityEngine;

public class TelescopeController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera playerMainCamera;
    [SerializeField] private Camera telescopeCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float Zoom = 2f;



    [Header("Rotation Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float VerticalAngle = 110f;

    [SerializeField] private float HorizontalAngle = 60f;



    private float moveY = 0f;
    private float moveX = 0f;
    private float initialRotationY;
    private float initialRotationX;
    private float initialZoom = 60f;

    private void Awake()
    {
        telescopeCamera.enabled = false;
        initialRotationY =   transform.localEulerAngles.y;
        initialRotationX = transform.localEulerAngles.x;
        initialZoom = telescopeCamera.fieldOfView;
    }

    private void Update()
    {

        if (telescopeCamera.isActiveAndEnabled)
        {
            HandleZoom();
            HandleRotation();
        }
    }


    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Fire2");
        if (scroll != 0)
        {
            telescopeCamera.fieldOfView = initialZoom - Zoom ;
        }
        else
        {
            telescopeCamera.fieldOfView = initialZoom;
        }
    }
    private void HandleRotation()
    {
        moveX = Mathf.Clamp(moveX + Input.GetAxis("Mouse X") * sensitivity,   initialRotationY - VerticalAngle, initialRotationY + VerticalAngle);
        moveY = Mathf.Clamp(moveY + Input.GetAxis("Mouse Y") * sensitivity,   initialRotationX - HorizontalAngle,   initialRotationX + HorizontalAngle);

        transform.localRotation = Quaternion.Euler(moveY,  moveX, 0);
    }

}
