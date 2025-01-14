using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonCameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 25f; // Velocidad de movimiento
    public float maxAngleX = 20f; // Ángulo máximo en el eje X
    public float maxAngleY = 20f; // Ángulo máximo en el eje Yç

    private Quaternion initialRotation;

    private void OnDisable()
    {
        transform.rotation = initialRotation;
    }


    void Start()
    {
        initialRotation = transform.rotation;
        Debug.Log("Initial rotation: " + initialRotation);
    }

    void FixedUpdate()
    {
        // Lee la entrada del usuario
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        float newAngleX = Mathf.Clamp(transform.rotation.x - inputY * speed, initialRotation.x - maxAngleX, initialRotation.x + maxAngleX);

        float newAngleY = Mathf.Clamp(transform.rotation.y + inputX * speed, initialRotation.y - maxAngleY, initialRotation.y + maxAngleY);

        if (inputX != 0 || inputY != 0)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newAngleX, newAngleY, 0),  speed);
    }
}
