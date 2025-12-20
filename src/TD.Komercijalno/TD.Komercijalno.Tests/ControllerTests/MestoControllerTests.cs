using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.IManagers;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class MestoControllerTests
{
    private readonly Mock<IMestoManager> _managerMock;
    private readonly MestoController _controller;

    public MestoControllerTests()
    {
        _managerMock = new Mock<IMestoManager>();
        _controller = new MestoController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsOk()
    {
        var expected = new List<MestoDto>();
        _managerMock.Setup(m => m.GetMultiple()).Returns(expected);

        var result = _controller.GetMultiple();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expected);
    }
}
