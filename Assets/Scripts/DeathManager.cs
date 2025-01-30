using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeathManager : MonoBehaviour
{
    public Transform respawnPosition;
    public float deathForce;
    public static DeathManager Instance;
    private GameObject globalVolume;
    private float vignetteIntensity = 0.2f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: Mantener entre escenas
        globalVolume = GameObject.Find("Global Volume");
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
        player.GetComponent<Rigidbody>().AddForce(transform.up * deathForce, ForceMode.Impulse);
        Coroutine cr = StartCoroutine(vingetteIntensity());
        yield return new WaitForSeconds(5);
        player.transform.position = respawnPosition.position;
        StopCoroutine(cr);
        globalVolume.GetComponent<Volume>().profile.TryGet(out Vignette vignette);
        vignette.intensity.value = vignetteIntensity;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().freezeRotation = true;
        player.GetComponent<FirstPersonController>().enabled = true;
    }

    public IEnumerator vingetteIntensity()
    {
        globalVolume.GetComponent<Volume>().profile.TryGet(out Vignette vignette);
        float intensity = vignette.intensity.value;

        while (intensity < 1)
        {
            intensity += 0.015f;
            vignette.intensity.value = intensity;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
