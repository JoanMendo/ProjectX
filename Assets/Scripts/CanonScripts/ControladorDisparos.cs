using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDisparos : MonoBehaviour
{
    
    public GameObject posicion_de_disparo;
    public GameObject restosDisparo;
    public GameObject restosSpawning;
    private Transform RestosSpawning;
    public Vector3 targetPosition;
    public GameObject projectil;
    public bool limpiado = false;
    public bool cargado { get; set; } = false;
    public AudioSource sonido;
    public LineRenderer lineRenderer; // Para proyectar la trayectoria
    public int puntosTrayectoria = 30; // Número de puntos en la línea
    public float tiempoSimulacion = 2f; // Tiempo máximo para simular la trayectoria
    public Camera playerCamera; // Cámara del jugador
    public LayerMask capasDeColision; // Capas con las que la trayectoria puede colisionar

    private List<GameObject> restos = new List<GameObject>();

    public void Start()
    {
         RestosSpawning = restosSpawning.transform;
    }



    public void prepararDisparo()
    {
        if (projectil == null)
        {
            return;
        }
        Debug.Log(cargado);

        // Inicializar la lista si es null
        if (restos == null)
        {
            restos = new List<GameObject>();
            limpiado = true; // Si no hay restos, está limpio
        }
        // Verificar si hay restos y manejarlos correctamente
        else if (restos.Count > 0)
        {
            // Crear una lista para los restos a eliminar
            List<GameObject> restosAEliminar = new List<GameObject>();
            limpiado = true; // Asumimos que está limpio inicialmente

            foreach (GameObject resto in restos)
            {
                if (resto == null)
                {
                    restosAEliminar.Add(resto);
                    continue;
                }

                if (Vector3.Distance(resto.transform.position, RestosSpawning.position) < 4f)
                {
                    limpiado = false; // Hay restos cercanos
                    break; // No necesitamos seguir verificando
                }
                else
                {
                    restosAEliminar.Add(resto); // Marcar para eliminar
                }
            }

            // Eliminar los restos marcados fuera del bucle foreach
            foreach (GameObject resto in restosAEliminar)
            {
                restos.Remove(resto);
            }
        }
        else
        {
            // Si la lista está vacía, consideramos que está limpio
            limpiado = true;
        }

        // Verificar condiciones para disparar
        if (!limpiado || !cargado)
        {
            return;
        }

        // Crear el proyectil
        GameObject bullet = Instantiate(projectil, posicion_de_disparo.transform.position, Quaternion.identity);

        // Destruir el primer hijo de restosSpawning
        if (restosSpawning.transform.childCount > 0)
        {
            Destroy(restosSpawning.transform.GetChild(0).gameObject);
        }

        // Generar nuevos restos después de disparar
        if (restosDisparo != null && RestosSpawning != null)
        {
            for (int i = 0; i < UnityEngine.Random.Range(4, 9); i++)
            {
                GameObject resto = Instantiate(restosDisparo, RestosSpawning.position, Quaternion.identity);
                float randomScale = UnityEngine.Random.Range(0.5f, 0.9f);
                resto.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                restos.Add(resto);
            }
        }

        // Marcar como no cargado después de disparar
        cargado = false;
    }



    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("enteoria se esta disparando");
            prepararDisparo();
        }
    }
}