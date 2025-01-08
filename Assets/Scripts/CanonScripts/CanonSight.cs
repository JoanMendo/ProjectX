using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonSight : MonoBehaviour, IInteractable
{
    public CameraManager cameraManager;
    public Camera canonCamera;
    public MonoBehaviour[] canonScripts;
    public void Start()
    {
        if (cameraManager == null)
        {
            cameraManager = GameObject.FindObjectOfType<CameraManager>();
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

}

