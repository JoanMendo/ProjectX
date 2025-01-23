using UnityEngine;

public class TelescopeController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera playerMainCamera;
    [SerializeField] private Camera telescopeCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float maxZoom = 2f;
    [SerializeField] private float minZoom = 60f;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float VerticalAngle = 110f;

    [SerializeField] private float HorizontalAngle = 60f;


    private bool isPlayerInTrigger = false;
    private bool isUsingTelescope = false;

    private float moveY = 0f;
    private float moveX = 0f;
    private float initialRotationY;
    private float initialRotationX;

    private void Awake()
    {
        telescopeCamera.enabled = false;
        initialRotationY =   transform.localEulerAngles.y;
        Debug.Log("Initial rotation Y: " + initialRotationY);
        Debug.Log("Min rotation Y" + (initialRotationY - VerticalAngle) + " Max rotation Y: " + (initialRotationY + VerticalAngle));
        initialRotationX = transform.localEulerAngles.x;
        Debug.Log("Initial rotation X: " + initialRotationX);
        Debug.Log("Min rotation X" + (initialRotationX - HorizontalAngle) + " Max rotation X: " + (initialRotationX + HorizontalAngle));
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ToggleTelescope();
        }

        if (isUsingTelescope)
        {
            HandleZoom();
            HandleRotation();
        }
    }

    private void ToggleTelescope()
    {
        isUsingTelescope = !isUsingTelescope;

        telescopeCamera.enabled = isUsingTelescope;
        playerMainCamera.enabled = !isUsingTelescope;

        Cursor.lockState = isUsingTelescope ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isUsingTelescope;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            telescopeCamera.fieldOfView = Mathf.Clamp(
                telescopeCamera.fieldOfView - scroll * zoomSpeed,
                maxZoom,
                minZoom
            );
        }
    }

    private void HandleRotation()
    {
        moveX = Mathf.Clamp(moveX + Input.GetAxis("Mouse X") * sensitivity,   initialRotationY - VerticalAngle, initialRotationY + VerticalAngle);
        moveY = Mathf.Clamp(moveY + Input.GetAxis("Mouse Y") * sensitivity,   initialRotationX - HorizontalAngle,   initialRotationX + HorizontalAngle);

        transform.localRotation = Quaternion.Euler(moveY,  moveX, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            if (isUsingTelescope)
            {
                ToggleTelescope();
            }
        }
    }
}
