using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 25f;      // Velocidad de movimiento
    public float maxAngleX = 20f;   // Ángulo máximo en el eje X (vertical)
    public float maxAngleY = 20f;   // Ángulo máximo en el eje Y (horizontal)
    public GameObject verticalMovementObject;
    public GameObject horizontalMovementObject;

    private Vector3 initialRotationHorizontal;  // Rotación inicial en Euler
    private Vector3 initialRotationVertical;
    private Vector2 rotationOffset;   // Offset acumulado (X para vertical, Y para horizontal)

    void Awake()
    {
        initialRotationHorizontal = horizontalMovementObject.transform.localEulerAngles;
        initialRotationVertical = verticalMovementObject.transform.localEulerAngles;
        rotationOffset = Vector2.zero;
    }

    void OnEnable()
    {
        horizontalMovementObject.transform.localEulerAngles = initialRotationHorizontal;
        verticalMovementObject.transform.localEulerAngles = initialRotationVertical;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            float inputX = 0f;
            float inputY = 0f;

            if (Input.GetKey(KeyCode.LeftArrow)) inputX = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) inputX = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) inputY = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) inputY = -1f;

            // Actualizar el offset de rotación acumulado
            // Nota: Se invierte el inputY para que mover hacia arriba rote hacia abajo (y viceversa),
            // pero puedes ajustar según el comportamiento deseado.
            rotationOffset.x = Mathf.Clamp(rotationOffset.x - inputY * speed * Time.deltaTime, -maxAngleX, maxAngleX);
            rotationOffset.y = Mathf.Clamp(rotationOffset.y + inputX * speed * Time.deltaTime, -maxAngleY, maxAngleY);

            // Calcular la rotación destino sumando el offset a la rotación inicial
            Quaternion targetRotationHorizontal  = Quaternion.Euler(initialRotationHorizontal.x - rotationOffset.x, initialRotationHorizontal.y, 0);
            Quaternion targetRotationVertical = Quaternion.Euler(initialRotationVertical.x, initialRotationVertical.y + rotationOffset.y, 0);

            horizontalMovementObject.transform.localRotation= Quaternion.Slerp(horizontalMovementObject.transform.localRotation, targetRotationHorizontal, Time.deltaTime * speed);
            verticalMovementObject.transform.localRotation = Quaternion.Slerp(verticalMovementObject.transform.localRotation, targetRotationVertical, Time.deltaTime * speed);

            // Aplicar la rotación de forma suave con Slerp


        }
    }
}
