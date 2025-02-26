using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParabolicThrow : MonoBehaviour
{
    // Tiempo deseado para alcanzar el objetivo
    private Rigidbody _rb;
    private GameObject objective;
    private GameObject canyon;



    
    private Vector3 posicion_orientacion; 

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
         objective = GameObject.Find("Objetivo");
        canyon = GameObject.Find("Canon");
     
    }
    void Start()
    {
        
        
        posicion_orientacion = canyon.transform.forward;
        ApplyForceToReachTarget(posicion_orientacion);
    }

    public void ApplyForceToReachTarget(Vector3 canyon)
    {
        if (_rb != null)
        {
            Vector3 forceDirection = transform.rotation * canyon.normalized;

            // Aplicar la fuerza con la magnitud calculada
            _rb.AddForce(forceDirection * 46f, ForceMode.Impulse);

            Debug.Log($"Impulso aplicado: {forceDirection * 46f}");
        }
    }
}
