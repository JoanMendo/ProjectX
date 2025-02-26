using UnityEngine;

public class TelescopeController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 25f;      // Velocidad de movimiento
    public float maxAngleX = 20f;   // �ngulo m�ximo en el eje X (vertical)
    public float maxAngleY = 20f;   // �ngulo m�ximo en el eje Y (horizontal)
    public GameObject proyectileOrigin;

    private Vector3 initialRotation;  // Rotaci�n inicial en Euler
    private Vector2 rotationOffset;   // Offset acumulado (X para vertical, Y para horizontal)

    void Awake()
    {
        initialRotation = transform.eulerAngles;
        rotationOffset = Vector2.zero;
    }

    void OnEnable()
    {
        transform.eulerAngles = initialRotation;
        rotationOffset = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            // Leer la entrada del usuario
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");

            // Actualizar el offset de rotaci�n acumulado
            // Nota: Se invierte el inputY para que mover hacia arriba rote hacia abajo (y viceversa),
            // pero puedes ajustar seg�n el comportamiento deseado.
            rotationOffset.x = Mathf.Clamp(rotationOffset.x - inputY * speed * Time.deltaTime, -maxAngleX, maxAngleX);
            rotationOffset.y = Mathf.Clamp(rotationOffset.y + inputX * speed * Time.deltaTime, -maxAngleY, maxAngleY);

            // Calcular la rotaci�n destino sumando el offset a la rotaci�n inicial
            Quaternion targetRotation = Quaternion.Euler(initialRotation.x + rotationOffset.x, initialRotation.y + rotationOffset.y, 0);

            // Aplicar la rotaci�n de forma suave con Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }
    public void CalculateAndDisplayCannonAngles()
    {
        // Lanza un Raycast desde la c�mara del telescopio
        Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        int layer = LayerMask.GetMask("Barco");

        if (Physics.Raycast(ray, out RaycastHit hit, 400f, layer))
        {
            Vector3 targetPos = hit.point;
            Vector3 cannonPos = proyectileOrigin.transform.position; // Posici�n del ca��n

            // 1. Calcular el �ngulo horizontal (Y)
            Vector3 horizontalDiff = new Vector3(targetPos.x - cannonPos.x, 0, targetPos.z - cannonPos.z);
            Quaternion lookRotation = Quaternion.LookRotation(horizontalDiff);
            float angleY = lookRotation.eulerAngles.y;


            // 2. Calcular la distancia horizontal y la diferencia de altura (Y)
            float horizontalDistance = horizontalDiff.magnitude;
            float heightDifference = targetPos.y - cannonPos.y;

            // 3. Calcular la velocidad inicial (v0) a partir de la fuerza aplicada
            float v0 = 46f;
    
            // 4. Calcular el �ngulo de elevaci�n (X) usando la ecuaci�n bal�stica
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

            // Se obtienen dos posibles �ngulos
            float angleXRad1 = Mathf.Atan((v0Squared + sqrtDiscriminant) / (g * horizontalDistance));
            float angleXRad2 = Mathf.Atan((v0Squared - sqrtDiscriminant) / (g * horizontalDistance));

            // Elegimos el �ngulo menor
            float angleX = Mathf.Min(angleXRad1, angleXRad2) * Mathf.Rad2Deg;
            Debug.Log($"�ngulo de elevaci�n corregido (X): {angleX}�");

            Debug.Log($"�ngulo horizontal (Y): {angleY}�");
            Debug.Log($"�ngulo de elevaci�n (X): {angleX}�");
        }
        else
        {
            Debug.Log("No se detect� ning�n objetivo en el Raycast");
        }
    }
}
