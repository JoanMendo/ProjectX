using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMovement : MonoBehaviour
{
    public Vector3 targetPosition;      // El punto de destino
    public float MoveSpeed { get; set; } = 1f;      // Velocidad de movimiento constante
    public float rotationSpeed = 1f;    // Velocidad de rotación
    private Rigidbody _rigidbody;        // El Rigidbody del barco
    public float arrivalThreshold = 10f; // Umbral de distancia para considerar que el barco ha llegado al destino

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        // Calcular la dirección hacia el destino
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Evitar movimientos en el eje Y (solo movimiento horizontal)
        direction.Normalize(); // Asegurarse de que la dirección sea un vector unitario

        // Establecer la velocidad del Rigidbody para que el barco se mueva a la velocidad deseada
        _rigidbody.velocity = direction * MoveSpeed;

        // Rotar el barco hacia el destino
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold)
        {
            SceneManager.LoadScene("FirstScene");
        }
    }
}
