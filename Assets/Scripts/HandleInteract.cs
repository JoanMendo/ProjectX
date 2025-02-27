using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HandleInteract : MonoBehaviour, IInteractable
{
    public float openSpeed = 5;
    public GameObject handleGameObject;
    public RecargarMunicion municion;

    private Coroutine coroutine;
    private Vector3 initialRotation;
    private Vector3 finalRotation;

    public void Start()
    {
        initialRotation = new Vector3(0,0,0);
        finalRotation = new Vector3(0,100,0);
        
    }
    public void Interact()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(moveHandle());
        }  
    }

    public IEnumerator moveHandle()
    {
        Quaternion initialRotationQuat = Quaternion.Euler(initialRotation.x, initialRotation.y, 0);
        Quaternion finalRotationQuat = Quaternion.Euler(finalRotation.x, finalRotation.y, 0);
        
        if (Quaternion.Angle(handleGameObject.transform.localRotation, finalRotationQuat) > 2)
        {
            municion.puerta_abierta = true;
        
            while (Quaternion.Angle(handleGameObject.transform.localRotation, finalRotationQuat) > 2)
            {
                handleGameObject.transform.localRotation = Quaternion.Slerp(handleGameObject.transform.localRotation, finalRotationQuat, Time.deltaTime * openSpeed);
                yield return null;
            }
        }
        
        else
        {
            municion.puerta_abierta = false;
            while (Quaternion.Angle(handleGameObject.transform.localRotation, initialRotationQuat) > 2)
            {
                handleGameObject.transform.localRotation = Quaternion.Slerp(handleGameObject.transform.localRotation, initialRotationQuat, Time.deltaTime * openSpeed);
                yield return null;
            }
        }

        coroutine = null;


    }
}

   

