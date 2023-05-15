using System;
using It270.MedicalSystem.Common.Application.ApplicationCore.Services;
using It270.MedicalSystem.Common.Tests.UnitTests.Data;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.Tests.UnitTests.Application.ApplicationCore.Services;

[TestFixture]
public class CryptoToolsTests
{
    [OneTimeSetUp]
    public void Init()
    {
        Environment.SetEnvironmentVariable(EnvVars.AesKey.Key, EnvVars.AesKey.Value);
        Environment.SetEnvironmentVariable(EnvVars.AesIv.Key, EnvVars.AesIv.Value);
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        Environment.SetEnvironmentVariable(EnvVars.AesKey.Key, null);
        Environment.SetEnvironmentVariable(EnvVars.AesIv.Key, null);
    }

    [Test]
    public void PasswordGenerator_ReturnsPasswordOfGivenLength()
    {
        // Arrange
        const int length = 20;

        // Act
        var password = CryptoTools.PasswordGenerator(length);

        // Assert
        Assert.AreEqual(length, password.Length);
    }

    [Test]
    public void EncryptString_ReturnsNonEmptyString()
    {
        // Arrange
        const string plainText = "hello world";

        // Act
        var encryptedText = CryptoTools.EncryptString(plainText);

        // Assert
        Assert.IsNotNull(encryptedText);
        Assert.IsNotEmpty(encryptedText);
    }

    [Test]
    public void DecryptString_ReturnsOriginalString()
    {
        // Arrange
        const string plainText = "hello world";

        // Act
        var encryptedText = CryptoTools.EncryptString(plainText);
        var decryptedText = CryptoTools.DecryptString(encryptedText);

        // Assert
        Assert.AreEqual(plainText, decryptedText);
    }

    [Test]
    public void DecryptString_ThrowsExceptionIfCipherTextIsInvalid()
    {
        // Arrange
        const string invalidCipherText = "invalid_cipher_text";

        // Act & Assert
        Assert.Throws<FormatException>(() => CryptoTools.DecryptString(invalidCipherText));
    }
}