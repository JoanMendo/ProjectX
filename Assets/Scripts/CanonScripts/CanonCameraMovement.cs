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

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        Quaternion rotationX = Quaternion.AngleAxis(-y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(x, Vector3.up);

        transform.localRotation = initialRotation * rotationX * rotationY;

        transform.localEulerAngles = new Vector3(
            Mathf.Clamp(transform.localEulerAngles.x, 0, maxAngleX),
            Mathf.Clamp(transform.localEulerAngles.y, 0, maxAngleY),
            transform.localEulerAngles.z
        );
    }
}
