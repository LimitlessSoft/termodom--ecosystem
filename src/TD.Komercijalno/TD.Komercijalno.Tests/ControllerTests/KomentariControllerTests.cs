using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Komentari;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class KomentariControllerTests
{
    private readonly Mock<IKomentarManager> _managerMock;
    private readonly KomentariController _controller;

    public KomentariControllerTests()
    {
        _managerMock = new Mock<IKomentarManager>();
        _controller = new KomentariController(_managerMock.Object);
    }

    [Fact]
    public void Create_ReturnsKomentarDto()
    {
        var request = new CreateKomentarRequest();
        var expected = new KomentarDto();
        _managerMock.Setup(m => m.Create(request)).Returns(expected);

        var result = _controller.Create(request);

        result.Should().Be(expected);
    }

    [Fact]
    public void FlushComments_ReturnsOk()
    {
        var request = new FlushCommentsRequest();

        var result = _controller.FlushComments(request);

        result.Should().BeOfType<OkResult>();
        _managerMock.Verify(m => m.FlushComments(request), Times.Once);
    }

    [Fact]
    public void Update_ReturnsKomentarDto()
    {
        var request = new UpdateKomentarRequest();
        var expected = new KomentarDto();
        _managerMock.Setup(m => m.Update(request)).Returns(expected);

        var result = _controller.Update(request);

        result.Should().Be(expected);
    }
}
