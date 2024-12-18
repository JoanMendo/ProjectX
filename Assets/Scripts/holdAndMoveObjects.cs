using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdAndMoveObjects : MonoBehaviour
{
    public float pickupDistance = 3f; // Distancia máxima para agarrar objetos
    public Transform holdPoint; // Punto frente al jugador donde se sostendrá el objeto
    public float moveSpeed = 10f; // Velocidad de movimiento del objeto

    public Camera playerCamera;
    private Rigidbody pickedObject;
    void Update()
    {
        // Detectar si se mantiene pulsado el botón derecho del mouse
        if (Input.GetMouseButtonDown(0)) // Click derecho inicial
        {
            TryPickObject();
        }
        else if (Input.GetMouseButtonUp(0)) // Soltar al dejar de presionar
        {
            ReleaseObject();
        }

        // Mover el objeto si está agarrado
        if (pickedObject)
        {
            MoveObject();
        }
    }

    void TryPickObject()
    {
        int layer = LayerMask.GetMask("Interactuable");
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, layer))
        {
            // Verificar si el objeto tiene un Rigidbody
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                pickedObject = rb;
                pickedObject.isKinematic = true; // Desactivar física mientras se mueve
            }
        }
    }

    void ReleaseObject()
    {
        if (pickedObject)
        {
            pickedObject.isKinematic = false; // Reactivar física
            pickedObject = null;
        }
    }

    void MoveObject()
    {
        // Mover el objeto al punto designado frente al jugador
        Vector3 targetPosition = holdPoint.position;
        pickedObject.MovePosition(Vector3.Lerp(pickedObject.position, targetPosition, Time.deltaTime * moveSpeed));
    }
}
