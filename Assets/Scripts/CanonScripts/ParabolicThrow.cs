using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicThrow : MonoBehaviour
{
    public float mass = 1f;          // Masa del objeto
    public float timeToReachTarget = 2f; // Tiempo deseado para alcanzar el objetivo
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ApplyForceToReachTarget(Vector3 target)
    {
        if (timeToReachTarget <= 0)
        {
            Debug.LogError("El tiempo para alcanzar el objetivo debe ser mayor que cero.");
            return;
        }

        Vector3 startPos = transform.position;
        Vector3 targetPos = target;
        float g = -Physics.gravity.y;  // Magnitud de la gravedad

        // Calcular componentes de la velocidad inicial
        Vector3 horizontalVelocity = (targetPos - startPos) / timeToReachTarget;
        horizontalVelocity.y = 0; // Mantener solo componentes X y Z

        // Componente vertical de la velocidad (fórmula cinemática)
        float verticalVelocity = (targetPos.y - startPos.y) / timeToReachTarget + 0.5f * g * timeToReachTarget;

        Vector3 initialVelocity = new Vector3(
            horizontalVelocity.x,
            verticalVelocity,
            horizontalVelocity.z
        );

        // Aplicar impulso correctamente (masa * velocidad)
        Vector3 impulse = initialVelocity * mass;

        if (_rb != null)
        {
            _rb.AddForce(impulse, ForceMode.Impulse);
            Debug.Log($"Impulso aplicado: {impulse}");
        }
    }
}
