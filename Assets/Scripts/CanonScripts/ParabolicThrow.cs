using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicThrow : MonoBehaviour
{
    public float mass = 1f;   // Masa del objeto
    public float timeToApplyForce = 0.1f; // Tiempo en el que se aplica la fuerza
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyForceToReachTarget(Vector3 target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target;
        float g = -Physics.gravity.y;  // Magnitud de la gravedad

        // Calcular el tiempo de vuelo basado en la componente Y
        float yDisplacement = targetPos.y - startPos.y;
        float t = Mathf.Sqrt(2 * Mathf.Abs(yDisplacement) / g);

        // Calcular la velocidad inicial
        Vector3 velocity = new Vector3(
            (targetPos.x - startPos.x) / t,
            (yDisplacement / t) + 0.5f * g * t,
            (targetPos.z - startPos.z) / t
        );

        // Calcular la fuerza inicial
        Vector3 initialForce = (mass * velocity) / timeToApplyForce;

        // Aplicar la fuerza al Rigidbody
        rb.AddForce(initialForce, ForceMode.Impulse);

        Debug.Log("Fuerza aplicada: " + initialForce);
    }
}
