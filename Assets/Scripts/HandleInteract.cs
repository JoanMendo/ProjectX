using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HandleInteract : MonoBehaviour, IInteractable
{
    public float openSpeed = 5;
    public GameObject handleGameObject;

    private Coroutine coroutine;
    private Vector3 initialRotation;
    private Vector3 finalRotation;

    public void Start()
    {
        
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
        initialRotation = handleGameObject.transform.localEulerAngles;
        finalRotation = initialRotation;
        finalRotation.y += 90;
        Quaternion initialRotationQuat = Quaternion.Euler(initialRotation.x, initialRotation.y, 0);
        Quaternion finalRotationQuat = Quaternion.Euler(finalRotation.x, -finalRotation.y, 0);
        
        if (Quaternion.Angle(handleGameObject.transform.rotation, finalRotationQuat) > 2)
        {
            while (Quaternion.Angle(handleGameObject.transform.rotation, finalRotationQuat) > 2)
            {
                handleGameObject.transform.rotation = Quaternion.Slerp(handleGameObject.transform.rotation, finalRotationQuat, Time.deltaTime * openSpeed);
                yield return null;
            }
        }
        
        else
        {
            while (Quaternion.Angle(handleGameObject.transform.rotation, initialRotationQuat) > 2)
            {
                handleGameObject.transform.rotation = Quaternion.Slerp(handleGameObject.transform.rotation, initialRotationQuat, Time.deltaTime * openSpeed);
                yield return null;
            }
        }

        coroutine = null;


    }
}

   

