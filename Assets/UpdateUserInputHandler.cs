using UnityEngine;
using TMPro;
using System.Collections;

public class UpdateUserInputHandler : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField nicknameInput;
    public ApiManager apiManager;

    private void Start()
    {
        // Cargar los datos actuales del usuario al iniciar
        LoadCurrentUserData();
    }

    private void LoadCurrentUserData()
    {
        StartCoroutine(apiManager.GetUser((userData, error) => {
            if (userData != null)
            {
                // Establecer los datos actuales como placeholder
                if (nameInput != null)
                {
                    nameInput.placeholder.GetComponent<TMP_Text>().text = userData.name;
                    nameInput.text = userData.name;
                }
                if (emailInput != null)
                {
                    emailInput.placeholder.GetComponent<TMP_Text>().text = userData.email;
                    emailInput.text = userData.email;
                }
                if (phoneInput != null)
                {
                    phoneInput.placeholder.GetComponent<TMP_Text>().text = userData.phone;
                    phoneInput.text = userData.phone;
                }
                if (nicknameInput != null)
                {
                    nicknameInput.placeholder.GetComponent<TMP_Text>().text = userData.nickname;
                    nicknameInput.text = userData.nickname;
                }
            }
            else
            {
                Debug.LogError($"Error al cargar datos del usuario: {error}");
                // Aquí podrías mostrar un mensaje de error en la UI
            }
        }));
    }

    public void ProcessUpdateUserInputs()
    {
        if (ValidateInputs())
        {
            string name = nameInput.text;
            string email = emailInput.text;
            string phone = phoneInput.text;
            string nickname = nicknameInput.text;

            Debug.Log($"Procesando actualización de usuario para: {email}");
            StartCoroutine(UpdateUserAndHandleResponse(name, email, phone, nickname));
        }
        else
        {
            Debug.LogError("Error en la validación de campos");
        }
    }

    private bool ValidateInputs()
    {
        // Verificamos que los InputFields existan, no es necesario que tengan texto
        // ya que usaremos los valores del placeholder si están vacíos
        return nameInput != null && emailInput != null &&
               phoneInput != null && nicknameInput != null;
    }

    private IEnumerator UpdateUserAndHandleResponse(string name, string email, string phone, string nickname)
    {
        yield return StartCoroutine(apiManager.UpdateUser(
            name,
            email,
            phone,
            nickname,
            (success, message) => {
                if (success)
                {
                    Debug.Log(message);
                    // Aquí podrías mostrar un mensaje de éxito en la UI
                    // Y/o recargar los datos actuales
                    LoadCurrentUserData();
                }
                else
                {
                    Debug.LogError($"Error en la actualización de usuario: {message}");
                    // Aquí podrías mostrar el mensaje de error en la UI
                }
            }));
    }
}