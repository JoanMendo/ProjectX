using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleFire : MonoBehaviour
{
    [SerializeField] GameObject fire;
    public static HandleFire instance;
    public bool isFireActive;
    void Start()
    {
        fire.SetActive(false);
        instance = this;
        isFireActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detecta la colision");
        if (other.gameObject.CompareTag("TourchSpot"))
        {
            Debug.Log("se deberia encender");
            StartCoroutine(ActivateFire());
        }
    }

    private IEnumerator ActivateFire()
    {
        yield return new WaitForSeconds(2);
        fire.SetActive(true);
        isFireActive = true;
    }
}