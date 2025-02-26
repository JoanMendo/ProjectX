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
    public Vector3 finalRotation;

    public void Start()
    {
        initialRotation = handleGameObject.transform.eulerAngles;
    }
    public void Interact()
    {
        Debug.Log("Interact");
        if (coroutine == null)
        {
            coroutine = StartCoroutine(moveHandle());
        }
        
    }

    public IEnumerator moveHandle()
    {
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

   

