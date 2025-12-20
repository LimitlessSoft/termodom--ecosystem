using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Magacini;
using Xunit;

namespace TD.Komercijalno.Tests;

public class MagaciniControllerTests
{
    private readonly Mock<IMagacinManager> _managerMock;
    private readonly MagaciniController _controller;

    public MagaciniControllerTests()
    {
        _managerMock = new Mock<IMagacinManager>();
        _controller = new MagaciniController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var request = new MagaciniGetMultipleRequest();
        var expected = new List<MagacinDto> { new() };
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

        var result = _controller.GetMultiple(request);

        result.Should().BeEquivalentTo(expected);
    }
}
