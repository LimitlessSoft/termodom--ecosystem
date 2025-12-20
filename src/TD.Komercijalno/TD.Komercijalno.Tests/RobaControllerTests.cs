using FluentAssertions;
using LSCore.Common.Contracts;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Roba;
using Xunit;

namespace TD.Komercijalno.Tests;

public class RobaControllerTests
{
    private readonly Mock<IRobaManager> _managerMock;
    private readonly RobaController _controller;

    public RobaControllerTests()
    {
        _managerMock = new Mock<IRobaManager>();
        _controller = new RobaController(_managerMock.Object);
    }

    [Fact]
    public void Create_ReturnsRoba()
    {
        var request = new RobaCreateRequest();
        var expected = new Roba();
        _managerMock.Setup(m => m.Create(request)).Returns(expected);

        var result = _controller.Create(request);

        result.Should().Be(expected);
    }

    [Fact]
    public void GetSingle_ReturnsRobaDto()
    {
        var request = new LSCoreIdRequest { Id = 1 };
        var expected = new RobaDto();
        _managerMock.Setup(m => m.Get(request)).Returns(expected);

        var result = _controller.GetSingle(request);

        result.Should().Be(expected);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var request = new RobaGetMultipleRequest();
        var expected = new List<RobaDto> { new() };
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

        var result = _controller.GetMultiple(request);

        result.Should().BeEquivalentTo(expected);
    }
}
