using System.Collections.Generic;
using System.Security.Claims;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Presentation.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.UnitTests.Presentation.WebApi.Extension;

[TestFixture]
public class HttpContextExtensionsTests
{
    [Test]
    public void IsAValidSession_ReturnsTrue_WhenAllConditionsAreMet()
    {
        // Arrange
        var usernameClaim = new Claim(IamConstants.UserName, "FakeUser");
        var roleClaim = new Claim(IamConstants.Role, "FakeRole");
        var claims = new List<Claim> { usernameClaim, roleClaim };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        // Act
        var result = httpContextMock.Object.IsAValidSession();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsAValidSession_ReturnsFalse_WhenUserNameIsMissing()
    {
        // Arrange
        var fakeRoleList = new List<Claim> { new(IamConstants.Role, "FakeRole") };

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User.FindAll(IamConstants.Role)).Returns(fakeRoleList);
        httpContextMock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
        // Act
        var result = httpContextMock.Object.IsAValidSession();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsAValidSession_ReturnsFalse_WhenRolesAreMissing()
    {
        // Arrange
        var usernameClaim = new Claim(IamConstants.UserName, "FakeUser");
        var claims = new List<Claim> { usernameClaim };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);
        httpContextMock.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);

        // Act
        var result = httpContextMock.Object.IsAValidSession();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsAValidSession_ReturnsFalse_WhenNotAuthenticated()
    {
        // Arrange
        var usernameClaim = new Claim(IamConstants.UserName, "FakeUser");
        var claims = new List<Claim> { usernameClaim };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var principal = new ClaimsPrincipal(identity);
        var fakeRoleList = new List<Claim> { new(IamConstants.Role, "FakeRole") };

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);
        httpContextMock.Setup(c => c.User.FindAll(IamConstants.Role)).Returns(fakeRoleList);
        httpContextMock.Setup(c => c.User.Identity.IsAuthenticated).Returns(false);

        // Act
        var result = httpContextMock.Object.IsAValidSession();

        // Assert
        Assert.IsFalse(result);
    }
}
