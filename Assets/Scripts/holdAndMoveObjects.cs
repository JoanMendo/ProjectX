using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndMoveObjects : MonoBehaviour
{
    public float pickupDistance = 3f; // Distancia máxima para agarrar objetos
    public float moveSpeed = 10f; // Velocidad de movimiento del objeto

    public Camera playerCamera;
    private Rigidbody grabbedObject;
    void Update()
    {
        // Detectar si se mantiene pulsado el botón derecho del mouse
        if (Input.GetMouseButtonDown(0)) // Click derecho inicial
        {
            TryPickObject();
        }
        else if (Input.GetMouseButtonDown(2))
            PushObject();
        else if (Input.GetMouseButtonUp(0)) // Soltar al dejar de presionar
        {
            ReleaseObject();
        }
  
    }

    private void FixedUpdate()
    {
        if (grabbedObject)
        {
            MoveObject();
        }
    }

    void TryPickObject()
    {
        int layer = LayerMask.GetMask("Movable");
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, layer))
        {
            // Verificar si el objeto tiene un Rigidbody
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false; // Desactivar gravedad
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject)
        {
            grabbedObject.useGravity = true; // Activar gravedad
            grabbedObject.AddForce(transform.up * 3, ForceMode.Impulse);
            grabbedObject = null;
        }
    }

    void MoveObject()
    {
        // Mover el objeto al punto designado frente al jugador
        if (Vector3.Distance(playerCamera.transform.position, grabbedObject.transform.position) > pickupDistance + 0.8f)
        {
            ReleaseObject();
            return;
        }
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * pickupDistance;

        if (Vector3.Distance(targetPosition, grabbedObject.position) > 1.7f)
        {
            ReleaseObject();
            return;
        }

        Vector3 direction = targetPosition - grabbedObject.position;



        // Aplica fuerza para mover el objeto hacia el punto de agarre
        grabbedObject.velocity = direction * 10f;

        // Limita la velocidad para evitar atraviesos
        grabbedObject.velocity = Vector3.ClampMagnitude(grabbedObject.velocity, 10f);
    }

    void PushObject()
    {
        if (grabbedObject)
        {
            grabbedObject.useGravity = true; // Activar gravedad
            grabbedObject.AddForce(playerCamera.transform.forward * 40f, ForceMode.Impulse);
            grabbedObject = null;
        }
    }
}
