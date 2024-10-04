using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Soundify.Controllers;
using Soundify.Managers.Interfaces;

using Moq;
using Soundify.DAL.PostgreSQL.Models.db;

namespace Soundify.Tests.Controllers;

[TestFixture]
public class ArtistControllerTest
{
    private Mock<IArtistManager> _artistManagerMock;
    private ArtistController _controller;

    [SetUp]
    public void SetUp()
    {
        _artistManagerMock = new Mock<IArtistManager>();
        _controller = new ArtistController(_artistManagerMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _controller?.Dispose();
    }

    [Test]
    public async Task GetArtist_ArtistNotFound_ReturnsNotFound()
    {
        // Arrange
        var artistId = Guid.NewGuid();

        // Настройка mock-объекта, чтобы метод GetArtistByIdAsync возвращал null (артист не найден)
        _artistManagerMock
            .Setup(m => m.GetArtistByIdAsync(artistId))
            .ReturnsAsync((Artist)null);

        // Act
        var result = await _controller.GetArtist(artistId) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null,
            $"Expected ObjectResult when artist is not found, but got null for artistId: {artistId}.");
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound), 
            $"Expected status code 404 Not Found for artistId: {artistId}, but got {result.StatusCode}.");
    }
}