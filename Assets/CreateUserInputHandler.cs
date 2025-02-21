using UnityEngine;
using TMPro;
using System.Collections;

public class CreateUserInputHandler : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField nicknameInput;
    public TMP_InputField passwordInput;
    public ApiManager apiManager;

    public void ProcessCreateUserInputs()
    {
        if (ValidateInputs())
        {
            string name = nameInput.text;
            string email = emailInput.text;
            string phone = phoneInput.text;
            string nickname = nicknameInput.text;
            string password = passwordInput.text;

            Debug.Log($"Procesando creaci�n de usuario para: {email}");
            StartCoroutine(CreateUserAndHandleResponse(name, email, phone, nickname, password));
        }
        else
        {
            Debug.LogError("Por favor, completa todos los campos requeridos");
        }
    }

    private bool ValidateInputs()
    {
        return nameInput != null && !string.IsNullOrEmpty(nameInput.text) &&
               emailInput != null && !string.IsNullOrEmpty(emailInput.text) &&
               phoneInput != null && !string.IsNullOrEmpty(phoneInput.text) &&
               nicknameInput != null && !string.IsNullOrEmpty(nicknameInput.text) &&
               passwordInput != null && !string.IsNullOrEmpty(passwordInput.text);
    }

    private IEnumerator CreateUserAndHandleResponse(string name, string email, string phone, string nickname, string password)
    {
        yield return StartCoroutine(apiManager.CreateUser(name, email, phone, nickname, password, (success, message) => {
            if (success)
            {
                Debug.Log(message);
                ClearInputs();
                // Aqu� podr�as redirigir al usuario a la pantalla de login
                // O mostrar un mensaje de �xito en la UI
            }
            else
            {
                Debug.LogError($"Error en la creaci�n de usuario: {message}");
                // Aqu� podr�as mostrar el mensaje de error en la UI
            }
        }));
    }

    private void ClearInputs()
    {
        nameInput.text = "";
        emailInput.text = "";
        phoneInput.text = "";
        nicknameInput.text = "";
        passwordInput.text = "";
    }
}