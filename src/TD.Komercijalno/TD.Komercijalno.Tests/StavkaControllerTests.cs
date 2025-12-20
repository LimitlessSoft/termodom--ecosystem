using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using Xunit;

namespace TD.Komercijalno.Tests;

public class StavkaControllerTests
{
    private readonly Mock<IStavkaManager> _managerMock;
    private readonly StavkaController _controller;

    public StavkaControllerTests()
    {
        _managerMock = new Mock<IStavkaManager>();
        _controller = new StavkaController(_managerMock.Object);
    }

    [Fact]
    public void Create_ShouldReturnStavka()
    {
        // Arrange
        var request = new StavkaCreateRequest();
        var expectedDto = new StavkaDto();
        _managerMock.Setup(m => m.Create(request)).Returns(expectedDto);

        // Act
        var result = _controller.Create(request);

        // Assert
        result.Should().Be(expectedDto);
    }

    [Fact]
    public void CreateOptimized_ShouldReturnStavke()
    {
        // Arrange
        var request = new StavkeCreateOptimizedRequest();
        var expectedDtos = new List<StavkaDto> { new() };
        _managerMock.Setup(m => m.CreateOptimized(request)).Returns(expectedDtos);

        // Act
        var result = _controller.CreateOptimized(request);

        // Assert
        result.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public void GetMultiple_ShouldReturnStavke()
    {
        // Arrange
        var request = new StavkaGetMultipleRequest();
        var expectedDtos = new List<StavkaDto> { new() };
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedDtos);

        // Act
        var result = _controller.GetMultiple(request);

        // Assert
        result.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public void DeleteStavke_ShouldReturnOk()
    {
        // Arrange
        var request = new StavkeDeleteRequest();

        // Act
        var result = _controller.DeleteStavke(request);

        // Assert
        result.Should().BeOfType<OkResult>();
        _managerMock.Verify(m => m.DeleteStavke(request), Times.Once);
    }

    [Fact]
    public void GetMultipleByRobaId_ShouldReturnOkWithStavke()
    {
        // Arrange
        var request = new StavkeGetMultipleByRobaId();
        var expectedDtos = new List<StavkaDto> { new() };
        _managerMock.Setup(m => m.GetMultipleByRobaId(request)).Returns(expectedDtos);

        // Act
        var result = _controller.GetMultipleByRobaId(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedDtos);
    }
}
