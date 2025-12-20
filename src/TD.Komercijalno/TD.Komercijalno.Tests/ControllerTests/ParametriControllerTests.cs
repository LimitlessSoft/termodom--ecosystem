using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Parametri;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class ParametriControllerTests
{
    private readonly Mock<IParametarManager> _managerMock;
    private readonly ParametriController _controller;

    public ParametriControllerTests()
    {
        _managerMock = new Mock<IParametarManager>();
        _controller = new ParametriController(_managerMock.Object);
    }

    [Fact]
    public void Update_ReturnsOk()
    {
        var request = new UpdateParametarRequest();

        var result = _controller.Update(request);

        result.Should().BeOfType<OkResult>();
        _managerMock.Verify(m => m.Update(request), Times.Once);
    }
}
