using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminadorBola : MonoBehaviour
{
    public ControladorDisparos bola; 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("esta detectando la colision");
        if (other.gameObject.tag =="municion")
        {
            Destroy(other.gameObject);
            bola.cargado=true;
        }
    }
}
