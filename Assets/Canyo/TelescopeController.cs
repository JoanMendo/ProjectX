using UnityEngine;

public class TelescopeController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float horizontalSpeed = 50f;
    public float verticalSpeed = 25f;
    public float maxHorizontalAngle = 30f;
    public float maxVerticalAngle = 30f;
    public GameObject proyectileOrigin;

    [Header("Camera Settings")]
    public Camera telescopeCamera;  // Cámara del telescopio
    public Camera playerCamera;     // Cámara del jugador (asignar desde inspector)

    private Quaternion initialRotation;
    private Vector2 currentAngles = Vector2.zero;
    private bool playerInTrigger = false;

    void Awake()
    {
        initialRotation = transform.localRotation;

        // Asegurarse de que la cámara del telescopio esté desactivada al inicio
        if (telescopeCamera == null)
            telescopeCamera = GetComponent<Camera>();

        telescopeCamera.enabled = false;
    }

    void OnEnable()
    {
        transform.localRotation = initialRotation;
        currentAngles = Vector2.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;

            // Cambiar cámaras
            if (playerCamera != null)
                playerCamera.enabled = false;

            telescopeCamera.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            // Restaurar cámaras
            telescopeCamera.enabled = false;

            if (playerCamera != null)
                playerCamera.enabled = true;
        }
    }

    void FixedUpdate()
    {
        // Solo ejecutar si el jugador está en el trigger
        if (!playerInTrigger)
            return;

        if (Input.GetMouseButton(1))
        {
            // Leer la entrada del usuario
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");

            // Actualizar los ángulos con límites independientes
            currentAngles.y += inputX * horizontalSpeed * Time.deltaTime;
            currentAngles.x -= inputY * verticalSpeed * Time.deltaTime;

            // Aplicar límites como un rectángulo
            currentAngles.x = Mathf.Clamp(currentAngles.x, -maxVerticalAngle, maxVerticalAngle);
            currentAngles.y = Mathf.Clamp(currentAngles.y, -maxHorizontalAngle, maxHorizontalAngle);

            // Aplicar rotaciones de forma independiente
            ApplyRectangularRotation();
        }
        CalculateAndDisplayCannonAngles();
    }

    private void ApplyRectangularRotation()
    {
        // Aplicamos primero la rotación inicial
        transform.localRotation = initialRotation;

        // Luego aplicamos rotación vertical (alrededor del eje X)
        transform.Rotate(Vector3.right, currentAngles.x, Space.Self);

        // Finalmente aplicamos rotación horizontal (alrededor del eje Y)
        transform.Rotate(Vector3.up, currentAngles.y, Space.Self);
    }

    public void CalculateAndDisplayCannonAngles()
    {
        // Lanza un Raycast desde la cámara del telescopio
        Ray ray = telescopeCamera.ScreenPointToRay(Input.mousePosition);
        int layer = LayerMask.GetMask("Barco");

        if (Physics.Raycast(ray, out RaycastHit hit, 400f, layer))
        {
            Vector3 targetPos = hit.point;
            Vector3 cannonPos = proyectileOrigin.transform.position; // Posición del cañón

            // 1. Calcular el ángulo horizontal (Y)
            Vector3 horizontalDiff = new Vector3(targetPos.x - cannonPos.x, 0, targetPos.z - cannonPos.z);
            Quaternion lookRotation = Quaternion.LookRotation(horizontalDiff);
            float angleY = lookRotation.eulerAngles.y;

            // 2. Calcular la distancia horizontal y la diferencia de altura (Y)
            float horizontalDistance = horizontalDiff.magnitude;
            float heightDifference = targetPos.y - cannonPos.y;

            // 3. Calcular la velocidad inicial (v0) a partir de la fuerza aplicada
            float v0 = 46f;

            // 4. Calcular el ángulo de elevación (X) usando la ecuación balística
            float g = -Physics.gravity.y; // Magnitud de la gravedad

            float v0Squared = v0 * v0;

            // Se debe asegurar que el discriminante sea no negativo
            float discriminant = (v0Squared * v0Squared) - (g * (g * horizontalDistance * horizontalDistance + 2 * heightDifference * v0Squared));
            if (discriminant < 0)
            {
                Debug.Log("El objetivo es inalcanzable con la fuerza aplicada.");
                return;
            }

            float sqrtDiscriminant = Mathf.Sqrt(discriminant);

            // Se obtienen dos posibles ángulos
            float angleXRad1 = Mathf.Atan((v0Squared + sqrtDiscriminant) / (g * horizontalDistance));
            float angleXRad2 = Mathf.Atan((v0Squared - sqrtDiscriminant) / (g * horizontalDistance));

            // Elegimos el ángulo menor
            float angleX = Mathf.Min(angleXRad1, angleXRad2) * Mathf.Rad2Deg;
            Debug.Log($"Ángulo de elevación corregido (X): {angleX}°");

            Debug.Log($"Ángulo horizontal (Y): {angleY}°");
            Debug.Log($"Ángulo de elevación (X): {angleX}°");
        }
        else
        {
            Debug.Log("No se detectó ningún objetivo en el Raycast");
        }
    }
}