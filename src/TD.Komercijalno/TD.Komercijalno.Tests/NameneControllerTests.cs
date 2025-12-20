using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.IManagers;
using Xunit;

namespace TD.Komercijalno.Tests;

public class NameneControllerTests
{
    private readonly Mock<INamenaManager> _managerMock;
    private readonly NameneController _controller;

    public NameneControllerTests()
    {
        _managerMock = new Mock<INamenaManager>();
        _controller = new NameneController(_managerMock.Object);
    }

    [Fact]
    public void GetMultiple_ReturnsList()
    {
        var expected = new List<NamenaDto>();
        _managerMock.Setup(m => m.GetMultiple()).Returns(expected);

        var result = _controller.GetMultiple();

        result.Should().BeEquivalentTo(expected);
    }
}
