using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveSystem
{

    static readonly string key = "DjxlxSCQ6IilZ3YAS3IdPCjWSNIKVgfv";
    static readonly string iv = "Jr87yZkNxaXmTFQm";

    //public static void SavePlayerData(PlayerData metaData)
    //{
    //    PlayerData info = metaData;
    //    BinaryFormatter binaryFormatter = new BinaryFormatter();

    //    string path = Application.persistentDataPath + "/playerData.save";

    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    binaryFormatter.Serialize(stream, info);
    //    stream.Close();
    //}

    //public static PlayerData LoadPlayerData()
    //{
    //    string path = Application.persistentDataPath + "/playerData.save";

    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter binaryFormatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);
    //        PlayerData info = binaryFormatter.Deserialize(stream) as PlayerData;
    //        stream.Close();

    //        return info;
    //    }
    //    else
    //    {
    //        Debug.Log("No Save file found");
    //        return new PlayerData();
    //    }
    //}

    public static void SavePlayerData(PlayerData playerData)
    {
        string jsonText = JsonUtility.ToJson(playerData);

        Aes myAes = Aes.Create();
        myAes.Key = Encoding.ASCII.GetBytes(key);
        myAes.IV = Encoding.ASCII.GetBytes(iv);
        byte[] encrypted = EncryptStringToBytes_Aes(jsonText, myAes.Key, myAes.IV);

        string path = Application.persistentDataPath + "/playerData.save";
        File.WriteAllBytes(path, encrypted);
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.save";
        if (!File.Exists(path))
        {
            return new PlayerData();
        }

        Aes myAes = Aes.Create();
        myAes.Key = Encoding.ASCII.GetBytes(key);
        myAes.IV = Encoding.ASCII.GetBytes(iv);
        byte[] cypherText = File.ReadAllBytes(path);

        string decrypted = DecryptStringFromBytes_Aes(cypherText, myAes.Key, myAes.IV);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(decrypted);
        return playerData;
    }

    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                //Write all data to the stream.
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }
        return encrypted;
    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);

            // Read the decrypted bytes from the decrypting stream
            // and place them in a string.
            plaintext = srDecrypt.ReadToEnd();
        }

        return plaintext;
    }
}