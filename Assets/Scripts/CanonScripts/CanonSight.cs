using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonSight : MonoBehaviour, IInteractable
{
    public CameraManager cameraManager;
    public Camera canonCamera;
    public MonoBehaviour[] canonScripts;

    private bool enabled = false;
    public void Start()
    {
        if (cameraManager == null)
        {
            cameraManager = GameObject.FindObjectOfType<CameraManager>();
        }
    }

    public void Update()
    {
        if (enabled && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            Disable();
        }
    }

    public void Interact()
    {
        cameraManager.changeActiveCamera(canonCamera);
        foreach (MonoBehaviour script in canonScripts)
        {
            script.enabled = true;
        }
    }

    public void Disable()
    {
        foreach (MonoBehaviour script in canonScripts)
        {
            script.enabled = false;
        }
        cameraManager.resetActiveCamera();
    }

}

