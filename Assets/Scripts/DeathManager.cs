using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public Transform respawnPosition;
    public float deathForce;
    public static DeathManager Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: Mantener entre escenas
    }


    public void OnDeath()
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in ships)
        {
            ship.transform.position += ship.GetComponent<Rigidbody>().velocity * 10;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(RespawnPlayer(player));
    }

    public IEnumerator RespawnPlayer(GameObject player)
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<Rigidbody>().freezeRotation = false;
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * deathForce, ForceMode.Impulse);
        yield return new WaitForSeconds(5);
        player.transform.position = respawnPosition.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().freezeRotation = true;
        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
