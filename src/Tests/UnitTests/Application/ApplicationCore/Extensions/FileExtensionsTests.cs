using System.Net.Mime;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace It270.MedicalSystem.Common.UnitTests.Application.ApplicationCore.Extensions;

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

    [Test]
    public void IsImageJpeg_ShouldReturnTrue_WhenContentTypeIsImageJpeg()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns(MediaTypeNames.Image.Jpeg);
        var file = fileMock.Object;

        // Act
        var result = file.IsImageJpeg();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsImageJpeg_ShouldReturnFalse_WhenContentTypeIsNotImageJpeg()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.ContentType).Returns(MediaTypeNames.Text.Plain);
        var file = fileMock.Object;

        // Act
        var result = file.IsImageJpeg();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void HasValidSize_ShouldReturnTrue_WhenFileSizeIsLessThanOrEqualToLimit()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(1);
        var file = fileMock.Object;

        // Act
        var result = file.HasValidSize();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void HasValidSize_ShouldReturnFalse_WhenFileSizeIsGreaterThanLimit()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(GeneralConstants.FileSizeLimit + 1);
        var file = fileMock.Object;

        // Act
        var result = file.HasValidSize();

        // Assert
        Assert.IsFalse(result);
    }
}