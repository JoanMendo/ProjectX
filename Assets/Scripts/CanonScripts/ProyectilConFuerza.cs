using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProyectilConFuerza : MonoBehaviour
{
    public float fuerza = 500f; // Magnitud de la fuerza aplicada al proyectil.
    public float alturaAdicional = 1f; // Fuerza adicional hacia arriba para un tiro parab�lico m�s alto.

    

  public void Disparar(GameObject cilindro)
    {
       Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("El proyectil necesita un Rigidbody.");
            return;
        }

        // Calcula la direcci�n de disparo en base a la orientaci�n del ca��n.
        Vector3 forward = cilindro.transform.forward.normalized;

        // Ajusta la fuerza seg�n el �ngulo del ca��n.
        Vector3 fuerzaDisparo = forward * fuerza; // Fuerza en la direcci�n del ca��n.
        fuerzaDisparo += Vector3.up * alturaAdicional; // A�ade una componente hacia arriba.

        // Aplica la fuerza al proyectil.
        rb.AddForce(fuerzaDisparo, ForceMode.Impulse);
    }
       
    
}
