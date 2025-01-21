using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlidosMoviendose : MonoBehaviour
{
    public GameObject targetObject; // Objeto con Animator y AudioSource
    public string animationParameter = "Activado"; // Nombre del par�metro del Animator
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        // Obt�n los componentes Animator y AudioSource
        if (targetObject != null)
        {
            animator = targetObject.GetComponent<Animator>();
            audioSource = targetObject.GetComponent<AudioSource>();

            if (animator == null)
                Debug.LogError("El GameObject no tiene un componente Animator.");

            if (audioSource == null)
                Debug.LogError("El GameObject no tiene un componente AudioSource.");

            // Inicia la llamada peri�dica
            StartCoroutine(CallActivationPeriodically());
        }
        else
        {
            Debug.LogError("No se ha asignado un GameObject.");
        }
    }

    public void ActivateAnimationWithAudioSync()
    {
        if (animator != null && audioSource != null)
        {
            // Aseg�rate de que la animaci�n de Movimiento sea el estado activo
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("Movimiento"))
            {
                // Calcula la velocidad de la animaci�n seg�n la duraci�n del audio
                float audioLength = audioSource.clip.length;
                animator.speed = audioLength / stateInfo.length;

                // Activa el par�metro de animaci�n y reproduce el audio
                animator.SetBool(animationParameter, true);
                audioSource.Play();

                // Llama a una corrutina para esperar y luego restablecer el estado
                StartCoroutine(ResetAfterAnimation(audioLength));
            }
        }
    }

    private System.Collections.IEnumerator ResetAfterAnimation(float delay)
    {
        // Espera la duraci�n del audio
        yield return new WaitForSeconds(delay);

        // Restablece el par�metro de animaci�n y la velocidad
        animator.SetBool(animationParameter, false);
        animator.speed = 1f; // Resetea la velocidad
    }

    private System.Collections.IEnumerator CallActivationPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(180f); // Espera 3 minutos (180 segundos)
            ActivateAnimationWithAudioSync(); // Llama a la funci�n de activaci�n
        }
    }
}
