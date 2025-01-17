using UnityEngine;

public class AimController : MonoBehaviour
{
    public Animator cannonAnimator; // Asigna este Animator desde el Inspector
    private float animationTime;    // Progreso actual de la animación (0 a 1)
    private bool isPaused = false;  // Indica si la animación está pausada

    void Update()
    {
        float mouseInput = Input.GetAxis("Mouse ScrollWheel");

        if (mouseInput != 0)
        {
            // Reiniciar la animación desde el punto pausado si estaba detenida
            if (isPaused)
            {
                AnimatorStateInfo currentState = cannonAnimator.GetCurrentAnimatorStateInfo(0);
                cannonAnimator.Play(currentState.shortNameHash, 0, animationTime);
                isPaused = false;
            }

            // Scroll hacia abajo
            if (mouseInput < 0)
            {
                cannonAnimator.SetBool("IsMovingDown", true);
                cannonAnimator.SetBool("IsMovingUp", false);
            }
            // Scroll hacia arriba
            else if (mouseInput > 0)
            {
                cannonAnimator.SetBool("IsMovingUp", true);
                cannonAnimator.SetBool("IsMovingDown", false);
            }
        }
        else
        {
            // Capturar el estado actual de la animación cuando no hay movimiento
            if (!isPaused)
            {
                AnimatorStateInfo currentState = cannonAnimator.GetCurrentAnimatorStateInfo(0);
                animationTime = currentState.normalizedTime % 1; // Progreso actual de la animación
                isPaused = true;
            }

            // Detener las animaciones de movimiento
            cannonAnimator.SetBool("IsMovingUp", false);
            cannonAnimator.SetBool("IsMovingDown", false);
        }
    }
}
