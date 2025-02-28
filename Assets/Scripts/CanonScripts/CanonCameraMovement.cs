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
    public float keyboardSensitivity = 0.2f;  // Sensibilidad para las teclas (valor menor para más precisión)
    public float smoothness = 5f;   // Factor de suavizado para el movimiento
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

    void Update()  // Cambiado a Update para mejor respuesta de entrada
    {
        if (Input.GetMouseButton(0))
        {
            float inputX = 0f;
            float inputY = 0f;

            // Usar valores más pequeños para movimientos más precisos
            if (Input.GetKey(KeyCode.LeftArrow)) inputX = -keyboardSensitivity;
            if (Input.GetKey(KeyCode.RightArrow)) inputX = keyboardSensitivity;
            if (Input.GetKey(KeyCode.UpArrow)) inputY = keyboardSensitivity;
            if (Input.GetKey(KeyCode.DownArrow)) inputY = -keyboardSensitivity;

            // Actualizar el offset de rotación acumulado
            rotationOffset.x = Mathf.Clamp(rotationOffset.x - inputY * speed * Time.deltaTime, -maxAngleX, maxAngleX);
            rotationOffset.y = Mathf.Clamp(rotationOffset.y + inputX * speed * Time.deltaTime, -maxAngleY, maxAngleY);

            // Calcular la rotación destino sumando el offset a la rotación inicial
            Quaternion targetRotationHorizontal = Quaternion.Euler(initialRotationHorizontal.x - rotationOffset.x, initialRotationHorizontal.y, initialRotationHorizontal.z);
            Quaternion targetRotationVertical = Quaternion.Euler(initialRotationVertical.x, initialRotationVertical.y + rotationOffset.y, initialRotationVertical.z);

            // Aplicar la rotación de forma suave con Slerp
            horizontalMovementObject.transform.localRotation = Quaternion.Slerp(horizontalMovementObject.transform.localRotation, targetRotationHorizontal, Time.deltaTime * smoothness);
            verticalMovementObject.transform.localRotation = Quaternion.Slerp(verticalMovementObject.transform.localRotation, targetRotationVertical, Time.deltaTime * smoothness);
        }
    }
}
