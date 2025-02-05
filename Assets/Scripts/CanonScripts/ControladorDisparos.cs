using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDisparos : MonoBehaviour
{
    public GameObject cilindro;
    public GameObject posicion_de_disparo;
    public Vector3 targetPosition;
    public GameObject projectil;
    public float fuerzaProyectil = 20f;
    public bool limpiado = true;
    public bool cargado = true;
    public AudioSource sonido;
    public LineRenderer lineRenderer; // Para proyectar la trayectoria
    public int puntosTrayectoria = 30; // N�mero de puntos en la l�nea
    public float tiempoSimulacion = 2f; // Tiempo m�ximo para simular la trayectoria
    public Camera playerCamera; // C�mara del jugador
    public LayerMask capasDeColision; // Capas con las que la trayectoria puede colisionar

    public void prepararDisparo()
    {
        if (projectil == null || cilindro == null)
        {
            Debug.LogError("Referencia nula: projectil o cilindro no est� asignado.");
            return;
        }

        if (!limpiado || !cargado)
        {
            Debug.Log("El disparo no est� preparado: limpiado o cargado es falso.");
            return;
        }

        
        GameObject bullet = Instantiate(projectil, posicion_de_disparo.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(cilindro.transform.forward.normalized * fuerzaProyectil, ForceMode.Impulse);

        Debug.Log("Se ha generado la bala");
        sonido.Play();
            
            // Llama a la funci�n que necesitas
        
        
    }

    private void MostrarTrayectoria()
    {
        if (cilindro == null || projectil == null || lineRenderer == null)
        {
            Debug.LogWarning("Faltan referencias para mostrar la trayectoria.");
            return;
        }

        if (playerCamera.isActiveAndEnabled)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
            // Configurar los par�metros iniciales
        Vector3 posicionInicial = posicion_de_disparo.transform.position;
        Vector3 direccion = cilindro.transform.forward.normalized;
       
        Vector3 velocidadProyectil = fuerzaProyectil * direccion;


        // Configurar LineRenderer
        lineRenderer.positionCount = puntosTrayectoria;


        for (int i = 0; i < puntosTrayectoria; i++)
        {
            // Tiempo basado en el �ndice
            float tiempo = i * (tiempoSimulacion / puntosTrayectoria);

            // Calcular posici�n usando f�sica
            Vector3 desplazamiento = velocidadProyectil * tiempo + 0.5f * Physics.gravity * tiempo * tiempo;
            Vector3 nuevaPosicion = posicionInicial + desplazamiento;

            lineRenderer.SetPosition(i, nuevaPosicion);

            // Verificar colisi�n con Raycast
            if (i > 0)
            {
                Vector3 direccionRay = nuevaPosicion - lineRenderer.GetPosition(i - 1);
                float distancia = direccionRay.magnitude;

                if (Physics.Raycast(lineRenderer.GetPosition(i - 1), direccionRay.normalized, out RaycastHit hit, distancia, capasDeColision))
                {
                    lineRenderer.positionCount = i + 1;
                    lineRenderer.SetPosition(i, hit.point);
                    break;
                }
            }
        }
    }


    public void Update()
    {
        MostrarTrayectoria();

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("enteoria se esta disparando");
            prepararDisparo();
        }
    }
}