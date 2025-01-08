using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{

    public Camera playerCamera;
    // Configuración de la distancia del raycast
    public float distanciaRaycast = 5f;

    // La tecla para activar la interacción
    public KeyCode teclaInteraccion = KeyCode.E;

    void Update()
    {
        // Detecta si se presiona la tecla especificada
        if (Input.GetKeyDown(teclaInteraccion))
        {
            RealizarInteraccion();
        }
    }

    void RealizarInteraccion()
    {
        int layer = LayerMask.GetMask("Interactuable");
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, distanciaRaycast, layer))
        {
                IInteractable interactuable = hit.collider.GetComponent<IInteractable>();

                if (interactuable != null)
                {
                    interactuable.Interact();
                }
        }
    }
}
