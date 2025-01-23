using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDisparos : MonoBehaviour
{
    public GameObject cilindro;
    public GameObject posicion_de_disparo;
    public GameObject projectil;
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

        Debug.Log("Se ha generado la bala");
        GameObject bullet = Instantiate(projectil, posicion_de_disparo.transform.position, Quaternion.identity);
        ProyectilConFuerza disparando = bullet.GetComponent<ProyectilConFuerza>();

        if (disparando != null)
        {
            sonido.Play();
            disparando.Disparar(posicion_de_disparo);
            // Llama a la funci�n que necesitas
        }
        else
        {
            Debug.LogWarning("El script ProyectilConFuerza no se encuentra en el objeto instanciado.");
        }
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

        // Fuerza inicial aplicada (convertida en velocidad inicial seg�n masa y ForceMode.Impulse)
        Rigidbody rb = projectil.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("El proyectil necesita un Rigidbody.");
            return;
        }

        float masa = rb.mass; // Masa del proyectil
        Vector3 velocidadInicial = (direccion * 50f + Vector3.up * 5f) / masa;

        // Configurar LineRenderer
        lineRenderer.positionCount = puntosTrayectoria;


        for (int i = 0; i < puntosTrayectoria; i++)
        {
            // Tiempo basado en el �ndice
            float tiempo = i * (tiempoSimulacion / puntosTrayectoria);

            // Calcular posici�n usando f�sica
            Vector3 desplazamiento = velocidadInicial * tiempo + 0.5f * Physics.gravity * tiempo * tiempo;
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




    private void Update()
    {

        MostrarTrayectoria();



        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("enteoria se esta disparando");
            prepararDisparo();

        }




    }
}