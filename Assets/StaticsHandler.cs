using UnityEngine;
using System.Collections;

public class StatisticsHandler : MonoBehaviour
{
    // Variables p�blicas para almacenar las estad�sticas
    public StaticsData[] statistics;
    public bool dataLoaded = false;
    public string lastError = "";

    public ApiManager apiManager;

    private void Start()
    {
        apiManager = GameObject.Find("GameManager").GetComponent<ApiManager>();
        LoadStatistics();
    }

    public void LoadStatistics()
    {
        StartCoroutine(LoadStatisticsCoroutine());
    }

    private IEnumerator LoadStatisticsCoroutine()
    {
        dataLoaded = false;
        lastError = "";

        yield return StartCoroutine(apiManager.GetDates((stats, error) => {
            if (stats != null)
            {
                statistics = stats;
                dataLoaded = true;
                Debug.Log($"Estad�sticas cargadas: {stats.Length} registros");

                // Aqu� puedes procesar los datos si lo necesitas
                ProcessStatistics();
            }
            else
            {
                lastError = error;
                Debug.LogError($"Error al cargar estad�sticas: {error}");
            }
        }));
    }

    private void ProcessStatistics()
    {
        if (statistics == null || statistics.Length == 0) return;

        // Aqu� puedes agregar cualquier procesamiento adicional que necesites
        // Por ejemplo, ordenar los datos
        foreach (var stat in statistics)
        {
            // Procesa cada registro de estad�sticas seg�n necesites
            Debug.Log($"Procesando estad�stica: {stat}");
        }
    }

    // M�todo para obtener las estad�sticas m�s recientes
    public void RefreshStatistics()
    {
        LoadStatistics();
    }

    // M�todo para verificar si los datos est�n cargados
    public bool AreStatisticsLoaded()
    {
        return dataLoaded && statistics != null;
    }

    // M�todo para obtener el �ltimo error
    public string GetLastError()
    {
        return lastError;
    }
}
