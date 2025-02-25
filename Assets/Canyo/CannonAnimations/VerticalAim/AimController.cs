using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Animator cannonAnimator;
    [SerializeField] private LayerMask verticalAimLayer;
    [SerializeField] private GameObject body;
    private CannonDoorController doorController; // Referencia al controlador de la puerta

    private float animationProgress = 0.5f; // Estado inicial en la mitad
    private float animationStep = 0.05f;
    private float mouseInput;

    private void Start()
    {
        doorController = GetComponent<CannonDoorController>();
    }
    private void Update()
    {
        if (!doorController.IsDoorOpen()) // Solo permitir apuntar si la puerta está cerrada
        {
            mouseInput = Input.GetAxis("Mouse ScrollWheel");

            if (mouseInput != 0)
            {
                VerticalAim();
            }
        }
    }

    void VerticalAim()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, verticalAimLayer))
        {
            if (mouseInput > 0)
            {
                animationProgress += animationStep;
            }
            else if (mouseInput < 0)
            {
                animationProgress -= animationStep;
            }

            animationProgress = Mathf.Clamp01(animationProgress);

            cannonAnimator.Play("VerticalAim", 0, animationProgress);
            cannonAnimator.speed = 0; // Pausar la animación en el último frame alcanzado
        }
    }

    public float IsCannonAligned()
    {
        return body.transform.rotation.x;
    }
}
