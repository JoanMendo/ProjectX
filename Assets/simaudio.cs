using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simaudio : MonoBehaviour
{

    public AudioSource audioSource; // Referencia al AudioSource

    void Update()
    {
        // Comprobamos si se presiona la tecla T
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Activamos el audio si está asignado
            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource no asignado en el inspector");
            }
        }
    }
}
