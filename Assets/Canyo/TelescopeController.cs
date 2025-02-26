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
    [SerializeField] private float maxVerticalAngle = 110f;
    [SerializeField] private float minVerticalAngle = 70f;
    [SerializeField] private float maxHorizontalAngle = 60f;
    [SerializeField] private float minHorizontalAngle = -60f;

    private bool isPlayerInTrigger = false;
    private bool isUsingTelescope = false;

    private float moveY = 0f;
    private float moveX = 0f;
    private float initialRotationY;

    private void Start()
    {
        telescopeCamera.enabled = false;
        initialRotationY = transform.localEulerAngles.y;
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
        moveX = Mathf.Clamp(moveX + Input.GetAxis("Mouse X") * sensitivity, minHorizontalAngle, maxHorizontalAngle);
        moveY = Mathf.Clamp(moveY - Input.GetAxis("Mouse Y") * sensitivity, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(0f, initialRotationY + moveX, moveY);
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
