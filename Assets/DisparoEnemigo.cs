using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    public AudioSource audioSourceIncoming;
    public AudioSource audioPared1;
    public AudioSource audioPared2;

    void Start()
    {

        StartCoroutine(CallActivationPeriodically());

    }



    public void disparo()
    {

        // Reproduce el primer audio inmediatamente
        if (audioSourceIncoming != null)
        {
            audioSourceIncoming.Play();
        }
        else
        {
            Debug.LogError("AudioSourceIncoming no asignado.");
        }

        // Usa la corrutina para reproducir el audio de la pared con un retraso
        StartCoroutine(PlayRandomWallAudioWithDelay(2f)); // 2 segundos de retraso

    }


    public IEnumerator PlayRandomWallAudioWithDelay(float delay)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Selecciona un audio aleatorio y lo reproduce
        int randomInt = Random.Range(0, 2);
        switch (randomInt)
        {
            case 0:
                if (audioPared1 != null)
                {
                    audioPared1.Play();
                }
                else
                {
                    Debug.LogError("AudioPared1 no asignado.");
                }
                break;

            case 1:
                if (audioPared2 != null)
                {
                    audioPared2.Play();
                }
                else
                {
                    Debug.LogError("AudioPared2 no asignado.");
                }
                break;
        }
    }


    private System.Collections.IEnumerator CallActivationPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f); // Espera 3 minutos (180 segundos)
            disparo(); // Llama a la función de activación
        }
    }
}
