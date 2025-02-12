using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// Clase para manejar las respuestas de Login
[System.Serializable]
public class LoginResponse
{
    public string accessToken;
    public string refreshToken;
}

// Clase para manejar la respuesta de la renovación del token
[System.Serializable]
public class RefreshResponse
{
    public string accessToken;
}


public class ApiManager : MonoBehaviour
{
    private string baseUrl = "aws-0-eu-west-3.pooler.supabase.com"; // Cambia esto si tu API está en otro servidor

    // Variables para almacenar el token de acceso y el token de actualización
    private string accessToken = "";
    private string refreshToken = "";

    // Tiempo en segundos antes de refrescar el token (55 minutos = 3300 segundos)
    private float refreshInterval = 55 * 60;

    // --------------------------------------------------------------------------------
    // LOGIN: Se llama al endpoint /login para obtener los tokens.
    // --------------------------------------------------------------------------------
    public IEnumerator Login(string email, string password)
    {
        string url = baseUrl + "/login";

        // Crear el JSON con email y contraseña
        string jsonData = "{\"email\": \"" + email + "\", \"password\": \"" + password + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parsear la respuesta para obtener los tokens
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                accessToken = response.accessToken;
                refreshToken = response.refreshToken;
                Debug.Log("Login exitoso. AccessToken: " + accessToken);

                // Inicia el proceso de renovación automática del token.
                StartCoroutine(AutoRefreshToken());
            }
            else
            {
                Debug.LogError("Error en Login: " + request.error);
            }
        }
    }

    // --------------------------------------------------------------------------------
    // REFRESH TOKEN: Se llama al endpoint /refresh-token para obtener un nuevo accessToken.
    // --------------------------------------------------------------------------------
    public IEnumerator RefreshAccessToken()
    {
        string url = baseUrl + "/refresh-token";
        // Se envía el refreshToken en un JSON
        string jsonData = "{\"refreshToken\": \"" + refreshToken + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parseamos la respuesta para obtener el nuevo accessToken
                RefreshResponse response = JsonUtility.FromJson<RefreshResponse>(request.downloadHandler.text);
                accessToken = response.accessToken;
                Debug.Log("Token refrescado: " + accessToken);
            }
            else
            {
                Debug.LogError("Error al refrescar token: " + request.error);
            }
        }
    }

    // --------------------------------------------------------------------------------
    // COROUTINE: Renovación Automática del Token
    // --------------------------------------------------------------------------------
    public IEnumerator AutoRefreshToken()
    {
        while (true)
        {
            // Espera el intervalo establecido (por ejemplo, 55 minutos)
            yield return new WaitForSeconds(refreshInterval);

            // Llama al endpoint para refrescar el token
            yield return StartCoroutine(RefreshAccessToken());
            Debug.Log("Access token automáticamente renovado.");
        }
    }

    // --------------------------------------------------------------------------------
    // GET: Obtener la lista de usuarios. (Endpoint protegido: /users)
    // --------------------------------------------------------------------------------
    public IEnumerator GetUsers()
    {
        string url = baseUrl + "/users";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Se añade el token en la cabecera de autorización
            request.SetRequestHeader("Authorization", accessToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuarios: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al obtener usuarios: " + request.error);
            }
        }
    }

    // --------------------------------------------------------------------------------
    // POST: Crear un usuario. (Endpoint: /crear-usuario)
    // Campos: name, email, phone, nickname.
    // --------------------------------------------------------------------------------
    public IEnumerator CreateUser(string name, string email, string phone, string nickname)
    {
        string url = baseUrl + "/crear-usuario";
        // Construcción del JSON con los datos del usuario.
        string jsonData = "{\"nombre\": \"" + name + "\", " +
                          "\"email\": \"" + email + "\", " +
                          "\"phone\": \"" + phone + "\", " +
                          "\"nickname\": \"" + nickname + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            // Endpoint protegido: se requiere el token de acceso.
            request.SetRequestHeader("Authorization", accessToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuario creado exitosamente: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al crear usuario: " + request.error);
            }
        }
    }

    // --------------------------------------------------------------------------------
    // PUT: Actualizar un usuario. (Endpoint: /actualizar-usuario/:id)
    // --------------------------------------------------------------------------------
    public IEnumerator UpdateUser(int id, string name, string email, string phone, string nickname)
    {
        string url = baseUrl + "/actualizar-usuario/" + id;
        string jsonData = "{\"nombre\": \"" + name + "\", " +
                          "\"email\": \"" + email + "\", " +
                          "\"phone\": \"" + phone + "\", " +
                          "\"nickname\": \"" + nickname + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", accessToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuario actualizado exitosamente: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al actualizar usuario: " + request.error);
            }
        }
    }

    // --------------------------------------------------------------------------------
    // DELETE: Eliminar un usuario. (Endpoint: /eliminar-usuario/:id)
    // --------------------------------------------------------------------------------
    public IEnumerator DeleteUser(int id)
    {
        string url = baseUrl + "/eliminar-usuario/" + id;

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            request.SetRequestHeader("Authorization", accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuario eliminado exitosamente: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al eliminar usuario: " + request.error);
            }
        }
    }
}
