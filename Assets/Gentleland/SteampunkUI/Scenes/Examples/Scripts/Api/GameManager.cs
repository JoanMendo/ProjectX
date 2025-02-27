using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager; // Instancia estática

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject); // No se destruye entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Índice de escena inválido: {sceneIndex}");
        }
    }

   
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    public void LoadSceneByIndexAsync(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }
        else
        {
            Debug.LogError($"Índice de escena inválido: {sceneIndex}");
        }
    }

    private System.Collections.IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            Debug.Log($"Cargando... {operation.progress * 100}%");
            yield return null;
        }
    }
}