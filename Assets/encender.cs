using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class encender : MonoBehaviour
{
    public ControladorDisparos disparos;
    [SerializeField] GameObject fire;
    public static encender instance;
    public bool isFireActive;
    void Start()
    {
        fire.SetActive(false);
        instance = this;
        isFireActive = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Detecta la colision");
        if (other.gameObject.CompareTag("TourchSpot"))
        {
            Debug.Log("se deberia encender");
            StartCoroutine(quemandoMecha());
        }
    }

    private IEnumerator quemandoMecha()
    {
        fire.SetActive(true);
        yield return new WaitForSeconds(2);
        fire.SetActive(false);
        disparos.prepararDisparo();

    }
}
