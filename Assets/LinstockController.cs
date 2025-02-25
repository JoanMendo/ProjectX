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
            linstock.transform.position = holdPosition.position;
            linstock.transform.rotation = holdPosition.rotation;
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
                rb.isKinematic = true; // Desactiva la física mientras está en la mano
            }
            isHolding = true;
        }
    }

    void DropObject()
    {
        if (linstock != null)
        {
            Rigidbody rb = linstock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Reactiva la física al soltar
            }
            isHolding = false;
            linstock = null;
        }
    }
}
