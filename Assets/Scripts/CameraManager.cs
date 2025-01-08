using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera temporalCamera;
    public GameObject player;

    public void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }
    public void changeActiveCamera(Camera newCamera)
    {
        temporalCamera = newCamera;
        mainCamera.enabled = false;
        player.SetActive(false);
        temporalCamera.enabled = true;
    }

    public void resetActiveCamera()
    {
        temporalCamera.enabled = false;
        mainCamera.enabled = true;
        player.SetActive(true);
    }    
}
