using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarImpacto : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objetivo alcanzado");
    }
}
