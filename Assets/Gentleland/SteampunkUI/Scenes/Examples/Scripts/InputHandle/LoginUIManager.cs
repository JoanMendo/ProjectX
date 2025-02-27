using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoginUIManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public GameObject errorImage;
    public TMP_Text errorMessage;

    private ApiManager apiManager;

    void Start()
    {
        apiManager = FindObjectOfType<ApiManager>();
        loginButton.onClick.AddListener(AttemptLogin);
        errorImage.SetActive(false);
    }

    void AttemptLogin()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorImage.SetActive(!false);
            errorMessage.text = "PLEASE FILL OUT ALL THE FIELDS";
            return;
        }

        StartCoroutine(apiManager.Login(email, password, OnLoginResponse));
    }

    void OnLoginResponse(bool success, string message)
    {
        if (success)
        {
            GameManager.gameManager.LoadSceneByIndex(3);
        }
        else
        {
            errorImage.SetActive(!false);
            errorMessage.text = message;
        }
    }

    /*
    void DisplayError(string message)
    {
        Debug.LogError(message);
    }
    */
}
