using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class LoginResponse
{
    public string accessToken;
    public string refreshToken;
    public string error;
}

[System.Serializable]
public class RefreshResponse
{
    public string accessToken;
    public string error;
}

[System.Serializable]
public class ApiResponse
{
    public string message;
    public string error;
    public int id;
}

public class ApiManager : MonoBehaviour
{
    private string baseUrl = "https://projectoxdatabase.onrender.com";
    private float refreshInterval = 45 * 60; // 45 minutos para mayor seguridad
    private bool isAutoRefreshActive = false;

    // -------------------------------
    // LOGIN
    // -------------------------------
    public IEnumerator Login(string email, string password, System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/login";
        var loginData = new { email, password };
        Debug.Log("Datos JSON a enviar: " + loginData);
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Datos JSON a enviar: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 10;

            yield return request.SendWebRequest();

            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Login", onComplete);
                yield break;
            }

            try
            {
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                SecureStorage.SaveAccessToken(response.accessToken);
                SecureStorage.SaveRefreshToken(response.refreshToken);

                isAutoRefreshActive = true;
                StartCoroutine(AutoRefreshToken());

                Debug.Log("Login exitoso");
                onComplete?.Invoke(true, "Login exitoso");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error crítico en Login: {e.Message}");
                onComplete?.Invoke(false, "Error en el procesamiento de la respuesta");
            }
        }
    }

    // -------------------------------
    // TOKEN MANAGEMENT
    // -------------------------------
    private IEnumerator AutoRefreshToken()
    {
        while (isAutoRefreshActive)
        {
            yield return new WaitForSeconds(refreshInterval);

            string currentRefreshToken = SecureStorage.LoadRefreshToken();
            if (string.IsNullOrEmpty(currentRefreshToken))
            {
                Debug.LogError("No hay refresh token disponible");
                isAutoRefreshActive = false;
                yield break;
            }

            yield return RefreshAccessToken(currentRefreshToken);
        }
    }

    public IEnumerator RefreshAccessToken(string refreshToken)
    {
        string url = $"{baseUrl}/refresh-token";
        var refreshData = new { refreshToken };
        string jsonData = JsonUtility.ToJson(refreshData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                if (request.responseCode == 403)
                {
                    Logout();
                }
                HandleRequestError(request, "Refresh Token", null);
                yield break;
            }

            try
            {
                RefreshResponse response = JsonUtility.FromJson<RefreshResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError($"Error al refrescar token: {response.error}");
                    yield break;
                }

                SecureStorage.SaveAccessToken(response.accessToken);
                Debug.Log("Token actualizado correctamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error crítico al refrescar token: {e.Message}");
            }
        }
    }

    // -------------------------------
    // USER MANAGEMENT
    // -------------------------------
    public IEnumerator GetUsers(System.Action<UserData[], string> onComplete)
    {
        string url = $"{baseUrl}/users";
        string accessToken = SecureStorage.LoadAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            onComplete?.Invoke(null, "No hay token de acceso disponible");
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Get Users", null);
                onComplete?.Invoke(null, "Error al obtener usuarios");
                yield break;
            }

            try
            {
                UserData[] users = JsonUtility.FromJson<UserData[]>(request.downloadHandler.text);
                onComplete?.Invoke(users, null);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al procesar usuarios: {e.Message}");
                onComplete?.Invoke(null, "Error al procesar la respuesta");
            }
        }
    }

    public IEnumerator UpdateUser(string userId, UserData userData, System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/actualizar-usuario/{userId}";
        string accessToken = SecureStorage.LoadAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            onComplete?.Invoke(false, "No hay token de acceso disponible");
            yield break;
        }

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            string jsonData = JsonUtility.ToJson(userData);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Update User", onComplete);
                yield break;
            }

            try
            {
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                SecureStorage.SaveUserData(userData);
                onComplete?.Invoke(true, "Usuario actualizado exitosamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al actualizar usuario: {e.Message}");
                onComplete?.Invoke(false, "Error al procesar la respuesta");
            }
        }
    }

    public IEnumerator DeleteUser(string userId, System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/eliminar-usuario/{userId}";
        string accessToken = SecureStorage.LoadAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            onComplete?.Invoke(false, "No hay token de acceso disponible");
            yield break;
        }

        using (UnityWebRequest request = new UnityWebRequest(url, "DELETE"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Delete User", onComplete);
                yield break;
            }

            try
            {
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                onComplete?.Invoke(true, "Usuario eliminado exitosamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al eliminar usuario: {e.Message}");
                onComplete?.Invoke(false, "Error al procesar la respuesta");
            }
        }
    }

    // -------------------------------
    // UTILITIES
    // -------------------------------
    private void HandleRequestError(UnityWebRequest request, string context, System.Action<bool, string> onComplete)
    {
        string errorMessage = $"{context} Error: {request.error}";

        if (request.responseCode > 0)
        {
            errorMessage += $"\nCódigo de estado: {request.responseCode}";

            try
            {
                var errorResponse = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(errorResponse.error))
                {
                    errorMessage = errorResponse.error;
                }
            }
            catch { }
        }

        Debug.LogError(errorMessage);
        onComplete?.Invoke(false, errorMessage);

        if (request.responseCode == 401)
        {
            Debug.LogError("Sesión expirada, requiere nuevo login");
            Logout();
        }
    }

    public void Logout()
    {
        SecureStorage.ClearAllData();
        isAutoRefreshActive = false;
        StopAllCoroutines();
        Debug.Log("Sesión cerrada correctamente");
    }
}