using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    [Header("Canva")]
    public GameObject canva;
    public TMP_Text textAngleX;
    public TMP_Text textAngleY;

    [Header("Canon")]
    public Transform rotationX;
    public Transform rotationY;

    private bool playerInside = false;

    void Start()
    {
        canva.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canva.SetActive(true);
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canva.SetActive(false);
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside)
        {
            textAngleX.text = "X ANGLE: " + rotationX.eulerAngles.x.ToString("F2") + "°";
            textAngleY.text = "Y ANGLE: " + rotationY.eulerAngles.y.ToString("F2") + "°";
        }
    }
}