using UnityEngine;
using System.Collections;

public class StatisticsHandler : MonoBehaviour
{
    // Variables públicas para almacenar las estadísticas
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
                Debug.Log($"Estadísticas cargadas: {stats.Length} registros");

                // Aquí puedes procesar los datos si lo necesitas
                ProcessStatistics();
            }
            else
            {
                lastError = error;
                Debug.LogError($"Error al cargar estadísticas: {error}");
            }
        }));
    }

    private void ProcessStatistics()
    {
        if (statistics == null || statistics.Length == 0) return;

        // Aquí puedes agregar cualquier procesamiento adicional que necesites
        // Por ejemplo, ordenar los datos
        foreach (var stat in statistics)
        {
            // Procesa cada registro de estadísticas según necesites
            Debug.Log($"Procesando estadística: {stat}");
        }
    }

    // Método para obtener las estadísticas más recientes
    public void RefreshStatistics()
    {
        LoadStatistics();
    }

    // Método para verificar si los datos están cargados
    public bool AreStatisticsLoaded()
    {
        return dataLoaded && statistics != null;
    }

    // Método para obtener el último error
    public string GetLastError()
    {
        return lastError;
    }
}
