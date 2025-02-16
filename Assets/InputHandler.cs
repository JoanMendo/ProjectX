using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    public TMP_InputField input1;
    public TMP_InputField input2;
    public ApiManager apiManager;

    public void ProcessInputs()
    {
        if (input1 != null && input2 != null)
        {
            string value1 = input1.text;
            string value2 = input2.text;
            Debug.Log(value1);
            Debug.Log(value2);
            StartCoroutine(LoginAndHandleResponse(value1, value2));



        }
        else
        {
            Debug.LogError("Por Alguna razon los inputs estan fallando");
        }
    }

    private IEnumerator LoginAndHandleResponse(string email, string password)
    {
        yield return StartCoroutine(apiManager.Login(email, password, (success, message) => {
            if (success)
            {
                GameManager.gameManager.LoadSceneByIndex(1);
            }
            else
            {
                Debug.LogError($"Error en el login: {message}");
                // Aquí podrías mostrar el mensaje de error en la UI
            }
        }));
    }
}
