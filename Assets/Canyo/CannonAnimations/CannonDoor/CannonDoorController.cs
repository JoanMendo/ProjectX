using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonDoorController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask doorLayer;
   
    private AimController aimController; // Referencia al controlador del cañón
    private bool opened = false;

    private void Start()
    {
        aimController = GetComponent<AimController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        if (aimController.IsCannonAligned() != 0)
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, doorLayer))
        {
            opened = !opened;
            animator.SetBool("OpeningDoor", opened);
        }
    }

    public bool IsDoorOpen()
    {
        return opened;
    }
}
