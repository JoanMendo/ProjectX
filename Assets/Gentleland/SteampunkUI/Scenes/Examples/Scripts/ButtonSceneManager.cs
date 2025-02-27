using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneManager : MonoBehaviour
{
    public static ButtonSceneManager instance;
    public GameObject panel;
    private void Start()
    {
        instance = this;
    }
    public void ChangeScene(string sceneName)
    {
        //Sonido de click en el botón
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame() 
    {
        //Sonido de click en el botón
        Application.Quit();
    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

}
