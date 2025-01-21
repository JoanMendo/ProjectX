using UnityEngine;

public class AimController : MonoBehaviour
{
    public Animator cannonAnimator;
    private float animationTime = 0.5f;
    public float animationStep = 0.01f;
    private bool isPlayerInTrigger = false;
    private bool isScrolling = false;


    [SerializeField] Camera AimCam;
    [SerializeField] Camera playerMainCamera;

    private void Start()
    {
        AimCam.enabled = false;
    }
    void Update()
    {
        if (isPlayerInTrigger)
        {
            float mouseInput = Input.GetAxis("Mouse ScrollWheel");

            if (mouseInput != 0)
            {
                isScrolling = true;

                if (mouseInput > 0)
                {
                    animationTime += animationStep;
                }
                else if (mouseInput < 0)
                {
                    animationTime -= animationStep;
                }

                animationTime = Mathf.Clamp01(animationTime);

                cannonAnimator.Play("Aim_Small_Cannon", 0, animationTime);
            }
            else if (isScrolling)
            {
                isScrolling = false;
                cannonAnimator.speed = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            AimCam.enabled = true;
            playerMainCamera.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            AimCam.enabled = false;
            playerMainCamera.enabled = true;
        }
    }
}
