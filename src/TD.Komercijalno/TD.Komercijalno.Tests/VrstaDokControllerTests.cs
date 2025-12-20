using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.IManagers;
using Xunit;

namespace TD.Komercijalno.Tests;

public class VrstaDokControllerTests
{
    private readonly Mock<IVrstaDokManager> _managerMock;
    private readonly VrstaDokController _controller;

    public VrstaDokControllerTests()
    {
        _managerMock = new Mock<IVrstaDokManager>();
        _controller = new VrstaDokController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var expected = new List<VrstaDokDto> { new() };
        _managerMock.Setup(m => m.GetMultiple()).Returns(expected);

        var result = _controller.GetMultiple();

        result.Should().BeEquivalentTo(expected);
    }
}
