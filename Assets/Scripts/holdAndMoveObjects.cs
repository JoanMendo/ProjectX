using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndMoveObjects : MonoBehaviour
{
    public float pickupDistance = 3f; // Distancia máxima para agarrar objetos
    public float moveSpeed = 10f; // Velocidad de movimiento del objeto

    public Camera playerCamera;
    public Material highlightMaterial;
    private GameObject grabbedObject;
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
                Material[] material = hit.collider.GetComponent<MeshRenderer>().materials;
                Material[] newMaterials = new Material[material.Length + 1];
                for (int i = 0; i < material.Length; i++)
                {
                    newMaterials[i] = material[i]; // Copia los materiales antiguos
                }
                newMaterials[newMaterials.Length - 1] = highlightMaterial; // Añade el nuevo material

                hit.collider.GetComponent<MeshRenderer>().materials = newMaterials;
                grabbedObject = hit.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody>().useGravity = false; // Desactivar gravedad
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject)
        {
            Material[] material = grabbedObject.GetComponent<MeshRenderer>().materials;
            Material[] newMaterials = new Material[material.Length - 1];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = material[i]; // Copia los materiales antiguos
            }
            grabbedObject.GetComponent<MeshRenderer>().materials = newMaterials; // Elimina el material de resaltado
            grabbedObject.GetComponent<Rigidbody>().useGravity = true; // Activar gravedad
            grabbedObject.GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
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

        if (Vector3.Distance(targetPosition, grabbedObject.GetComponent<Rigidbody>().position) > 1.7f)
        {
            ReleaseObject();
            return;
        }

        Vector3 direction = targetPosition - grabbedObject.GetComponent<Rigidbody>().position;



        // Aplica fuerza para mover el objeto hacia el punto de agarre
        grabbedObject.GetComponent<Rigidbody>().velocity = direction * 10f;

        // Limita la velocidad para evitar atraviesos
        grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(grabbedObject.GetComponent<Rigidbody>().velocity, 10f);
    }

    void PushObject()
    {
        if (grabbedObject)
        {
            grabbedObject.GetComponent<Rigidbody>().useGravity = true; // Activar gravedad
            grabbedObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 40f, ForceMode.Impulse);
            grabbedObject = null;
        }
    }
}
