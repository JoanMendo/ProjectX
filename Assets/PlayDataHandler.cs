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

    // Método para establecer los datos de la partida
    public void SetPlayData(int soldiers, int shoots, int ships, float time, int score, bool victory)
    {
        soldierUsed = soldiers;
        shootMade = shoots;
        shipSinked = ships;
        timeLeft = time;
        points = score;
        win = victory;
    }

    // Método para enviar los datos de la partida
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
                    // Aquí puedes agregar cualquier acción adicional después de guardar
                    // Por ejemplo, mostrar un mensaje de éxito en la UI
                    OnPlayDataSaved();
                }
                else
                {
                    Debug.LogError($"Error al guardar la partida: {message}");
                    // Aquí puedes manejar el error, por ejemplo mostrando un mensaje al usuario
                    OnPlayDataError(message);
                }
            }));
    }

    // Método para guardar rápidamente una partida con todos los datos a la vez
    public void QuickSavePlay(int soldiers, int shoots, int ships, float time, int score, bool victory)
    {
        SetPlayData(soldiers, shoots, ships, time, score, victory);
        SavePlayData();
    }

    // Eventos que puedes personalizar según tus necesidades
    private void OnPlayDataSaved()
    {
        // Aquí puedes agregar la lógica que necesites ejecutar después de guardar exitosamente
        // Por ejemplo:
        // - Mostrar una pantalla de resultados
        // - Regresar al menú principal
        // - Actualizar estadísticas locales
    }

    private void OnPlayDataError(string errorMessage)
    {
        // Aquí puedes agregar la lógica para manejar errores
        // Por ejemplo:
        // - Mostrar un mensaje de error en la UI
        // - Intentar guardar localmente para sincronizar después
        // - Ofrecer al usuario la opción de reintentar
    }

    // Método para verificar si hay datos válidos para guardar
    public bool HasValidDataToSave()
    {
        // Puedes agregar aquí las validaciones que necesites
        return shootMade >= 0 && timeLeft >= 0 && points >= 0;
    }

    // Método para limpiar los datos después de guardar
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