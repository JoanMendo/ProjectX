using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 25f;      // Velocidad de movimiento
    public float maxAngleX = 20f;   // Ángulo máximo en el eje X (vertical)
    public float maxAngleY = 20f;   // Ángulo máximo en el eje Y (horizontal)

    private Vector3 initialRotation;  // Rotación inicial en Euler
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
        if (Input.GetMouseButton(0))
        {
            // Leer la entrada del usuario
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            // Actualizar el offset de rotación acumulado
            // Nota: Se invierte el inputY para que mover hacia arriba rote hacia abajo (y viceversa),
            // pero puedes ajustar según el comportamiento deseado.
            rotationOffset.x = Mathf.Clamp(rotationOffset.x - inputY * speed * Time.deltaTime, -maxAngleX, maxAngleX);
            rotationOffset.y = Mathf.Clamp(rotationOffset.y + inputX * speed * Time.deltaTime, -maxAngleY, maxAngleY);

            // Calcular la rotación destino sumando el offset a la rotación inicial
            Quaternion targetRotation = Quaternion.Euler(initialRotation.x + rotationOffset.x, initialRotation.y + rotationOffset.y, 0);

            // Aplicar la rotación de forma suave con Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }
}
