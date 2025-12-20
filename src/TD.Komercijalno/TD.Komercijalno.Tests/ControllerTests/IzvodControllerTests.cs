using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class IzvodControllerTests
{
    private readonly Mock<IIzvodManager> _managerMock;
    private readonly IzvodController _controller;

    public IzvodControllerTests()
    {
        _managerMock = new Mock<IIzvodManager>();
        _controller = new IzvodController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsOk()
    {
        var request = new IzvodGetMultipleRequest();
        var expected = new List<IzvodDto>();
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

        var result = _controller.GetMultiple(request);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expected);
    }
}
