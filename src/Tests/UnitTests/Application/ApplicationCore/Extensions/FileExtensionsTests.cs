using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.Tests.UnitTests.Application.ApplicationCore.Extensions;

[TestFixture]
public class FileExtensionsTests
{
    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = file.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenFileLengthIsZero()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(0);
        var file = fileMock.Object;

        // Act
        var result = file.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsEmpty_ShouldReturnFalse_WhenFileLengthIsGreaterThanZero()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(1);
        var file = fileMock.Object;

        // Act
        var result = file.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }
}