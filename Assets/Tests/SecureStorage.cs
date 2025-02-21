using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

// ------------------------------------------------------------------------------------

[System.Serializable]
public class UserData
{
    public string name;
    public string email;
    public string phone;
    public string nickname;
}

[System.Serializable]
public class PlayData
{
    public int soldier_used;
    public int shoot_made;
    public int ship_sinked;
    public float time_left;
    public int points;
    public bool win;
}

[System.Serializable]
public class StaticsData
{
    public string date;
    public int games_played;
    public int games_won;
}

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

    public static void SaveAccessToken(string token)
    {
        string encryptedToken = Encrypt(token);
        PlayerPrefs.SetString("AccessToken", encryptedToken);
        PlayerPrefs.Save();
    }

    public static string LoadAccessToken()
    {
        string encryptedToken = PlayerPrefs.GetString("AccessToken", "");
        return Decrypt(encryptedToken);
    }

    public static void SaveRefreshToken(string token)
    {
        string encryptedToken = Encrypt(token);
        PlayerPrefs.SetString("RefreshToken", encryptedToken);
        PlayerPrefs.Save();
    }

    public static string LoadRefreshToken()
    {
        string encryptedToken = PlayerPrefs.GetString("RefreshToken", "");
        return Decrypt(encryptedToken);
    }

    // ------------------------------------------------------------------------------------
    // Métodos para guardar y cargar datos
    // ------------------------------------------------------------------------------------

    public static void SaveUserData(UserData userData)
    {
        string jsonData = JsonConvert.SerializeObject(userData);
        string encryptedData = Encrypt(jsonData);
        PlayerPrefs.SetString("UserData", encryptedData);
        PlayerPrefs.Save();
    }

    public static UserData LoadUserData()
    {
        string encryptedData = PlayerPrefs.GetString("UserData", "");
        if (string.IsNullOrEmpty(encryptedData))
        {
            Debug.Log("No se encontraron datos del usuario.");
            return null;
        }

        string jsonData = Decrypt(encryptedData);
        return JsonConvert.DeserializeObject<UserData>(jsonData);
    }

    public static void SavePlayData(PlayData playData)
    {
        string jsonData = JsonConvert.SerializeObject(playData);
        string encryptedData = Encrypt(jsonData);
        PlayerPrefs.SetString("PlayData", encryptedData);
        PlayerPrefs.Save();
    }

    public static PlayData LoadPlayData()
    {
        string encryptedData = PlayerPrefs.GetString("PlayData", "");
        if (string.IsNullOrEmpty(encryptedData))
        {
            Debug.Log("No se encontraron datos de la partida.");
            return null;
        }

        string jsonData = Decrypt(encryptedData);
        return JsonConvert.DeserializeObject<PlayData>(jsonData);
    }

    public static void SaveStaticsData(StaticsData[] staticsData)
    {
        string jsonData = JsonConvert.SerializeObject(staticsData);
        string encryptedData = Encrypt(jsonData);
        PlayerPrefs.SetString("StaticsData", encryptedData);
        PlayerPrefs.Save();
    }

    public static StaticsData[] LoadStaticsData()
    {
        string encryptedData = PlayerPrefs.GetString("StaticsData", "");
        if (string.IsNullOrEmpty(encryptedData))
        {
            Debug.Log("No se encontraron datos de estadísticas.");
            return null;
        }

        string jsonData = Decrypt(encryptedData);
        return JsonConvert.DeserializeObject<StaticsData[]>(jsonData);
    }

    // ------------------------------------------------------------------------------------
    // Métodos para eliminar datos
    // ------------------------------------------------------------------------------------

    public static void ClearAllData()
    {
        PlayerPrefs.DeleteKey("AccessToken");
        PlayerPrefs.DeleteKey("RefreshToken");
        PlayerPrefs.DeleteKey("UserData");
        PlayerPrefs.DeleteKey("PlayData");
        PlayerPrefs.DeleteKey("StaticsData");
        PlayerPrefs.Save();
    }
}
