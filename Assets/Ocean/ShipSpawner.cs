using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject shipPrefab;          // Prefab del barco que va a aparecer
    public Transform[] spawnPoints;        // Puntos de spawn posibles
    public float spawnDelay = 2f;          // Tiempo entre el spawn de cada barco
    public int roundCount = 5;             // Número de rondas (cuántos barcos quieres que aparezcan)
    private int currentRound = 0;          // Contador para las rondas

    void Start()
    {
        // Llamar al método para iniciar el spawn
        StartCoroutine(SpawnShips());
    }

    IEnumerator SpawnShips()
    {
        // Mientras haya rondas disponibles
        while (currentRound < roundCount)
        {
            SpawnShipRandomly();
            currentRound++;

            // Esperar un tiempo antes de spawnear el siguiente barco
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnShipRandomly()
    {
        // Seleccionar un punto de spawn aleatorio
        int randomIndex = currentRound;

        // Instanciar el barco en el punto de spawn seleccionado
        Instantiate(shipPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }
}
