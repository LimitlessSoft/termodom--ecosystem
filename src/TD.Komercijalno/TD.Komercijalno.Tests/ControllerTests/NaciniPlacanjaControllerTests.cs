using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.IManagers;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class NaciniPlacanjaControllerTests
{
    private readonly Mock<INacinPlacanjaManager> _managerMock;
    private readonly NaciniPlacanjaController _controller;

    public NaciniPlacanjaControllerTests()
    {
        _managerMock = new Mock<INacinPlacanjaManager>();
        _controller = new NaciniPlacanjaController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var expected = new List<NacinPlacanjaDto>();
        _managerMock.Setup(m => m.GetMultiple()).Returns(expected);

        var result = _controller.GetMultiple();

        result.Should().BeEquivalentTo(expected);
    }
}
