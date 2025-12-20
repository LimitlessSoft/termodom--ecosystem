using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Promene;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Promene;
using Xunit;

namespace TD.Komercijalno.Tests;

public class PromenaControllerTests
{
    private readonly Mock<IPromenaManager> _managerMock;
    private readonly PromenaController _controller;

    public PromenaControllerTests()
    {
        _managerMock = new Mock<IPromenaManager>();
        _controller = new PromenaController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsOk()
    {
        var request = new PromenaGetMultipleRequest();
        var expected = new List<PromenaDto>();
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

        var result = _controller.GetMultiple(request);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expected);
    }
}
