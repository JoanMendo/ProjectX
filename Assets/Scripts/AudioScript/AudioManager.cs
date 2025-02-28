
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    private int _numero = 0;
    public int numero
    {
        get { return _numero; }
        set
        {
            _numero = value;
            ActualizarNumero(_numero);
        }
    }

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [Header("Eventos de FMOD")]
    [SerializeField] private List<EventReference> eventList = new List<EventReference>();

    [Header("Emisores de Sonido")]
    [SerializeField] private List<GameObject> emittersList = new List<GameObject>();

    // Variable para controlar si ha emitido sonido
    private bool haEmitidoSonido = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ActualizarNumero(_numero);
        // Iniciar la corrutina para reproducir sonido cada 15 segundos
        StartCoroutine(ReproducirSonidoPeriodico());
    }

    /// 

    /// Corrutina que reproduce un sonido cada 15 segundos
    /// 

    private IEnumerator ReproducirSonidoPeriodico()
    {
        while (true)
        {
            // Esperar 25 segundos
            yield return new WaitForSeconds(35f);

            // Reproducir el sonido especificado
            PlaySoundAtEmitter(7, 1);

            // Marcar que ha emitido sonido
            haEmitidoSonido = true;

            // Si ha emitido sonido, reproducir otro sonido con emisor aleatorio
            if (haEmitidoSonido)
            {
                // Generar un n�mero aleatorio entre 2 y 4 (ambos inclusive)
                int emisorAleatorio = Random.Range(2, 5); // Random.Range es inclusivo para el m�nimo y exclusivo para el m�ximo

                // Reproducir el segundo sonido
                PlaySoundAtEmitter(4, emisorAleatorio);

                // Opcional: resetear el estado si es necesario para la l�gica del juego
                // haEmitidoSonido = false;
            }
        }
    }

    /// 

    /// Reproduce un sonido una sola vez
    /// 

    /// <param name="eventIndex">�ndice del evento en la lista</param>
    public void PlaySound(int eventIndex)
    {
        if (IsValidEventIndex(eventIndex))
        {
            RuntimeManager.PlayOneShot(eventList[eventIndex]);
        }
    }

    /// 

    /// Reproduce un sonido desde un emisor espec�fico
    /// 

    /// <param name="eventIndex">�ndice del evento en la lista</param>
    /// <param name="emitterIndex">�ndice del emisor en la lista</param>
    public void PlaySoundAtEmitter(int eventIndex, int emitterIndex)
    {
        if (IsValidEventIndex(eventIndex) && IsValidEmitterIndex(emitterIndex))
        {
            RuntimeManager.PlayOneShot(eventList[eventIndex], emittersList[emitterIndex].transform.position);
        }
    }

    /// 

    /// Actualiza el n�mero y reproduce el sonido correspondiente
    /// 

    /// <param name="nuevoNumero">El nuevo valor del n�mero</param>
    private void ActualizarNumero(int nuevoNumero)
    {
        // Reproduce el sonido usando el emisor 3 y el �ndice del evento igual al valor del n�mero
        PlaySoundAtEmitter(nuevoNumero, 3);

        // Si el n�mero es 3, llama a la funci�n FinDePartida
        if (nuevoNumero == 3)
        {
            FinDePartida();
        }
    }

    /// 

    /// Funci�n que se llama cuando el n�mero es 3
    /// 

    public void FinDePartida()
    {
        GameManager.gameManager.LoadSceneByIndex(0);
    }

    private bool IsValidEventIndex(int index)
    {
        return index >= 0 && index < eventList.Count;
    }

    private bool IsValidEmitterIndex(int index)
    {
        return index >= 0 && index < emittersList.Count;
    }
}