
using UnityEngine;

public class RecargarMunicion : MonoBehaviour
{
    public GameObject prefabASpawnear;
    private bool estaEnContactoConMunicion = false;
    private GameObject municionActual;
    public bool puerta_abierta { get; set; } = false;
    public ControladorDisparos disparo;


  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("municion") && puerta_abierta)
        {
            Debug.Log("esta encontrando la municion");
            estaEnContactoConMunicion = true;
            municionActual = other.gameObject;
            disparo.cargado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("municion"))
        {
            estaEnContactoConMunicion = false;

            Destroy(municionActual);

            

        }
    }

    private void Update()
    {
        if (estaEnContactoConMunicion && Input.GetKeyDown(KeyCode.R))
        {
            // Spawnear el prefab en la posición de este objeto
            GameObject objetoInstanciado = Instantiate(prefabASpawnear, transform.position, transform.rotation);
            objetoInstanciado.transform.SetParent(transform);

            // Destruir el objeto de munición
            Destroy(municionActual);
        }
    }
}