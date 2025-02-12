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

    [Header("Raycast Settings")]
    [SerializeField] private float maxRaycastDistance = 1000f;

    private bool isPlayerInTrigger = false;
    private bool isUsingTelescope = false;
    public GameObject Canyon;
    public float mass = 1f;          // Masa del objeto
    public float timeToReachTarget = 2f;

    private float moveY = 0f;
    private float moveX = 0f;
    private float initialRotationY;
    private float initialRotationX;
    

    private void Start()
    {
        telescopeCamera.enabled = false;
        initialRotationY = transform.localEulerAngles.y;
        initialRotationX = transform.localEulerAngles.x;


        Debug.Log($"Rotación inicial - X: {initialRotationX}, Y: {initialRotationY}, Z: {transform.localEulerAngles.z}");
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
            CalculateAndDisplayCannonAngles();
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
        moveX += Input.GetAxis("Mouse X") * sensitivity;
        moveX = Mathf.Clamp(moveX, minHorizontalAngle, maxHorizontalAngle);

        moveY -= Input.GetAxis("Mouse Y") * sensitivity;
        moveY = Mathf.Clamp(moveY, minVerticalAngle, maxVerticalAngle);

        // Aplicar la rotación manteniendo la X inicial (90°)
        transform.localRotation = Quaternion.Euler(initialRotationX, initialRotationY + moveX, 0);

        Debug.Log($"Rotación después de HandleRotation - X: {transform.localEulerAngles.x}, Y: {transform.localEulerAngles.y}, Z: {transform.localEulerAngles.z}");
    }

    public void CalculateAndDisplayCannonAngles()
    {
        // Lanza un Raycast desde la cámara del telescopio
        Ray ray = new Ray(telescopeCamera.transform.position, telescopeCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            Vector3 targetPos = hit.point;
            Vector3 cannonPos = Canyon.transform.position; // Posición del cañón

            // 1. Calcular el ángulo horizontal (Y)
            Vector3 horizontalDiff = new Vector3(targetPos.x - cannonPos.x, 0, targetPos.z - cannonPos.z);
            float angleY = Mathf.Atan2(horizontalDiff.x, horizontalDiff.z) * Mathf.Rad2Deg;

            // 2. Calcular la distancia horizontal y la diferencia de altura (Y)
            float horizontalDistance = horizontalDiff.magnitude;
            float heightDifference = targetPos.y - cannonPos.y;

            // 3. Calcular la velocidad inicial (v0) a partir de la fuerza aplicada
            float v0 = ApplyForceToReachTarget(targetPos)/1 ;

            // 4. Calcular el ángulo de elevación (X) usando la ecuación balística
            float g = -Physics.gravity.y; // Magnitud de la gravedad
            float v0Squared = v0 * v0;

            // Se debe asegurar que el discriminante sea no negativo
            float discriminant = v0Squared * v0Squared - g * (g * horizontalDistance * horizontalDistance + 2 * heightDifference * v0Squared);
            if (discriminant < 0)
            {
                Debug.Log("El objetivo es inalcanzable con la fuerza aplicada");
                return;
            }

            float sqrtDiscriminant = Mathf.Sqrt(discriminant);
            float angleXRad = Mathf.Atan((v0Squared - sqrtDiscriminant) / (g * horizontalDistance));
            float angleX = angleXRad * Mathf.Rad2Deg;

            Debug.Log($"Ángulo horizontal (Y): {angleY}°");
            Debug.Log($"Ángulo de elevación (X): {angleX}°");
        }
        else
        {
            Debug.Log("No se detectó ningún objetivo en el Raycast");
        }
    }

    public float ApplyForceToReachTarget(Vector3 target)
    {
        if (timeToReachTarget <= 0)
        {
            Debug.LogError("El tiempo para alcanzar el objetivo debe ser mayor que cero.");
            return 0;
        }

        Vector3 startPos = transform.position;
        Vector3 targetPos = target;
        float g = -Physics.gravity.y;  // Magnitud de la gravedad

        // Calcular componentes de la velocidad inicial
        Vector3 horizontalVelocity = (targetPos - startPos) / timeToReachTarget;
        horizontalVelocity.y = 0; // Mantener solo componentes X y Z

        // Componente vertical de la velocidad (fórmula cinemática)
        float verticalVelocity = (targetPos.y - startPos.y) / timeToReachTarget + 0.5f * g * timeToReachTarget;

        Vector3 initialVelocity = new Vector3(
            horizontalVelocity.x,
            verticalVelocity,
            horizontalVelocity.z
        );

        // Aplicar impulso correctamente (masa * velocidad)
        Vector3 impulse = initialVelocity * mass;
        float magnitude = impulse.magnitude;

        return magnitude;
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
