using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f; // Velocidad de movimiento
    public float maxAngleX = 30f; // �ngulo m�ximo en el eje X
    public float maxAngleY = 30f; // �ngulo m�ximo en el eje Y

    private Vector3 initialRotation;

    private void OnDisable()
    {
        // Restaura la rotaci�n inicial de la c�mara
        transform.eulerAngles = initialRotation;
    }


    void Start()
    {
        // Guarda la rotaci�n inicial de la c�mara
        initialRotation = transform.eulerAngles;
    }

    void Update()
    {
        // Lee la entrada del usuario
        float inputX = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float inputY = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo

        // Calcula el nuevo �ngulo basado en la entrada del usuario
        float newAngleX = Mathf.Clamp(initialRotation.x - inputY * speed * Time.deltaTime, initialRotation.x - maxAngleX, initialRotation.x + maxAngleX);
        float newAngleY = Mathf.Clamp(initialRotation.y + inputX * speed * Time.deltaTime, initialRotation.y - maxAngleY, initialRotation.y + maxAngleY);

        // Aplica la nueva rotaci�n
        transform.eulerAngles = new Vector3(newAngleX, newAngleY, transform.eulerAngles.z);
    }
}
