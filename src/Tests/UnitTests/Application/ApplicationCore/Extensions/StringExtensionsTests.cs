using System;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.UnitTests.Data;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.UnitTests.Application.ApplicationCore.Extensions;

[TestFixture]
public class StringExtensionsTests
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

    private enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }

    [Test]
    public void CastToEnum_ValidInput_ReturnsEnum()
    {
        // Arrange
        string input = "Value1";

        // Act
        TestEnum result = input.CastToEnum<TestEnum>();

        // Assert
        Assert.AreEqual(TestEnum.Value1, result);
    }

    [Test]
    public void CastToEnum_InvalidInput_ThrowsException()
    {
        // Arrange
        string input = "InvalidValue";

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => input.CastToEnum<TestEnum>());
    }

    [Test]
    public void CastToInt_ValidInput_ReturnsInt()
    {
        // Arrange
        string input = "123";

        // Act
        int? result = input.CastToInt();

        // Assert
        Assert.AreEqual(123, result);
    }

    [Test]
    public void CastToInt_InvalidInput_ReturnsNull()
    {
        // Arrange
        string input = "InvalidValue";

        // Act
        int? result = input.CastToInt();

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void CastToBool_ValidInput_ReturnsBool()
    {
        // Arrange
        string input = "true";

        // Act
        bool? result = input.CastToBool();

        // Assert
        Assert.AreEqual(true, result);
    }

    [Test]
    public void CastToBool_InvalidInput_ReturnsNull()
    {
        // Arrange
        string input = "InvalidValue";

        // Act
        bool? result = input.CastToBool();

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void CastToArray_ValidInput_ReturnsArray()
    {
        // Arrange
        string input = "[value1,value2,value3]";

        // Act
        string[] result = input.CastToArray();

        // Assert
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual("value1", result[0]);
        Assert.AreEqual("value2", result[1]);
        Assert.AreEqual("value3", result[2]);
    }

    [Test]
    public void CastToArray_NullInput_ReturnsNull()
    {
        // Arrange
        string input = null;

        // Act
        string[] result = input.CastToArray();

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void Encrypt_ValidInput_ReturnsEncryptedString()
    {
        // Arrange
        string input = "test";

        // Act
        string result = input.Encrypt();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreNotEqual(input, result);
    }

    [Test]
    public void Decrypt_ValidInput_ReturnsDecryptedString()
    {
        // Arrange
        string input = "test";
        string encryptedInput = input.Encrypt();

        // Act
        string result = encryptedInput.Decrypt();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(input, result);
    }
}