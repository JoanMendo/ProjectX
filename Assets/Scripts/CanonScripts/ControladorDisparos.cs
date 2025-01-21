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




    public void prepararDisparo()
    {
        if (projectil == null || cilindro == null)
        {
            Debug.LogError("Referencia nula: projectil o cilindro no está asignado.");
            return;
        }

        if (!limpiado || !cargado)
        {
            Debug.Log("El disparo no está preparado: limpiado o cargado es falso.");
            return;
        }

        Debug.Log("Se ha generado la bala");
        GameObject bullet = Instantiate(projectil, posicion_de_disparo.transform.position, Quaternion.identity);
        ProyectilConFuerza disparando = bullet.GetComponent<ProyectilConFuerza>();

        if (disparando != null)
        {
            sonido.Play();  
            disparando.Disparar(posicion_de_disparo);
            // Llama a la función que necesitas
        }
        else
        {
            Debug.LogWarning("El script ProyectilConFuerza no se encuentra en el objeto instanciado.");
        }
    }

    private void Update()
    {
        Debug.Log($"Limpiado: {limpiado}, Cargado: {cargado}");
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("enteoria se esta disparando");
            prepararDisparo();

        }
       
    }



}
