using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAim : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Animator cannonAnimator;
    [SerializeField] private LayerMask horizontalAimLayer;
    [SerializeField] private GameObject body;

    private float animationProgress = 0f; // Estado inicial en la mitad
    private float animationStep = 0.05f;
    private float mouseInput;

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Solo si el botón izquierdo está presionado
        {
            mouseInput = Input.GetAxis("Mouse X");
            Debug.Log(mouseInput);
            if (body.transform.rotation.y == 0)
            {
                if (mouseInput > 0)
                {
                    MoveRight();
                }
                 if (mouseInput < 0)
                {
                    MoveLeft();
                }
            }
            else if (body.transform.rotation.y > 0)
            {
                MoveLeft();
            }
            else if(body.transform.rotation.y < 0)
            {
                MoveRight();
            }

        }
    }

    void MoveRight()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, horizontalAimLayer))
        {
            if (mouseInput > 0)
            {
                animationProgress += animationStep;
            }
            else if (mouseInput < 0)
            {
                animationProgress -= animationStep;
            }
            animationProgress = Mathf.Clamp(animationProgress, 0f, 1f);
            cannonAnimator.Play("HorizontalAimRight", 0, animationProgress);
            cannonAnimator.speed = 0;
        }
    }

    void MoveLeft()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, horizontalAimLayer))
        {
            if (mouseInput < 0)
            {
                animationProgress += animationStep;
            }
            else if (mouseInput > 0)
            {
                animationProgress -= animationStep;
            }
            animationProgress = Mathf.Clamp(animationProgress, 0f, 1f);
            cannonAnimator.Play("HorizontalAimLeft", 0, animationProgress);
            cannonAnimator.speed = 0;
        }
    }
}