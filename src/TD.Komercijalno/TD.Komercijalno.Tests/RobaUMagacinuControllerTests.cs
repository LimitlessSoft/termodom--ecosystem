using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using Xunit;

namespace TD.Komercijalno.Tests;

public class RobaUMagacinuControllerTests
{
    private readonly Mock<IRobaUMagacinuManager> _managerMock;
    private readonly RobaUMagacinuController _controller;

    public RobaUMagacinuControllerTests()
    {
        _managerMock = new Mock<IRobaUMagacinuManager>();
        _controller = new RobaUMagacinuController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var request = new RobaUMagacinuGetMultipleRequest();
        var expected = new List<RobaUMagacinuGetDto>();
        _managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

        var result = _controller.GetMultiple(request);

        result.Should().BeEquivalentTo(expected);
    }
}
