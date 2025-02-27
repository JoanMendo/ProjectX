using UnityEngine;
using System.Collections;
using TMPro;

public class StatisticsHandler : MonoBehaviour
{
    public StaticsData[] statistics;
    public bool dataLoaded = false;
    public string lastError = "";

    public TMP_Text deadSoldiersText;
    public TMP_Text shotsFiredText;
    public TMP_Text shipsDestroyedText;
    public TMP_Text lossesText;
    public TMP_Text winsText;
    public TMP_Text rankText;

    private ApiManager apiManager;

    private void Start()
    {
        apiManager = FindObjectOfType<ApiManager>();
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

                ProcessStatistics();
                UpdateStatisticsUI();
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

        foreach (var stat in statistics)
        {
            Debug.Log($"Procesando estadística: {stat}");
        }
    }

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
    private void UpdateStatisticsUI()
    {
        if (statistics == null || statistics.Length == 0) return;

        StaticsData lastStat = statistics[statistics.Length - 1];

        deadSoldiersText.text = lastStat.soldier_died.ToString();
        shotsFiredText.text = lastStat.shoots_made.ToString();
        shipsDestroyedText.text = lastStat.ships_detroyed.ToString();
        lossesText.text = lastStat.loses.ToString();
        winsText.text = lastStat.wins.ToString();
        rankText.text = lastStat.rank.ToString();
    }
}
