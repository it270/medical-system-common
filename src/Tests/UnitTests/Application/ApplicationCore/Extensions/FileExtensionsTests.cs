using System.IO;
using System.Net.Mime;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;
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
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, 0, "testfile", "test.jpg");

        // Act
        var result = file.IsEmpty();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsEmpty_ShouldReturnFalse_WhenFileLengthIsGreaterThanZero()
    {
        // Arrange
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, 100, "testfile", "test.jpg");

        // Act
        var result = file.IsEmpty();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    [Ignore("Ignore file test")]
    public void IsImageJpeg_ShouldReturnTrue_WhenContentTypeIsImageJpeg()
    {
        // Arrange
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, 0, "testfile", "test.jpg");
        file.ContentType = MediaTypeNames.Image.Jpeg;

        // Act
        var result = file.IsImageJpeg();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    [Ignore("Ignore file test")]
    public void IsImageJpeg_ShouldReturnFalse_WhenContentTypeIsNotImageJpeg()
    {
        // Arrange
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, 0, "testfile", "test.jpg");
        file.ContentType = MediaTypeNames.Text.Plain;

        // Act
        var result = file.IsImageJpeg();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void HasValidSize_ShouldReturnTrue_WhenFileSizeIsLessThanOrEqualToLimit()
    {
        // Arrange
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, 100, "testfile", "test.jpg");

        // Act
        var result = file.HasValidSize();

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void HasValidSize_ShouldReturnFalse_WhenFileSizeIsGreaterThanLimit()
    {
        // Arrange
        using var stream = new MemoryStream();
        var file = new FormFile(stream, 0, GeneralConstants.FileSizeLimit + 1, "testfile", "test.jpg");

        // Act
        var result = file.HasValidSize();

        // Assert
        Assert.IsFalse(result);
    }
}