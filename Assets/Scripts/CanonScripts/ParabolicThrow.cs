using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParabolicThrow : MonoBehaviour
{
    public float mass = 1f;          // Masa del objeto
    public float timeToReachTarget = 2f; // Tiempo deseado para alcanzar el objetivo
    private Rigidbody _rb;
    private GameObject objective;
    private GameObject canyon;



    private Vector3 posicion_enemiga;
    private Vector3 posicion_orientacion; 

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
         objective = GameObject.Find("Objetivo");
        canyon = GameObject.Find("rotacion");
     
    }
    void Start()
    {
        
        posicion_enemiga = objective.transform.position;
        posicion_orientacion = canyon.transform.forward;
        ApplyForceToReachTarget(posicion_enemiga,posicion_orientacion);
    }

    public void ApplyForceToReachTarget(Vector3 target,Vector3 canyon)
    {
        if (timeToReachTarget <= 0)
        {
            Debug.LogError("El tiempo para alcanzar el objetivo debe ser mayor que cero.");
            return;
        }
        Debug.Log("esta aplicando la fuerza");
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
        float magnitude = impulse.magnitude;
       


        if (_rb != null)
        {
            Vector3 forceDirection = transform.rotation * canyon.normalized;

            // Aplicar la fuerza con la magnitud calculada
            _rb.AddForce(forceDirection * magnitude, ForceMode.Impulse);

            Debug.Log($"Impulso aplicado: {impulse}");
        }
    }
}
