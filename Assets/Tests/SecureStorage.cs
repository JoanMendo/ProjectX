using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SecureStorage
{
    // Clave de cifrado (debe ser segura y única)
    private static readonly string key = "TuClaveSecreta1234567890"; // Cambia esto por una clave segura

    // ------------------------------------------------------------------------------------
    // Métodos para cifrar y descifrar
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Cifra una cadena de texto usando AES.
    /// </summary>
    public static string Encrypt(string data)
    {
        try
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al cifrar: " + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Descifra una cadena de texto cifrada con AES.
    /// </summary>
    public static string Decrypt(string encryptedData)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al descifrar: " + ex.Message);
            return null;
        }
    }

    // ------------------------------------------------------------------------------------
    // Métodos para guardar y cargar tokens
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Guarda el token de acceso de manera segura.
    /// </summary>
    public static void SaveAccessToken(string token)
    {
        string encryptedToken = Encrypt(token);
        PlayerPrefs.SetString("AccessToken", encryptedToken);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga el token de acceso de manera segura.
    /// </summary>
    public static string LoadAccessToken()
    {
        string encryptedToken = PlayerPrefs.GetString("AccessToken", "");
        return Decrypt(encryptedToken);
    }

    /// <summary>
    /// Guarda el token de refresco de manera segura.
    /// </summary>
    public static void SaveRefreshToken(string token)
    {
        string encryptedToken = Encrypt(token);
        PlayerPrefs.SetString("RefreshToken", encryptedToken);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga el token de refresco de manera segura.
    /// </summary>
    public static string LoadRefreshToken()
    {
        string encryptedToken = PlayerPrefs.GetString("RefreshToken", "");
        return Decrypt(encryptedToken);
    }

    // ------------------------------------------------------------------------------------
    // Métodos para guardar y cargar datos del usuario
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Guarda los datos del usuario de manera segura.
    /// </summary>
    public static void SaveUserData(UserData userData)
    {
        string jsonData = JsonUtility.ToJson(userData);
        string encryptedData = Encrypt(jsonData);
        PlayerPrefs.SetString("UserData", encryptedData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga los datos del usuario de manera segura.
    /// </summary>
    public static UserData LoadUserData()
    {
        string encryptedData = PlayerPrefs.GetString("UserData", "");
        if (string.IsNullOrEmpty(encryptedData))
        {
            Debug.Log("No se encontraron datos del usuario.");
            return null;
        }

        string jsonData = Decrypt(encryptedData);
        return JsonUtility.FromJson<UserData>(jsonData);
    }

    // ------------------------------------------------------------------------------------
    // Métodos para eliminar datos
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Elimina todos los datos guardados (tokens y datos del usuario).
    /// </summary>
    public static void ClearAllData()
    {
        PlayerPrefs.DeleteKey("AccessToken");
        PlayerPrefs.DeleteKey("RefreshToken");
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.Save();
    }
}

// ------------------------------------------------------------------------------------
// Clase para representar los datos del usuario
// ------------------------------------------------------------------------------------

[System.Serializable]
public class UserData
{
    public string userId;
    public string username;
    public string email;
}
