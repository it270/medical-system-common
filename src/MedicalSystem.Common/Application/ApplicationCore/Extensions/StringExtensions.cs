using System;
using It270.MedicalSystem.Common.Application.ApplicationCore.Services;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;

public static class StringExtensions
{
    #region Cast functions

    /// <summary>
    /// Cast string to enum
    /// </summary>
    /// <typeparam name="E">Enum type</typeparam>
    /// <param name="input">Input string</param>
    /// <returns>Enum type</returns>
    /// <exception cref="InvalidCastException">Exception when string to enum cast fails</exception>
    public static E CastToEnum<E>(this string input)
        where E : struct, Enum
    {
        if (Enum.TryParse<E>(input, true, out E resultInputType))
        {
            return resultInputType;
        }
        else
        {
            var enumName = typeof(E).Name;
            throw new InvalidCastException($"Cast {enumName} enum error, invalid value {input}");
        }
    }

    /// <summary>
    /// Cast string to integer
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Integer value if process is successful. Null otherwise</returns>
    public static int? CastToInt(this string input)
    {
        if (int.TryParse(input, out int result))
            return result;

        return null;
    }

    /// <summary>
    /// Cast string to boolean
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Boolean value if process is successful. Null otherwise</returns>
    public static bool? CastToBool(this string input)
    {
        if (bool.TryParse(input, out bool result))
            return result;

        return null;
    }

    /// <summary>
    /// Cast string to array
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>string array is successful. Null otherwise</returns>
    public static string[] CastToArray(this string input)
    {
        if (!string.IsNullOrEmpty(input))
            return input.TrimStart('[')
                .TrimEnd(']')
                .Split(',');

        return null;
    }

    #endregion

    #region Cryptography functions

    /// <summary>
    /// Encrypt string
    /// </summary>
    /// <param name="input">Plain text string</param>
    /// <returns>Base64 encrypted string</returns>
    public static string Encrypt(this string input)
    {
        return CryptoTools.EncryptString(input);
    }

    /// <summary>
    /// Decrypt string
    /// </summary>
    /// <param name="input">Base64 cipher text</param>
    /// <returns>Decrypted string</returns>
    public static string Decrypt(this string input)
    {
        return CryptoTools.DecryptString(input);
    }

    #endregion
}