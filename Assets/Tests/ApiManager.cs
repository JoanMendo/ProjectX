using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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
}

public class ApiManager : MonoBehaviour
{
    private string baseUrl = "https://projectoxdatabase.onrender.com";
    private float refreshInterval = 45 * 60; // 45 minutos para mayor seguridad
    private bool isAutoRefreshActive = false;

    // -------------------------------
    // USER MANAGEMENT
    // -------------------------------
    public IEnumerator CreateUser(string name, string email, string phone, string nickname, string password,
                                System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/crear-usuario";

        var userData = new
        {
            name = name,
            email = email,
            phone = phone,
            nickname = nickname,
            password = password
        };

        string jsonData = JsonConvert.SerializeObject(userData);
        

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Create User", onComplete);
                yield break;
            }

            try
            {
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                // No guardamos datos aquí - el usuario necesita hacer login primero
                onComplete?.Invoke(true, "Usuario creado exitosamente. Por favor, inicia sesión.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al crear usuario: {e.Message}");
                onComplete?.Invoke(false, "Error al procesar la respuesta");
            }
        }
    }

    public IEnumerator Login(string email, string password, System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/login";
        var loginData = new { email, password };
        string jsonData = JsonConvert.SerializeObject(loginData);
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

            // Procesamos la respuesta en una función separada para evitar el `try` dentro del `IEnumerator`
            yield return ProcessLoginResponse(request.downloadHandler.text, onComplete);
        }
    }

    private IEnumerator ProcessLoginResponse(string jsonResponse, System.Action<bool, string> onComplete)
    {
        LoginResponse response;


        try
        {
            response = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error en Login: {e.Message}");
            onComplete?.Invoke(false, "Error en el procesamiento de la respuesta");
            yield break;
        }

        if (!string.IsNullOrEmpty(response.error))
        {
            onComplete?.Invoke(false, response.error);
            yield break;
        }

        SecureStorage.SaveAccessToken(response.accessToken);
        SecureStorage.SaveRefreshToken(response.refreshToken);

        // Obtenemos y guardamos los datos del usuario inmediatamente después del login
        yield return GetUser((userData, error) =>
        {
            if (userData != null)
            {
                Debug.Log("Datos de usuario guardados correctamente tras login");
            }
            else
            {
                Debug.LogWarning("No se pudieron obtener los datos del usuario tras login: " + error);
            }
        });

        isAutoRefreshActive = true;
        StartCoroutine(AutoRefreshToken());

        onComplete?.Invoke(true, "Login exitoso");
    }
    public UserData GetLocalUserData()
    {
        return SecureStorage.LoadUserData();
    }

    public IEnumerator GetUser(System.Action<UserData, string> onComplete)
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
                HandleRequestError(request, "Get User", null);
                onComplete?.Invoke(null, "Error al obtener usuario");
                yield break;
            }

            try
            {
                UserData userData = JsonConvert.DeserializeObject<UserData>(request.downloadHandler.text);

                // Guardamos los datos localmente
                SecureStorage.SaveUserData(userData);

                onComplete?.Invoke(userData, null);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al procesar usuario: {e.Message}");
                onComplete?.Invoke(null, "Error al procesar la respuesta");
            }
        }
    }

   

    public IEnumerator UpdateUser(string name, string email, string phone, string nickname,
                                System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/actualizar-usuario";
        string accessToken = SecureStorage.LoadAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            onComplete?.Invoke(false, "No hay token de acceso disponible");
            yield break;
        }

        // Obtenemos los datos actuales
        UserData currentData = SecureStorage.LoadUserData();

        // Actualizamos solo los campos que han cambiado
        var userData = new UserData
        {
            name = string.IsNullOrEmpty(name) ? currentData?.name : name,
            email = string.IsNullOrEmpty(email) ? currentData?.email : email,
            phone = string.IsNullOrEmpty(phone) ? currentData?.phone : phone,
            nickname = string.IsNullOrEmpty(nickname) ? currentData?.nickname : nickname
        };

        string jsonData = JsonConvert.SerializeObject(userData);

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
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
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                // Si la actualización fue exitosa, guardamos los nuevos datos
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

    // -------------------------------
    // STATISTICS MANAGEMENT
    // -------------------------------
    public IEnumerator GetDates(System.Action<StaticsData[], string> onComplete)
    {
        string url = $"{baseUrl}/dates";
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
                HandleRequestError(request, "Get Dates", null);
                onComplete?.Invoke(null, "Error al obtener estadísticas");
                yield break;
            }

            try
            {
                StaticsData[] statistics = JsonConvert.DeserializeObject<StaticsData[]>(request.downloadHandler.text);

                // Guardamos las estadísticas localmente
                SecureStorage.SaveStaticsData(statistics);

                onComplete?.Invoke(statistics, null);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al procesar estadísticas: {e.Message}");
                onComplete?.Invoke(null, "Error al procesar la respuesta");
            }
        }
    }

    public IEnumerator CreatePlay(int soldierUsed, int shootMade, int shipSinked,
                                float timeLeft, int points, bool win,
                                System.Action<bool, string> onComplete)
    {
        string url = $"{baseUrl}/play";
        string accessToken = SecureStorage.LoadAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            onComplete?.Invoke(false, "No hay token de acceso disponible");
            yield break;
        }

        var playData = new PlayData
        {
            soldier_used = soldierUsed,
            shoot_made = shootMade,
            ship_sinked = shipSinked,
            time_left = timeLeft,
            points = points,
            win = win
        };

        string jsonData = JsonConvert.SerializeObject(playData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleRequestError(request, "Create Play", onComplete);
                yield break;
            }

            try
            {
                ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(response.error))
                {
                    onComplete?.Invoke(false, response.error);
                    yield break;
                }

                // Guardamos los datos de la partida localmente
                SecureStorage.SavePlayData(playData);

                onComplete?.Invoke(true, "Partida creada exitosamente");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al crear partida: {e.Message}");
                onComplete?.Invoke(false, "Error al procesar la respuesta");
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
        string jsonData = JsonConvert.SerializeObject(refreshData);

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
                RefreshResponse response = JsonConvert.DeserializeObject<RefreshResponse>(request.downloadHandler.text);

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
                var errorResponse = JsonConvert.DeserializeObject<ApiResponse>(request.downloadHandler.text);

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