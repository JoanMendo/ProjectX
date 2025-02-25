using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarImpacto : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("objetivo alcanzado");
        Destroy(other.gameObject);
    }
}
