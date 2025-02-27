using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinstockController : MonoBehaviour
{
    [SerializeField] private GameObject linstock;
    [SerializeField] private Vector3 pos;
    public float pickupDistance = 2f;
    public Camera playerCamera;
    private bool isHolding = false;
    private Transform holdPosition;

    void Start()
    {
        // Crear un objeto vacío como punto de sujeción en la cámara del jugador
        holdPosition = new GameObject("HoldPosition").transform;
        holdPosition.SetParent(playerCamera.transform);
        holdPosition.localPosition = pos; 
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHolding)
            {
                TryPickObject();
            }
            else
            {
                DropObject();
            }
        }

        if (isHolding && linstock != null)
        {
            // Posición base desde el punto de sujeción
            Vector3 adjustedPosition = holdPosition.position;

            // Aplicar los ajustes específicos en coordenadas locales de la cámara
            adjustedPosition += playerCamera.transform.up * -0.2f;      // Y: -0.2 (hacia abajo)
            adjustedPosition += playerCamera.transform.forward * 1.3f; // Z: -1.0 (hacia atrás)

            // Aplicar la posición ajustada
            linstock.transform.position = adjustedPosition;

            // Mantener la rotación como en la opción 3
            Quaternion baseRotation = playerCamera.transform.rotation;
            Quaternion offsetRotation = Quaternion.Euler(90, 0, 0); // Ajusta estos valores según necesites
            linstock.transform.rotation = baseRotation * offsetRotation;
        }
    }

    void TryPickObject()
    {
        int layer = LayerMask.GetMask("Linstock");
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, layer))
        {
            linstock = hit.collider.gameObject;

            Rigidbody rb = linstock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Physics.IgnoreCollision(linstock.GetComponent<Collider>(),
                                       GetComponent<Collider>(), true);
            }

            // Usar worldPositionStays = false para que adopte la rotación del padre
            linstock.transform.SetParent(holdPosition, false);

            // Ajustar la rotación local si es necesario
            linstock.transform.localRotation = Quaternion.identity;

            isHolding = true;
        }
    }

    void DropObject()
    {
        if (linstock != null)
        {
            // Desacoplar del punto de sujeción
            linstock.transform.SetParent(null);

            Rigidbody rb = linstock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                // Reactivar colisiones
                Physics.IgnoreCollision(linstock.GetComponent<Collider>(),
                                       GetComponent<Collider>(), false);
            }

            isHolding = false;
            linstock = null;
        }
    }
}
