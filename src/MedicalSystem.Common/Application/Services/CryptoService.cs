using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace It270.MedicalSystem.Common.Application.Services;

/// <summary>
/// Cryptography tools
/// </summary>
public static class CryptoTools
{
    #region Hash data functions

    /// <summary>
    /// Password generator
    /// </summary>
    /// <param name="length">Password generator length</param>
    /// <returns>Plan text generated password</returns>
    public static string PasswordGenerator(int length = 12)
    {
        // Source code from https://stackoverflow.com/a/54997

        if (length < 8)
            length = 12;

        const string numbers = "1234567890";
        const string specialChars = "!?@#$%&*/-+_=()[]{}<>,.\'\"\\|";
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";

        var res = new StringBuilder();

        var rnd = new Random();

        // Add uppercase characters
        res.Append(uppercaseChars[rnd.Next(uppercaseChars.Length)]);

        // Add lowercase characters
        while (3 < length--)
        {
            res.Append(lowercaseChars[rnd.Next(lowercaseChars.Length)]);
        }

        // Add numbers
        res.Append(numbers[rnd.Next(numbers.Length)]);

        // Add special characters
        res.Append(specialChars[rnd.Next(specialChars.Length)]);

        return res.ToString();
    }

    #endregion

    #region Symmetric encryption functions

    /// <summary>
    /// Encrypt string
    /// </summary>
    /// <param name="plainText">Plain text string</param>
    /// <returns>Base64 encrypted string</returns>
    public static string EncryptString(string plainText)
    {
        using (var aes = Aes.Create())
        {
            // Read key and iv values (generated with random values from aes.Key and aes.IV init variables)
            var keyB64 = Environment.GetEnvironmentVariable("SYSTEM_AES_KEY");
            var vectorB64 = Environment.GetEnvironmentVariable("SYSTEM_AES_IV");

            aes.Key = Convert.FromBase64String(keyB64);
            aes.IV = Convert.FromBase64String(vectorB64);

            // Create encryptor object
            var encryptor = aes.CreateEncryptor();
            byte[] encryptedData;

            // Encryption will be done in a memory stream through a CryptoStream object
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    encryptedData = ms.ToArray();
                }

            }

            var base64Str = Convert.ToBase64String(encryptedData);
            // return HttpUtility.UrlEncode(base64Str);
            return base64Str;
        }
    }

    /// <summary>
    /// Decrypt string
    /// </summary>
    /// <param name="cipherText">Base64 cipher text</param>
    /// <returns>Decrypted string</returns>
    public static string DecryptString(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            var keyB64 = Environment.GetEnvironmentVariable("SYSTEM_AES_KEY");
            var vectorB64 = Environment.GetEnvironmentVariable("SYSTEM_AES_IV");

            aes.Key = Convert.FromBase64String(keyB64);
            aes.IV = Convert.FromBase64String(vectorB64);

            // Create decryptor object
            var decryptor = aes.CreateDecryptor();
            byte[] cipher = Convert.FromBase64String(cipherText);

            // Decryption will be done in a memory stream through a CryptoStream object
            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

    #endregion
}