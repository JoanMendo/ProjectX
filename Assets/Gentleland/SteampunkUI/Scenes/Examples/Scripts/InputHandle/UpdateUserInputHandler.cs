using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UpdateUserInputHandler : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField nicknameInput;
    public Button updateButton;
    public Button deleteButton;
    public TMP_Text errorMessage;
    public GameObject errorImage;
    private ApiManager apiManager;

    private void Start()
    {
        errorImage.SetActive(false);
        apiManager = FindObjectOfType<ApiManager>();
        LoadCurrentUserData();
        updateButton.onClick.AddListener(ProcessUpdateUserInputs);
        deleteButton.onClick.AddListener(DeleteUser);

    }
    private void DeleteUser()
    {
        StartCoroutine(apiManager.DeleteUser((success, message) => {
            if (success)
            {
                Debug.Log(message);
                GameManager.gameManager.LoadSceneByIndex(0); // Si deseas cambiar de escena
            }
            else
            {
                Debug.LogError($"Error al eliminar el usuario: {message}");
            }
        }));
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
        return !string.IsNullOrEmpty(nameInput.text) &&
            !string.IsNullOrEmpty(emailInput.text) &&
            !string.IsNullOrEmpty(phoneInput.text) &&
            !string.IsNullOrEmpty(nicknameInput.text);
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
                    //Debug.Log(message);
                    LoadCurrentUserData();
                    GameManager.gameManager.LoadSceneByIndex(3);

                }
                else
                {
                    errorImage.SetActive(!false);
                    errorMessage.text = "WRONG EMAIL";
                    //Debug.LogError($"Error en la actualización de usuario: {message}");
                }
            }));
    }
}