using UnityEngine;
using System.Collections;

public class PlayDataHandler : MonoBehaviour
{
    public ApiManager apiManager;

    // Variables para almacenar los datos de la partida
    private int soldierUsed;
    private int shootMade;
    private int shipSinked;
    private float timeLeft;
    private int points;
    private bool win;

    // M�todo para establecer los datos de la partida
    public void SetPlayData(int soldiers, int shoots, int ships, float time, int score, bool victory)
    {
        soldierUsed = soldiers;
        shootMade = shoots;
        shipSinked = ships;
        timeLeft = time;
        points = score;
        win = victory;
    }

    // M�todo para enviar los datos de la partida
    public void SavePlayData()
    {
        StartCoroutine(SavePlayDataCoroutine());
    }

    private IEnumerator SavePlayDataCoroutine()
    {
        Debug.Log("Iniciando guardado de partida...");

        yield return StartCoroutine(apiManager.CreatePlay(
            soldierUsed,
            shootMade,
            shipSinked,
            timeLeft,
            points,
            win,
            (success, message) => {
                if (success)
                {
                    Debug.Log($"Partida guardada exitosamente: {message}");
                    // Aqu� puedes agregar cualquier acci�n adicional despu�s de guardar
                    // Por ejemplo, mostrar un mensaje de �xito en la UI
                    OnPlayDataSaved();
                }
                else
                {
                    Debug.LogError($"Error al guardar la partida: {message}");
                    // Aqu� puedes manejar el error, por ejemplo mostrando un mensaje al usuario
                    OnPlayDataError(message);
                }
            }));
    }

    // M�todo para guardar r�pidamente una partida con todos los datos a la vez
    public void QuickSavePlay(int soldiers, int shoots, int ships, float time, int score, bool victory)
    {
        SetPlayData(soldiers, shoots, ships, time, score, victory);
        SavePlayData();
    }

    // Eventos que puedes personalizar seg�n tus necesidades
    private void OnPlayDataSaved()
    {
        // Aqu� puedes agregar la l�gica que necesites ejecutar despu�s de guardar exitosamente
        // Por ejemplo:
        // - Mostrar una pantalla de resultados
        // - Regresar al men� principal
        // - Actualizar estad�sticas locales
    }

    private void OnPlayDataError(string errorMessage)
    {
        // Aqu� puedes agregar la l�gica para manejar errores
        // Por ejemplo:
        // - Mostrar un mensaje de error en la UI
        // - Intentar guardar localmente para sincronizar despu�s
        // - Ofrecer al usuario la opci�n de reintentar
    }

    // M�todo para verificar si hay datos v�lidos para guardar
    public bool HasValidDataToSave()
    {
        // Puedes agregar aqu� las validaciones que necesites
        return shootMade >= 0 && timeLeft >= 0 && points >= 0;
    }

    // M�todo para limpiar los datos despu�s de guardar
    public void ClearPlayData()
    {
        soldierUsed = 0;
        shootMade = 0;
        shipSinked = 0;
        timeLeft = 0;
        points = 0;
        win = false;
    }
}