using It270.MedicalSystem.Common.Application.ApplicationCore.Services;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.Tests.UnitTests.Application.ApplicationCore.Services;

[TestFixture]
public class IamToolsTests
{
    [Test]
    public void IsAValidUserName_ValidUserName_ReturnsTrue()
    {
        // Arrange
        string userName = "JohnDoe";

        // Act
        bool isUserNameValid = IamTools.IsAValidUserName(userName);

        // Assert
        Assert.IsTrue(isUserNameValid);
    }

    [Test]
    public void IsAValidUserName_InvalidUserName_ReturnsFalse()
    {
        // Arrange
        string userName = "johndoe";

        // Act
        bool isUserNameValid = IamTools.IsAValidUserName(userName);

        // Assert
        Assert.IsFalse(isUserNameValid);
    }

    [Test]
    public void IsAValidRoleName_ValidRoleName_ReturnsTrue()
    {
        // Arrange
        string roleName = "Admin";

        // Act
        bool isRoleNameValid = IamTools.IsAValidRoleName(roleName);

        // Assert
        Assert.IsTrue(isRoleNameValid);
    }

    [Test]
    public void IsAValidRoleName_InvalidRoleName_ReturnsFalse()
    {
        // Arrange
        string roleName = "admin";

        // Act
        bool isRoleNameValid = IamTools.IsAValidRoleName(roleName);

        // Assert
        Assert.IsFalse(isRoleNameValid);
    }
}