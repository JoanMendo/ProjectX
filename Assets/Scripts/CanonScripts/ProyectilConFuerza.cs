using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProyectilConFuerza : MonoBehaviour
{
    public float fuerza = 500f; // Magnitud de la fuerza aplicada al proyectil.
    public float alturaAdicional = 1f; // Fuerza adicional hacia arriba para un tiro parabólico más alto.

    

  public void Disparar(GameObject cilindro)
    {
       Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("El proyectil necesita un Rigidbody.");
            return;
        }

        // Calcula la dirección de disparo en base a la orientación del cañón.
        Vector3 forward = cilindro.transform.forward.normalized;

        // Ajusta la fuerza según el ángulo del cañón.
        Vector3 fuerzaDisparo = forward * fuerza; // Fuerza en la dirección del cañón.
        fuerzaDisparo += Vector3.up * alturaAdicional; // Añade una componente hacia arriba.

        // Aplica la fuerza al proyectil.
        rb.AddForce(fuerzaDisparo, ForceMode.Impulse);
    }
       
    
}
