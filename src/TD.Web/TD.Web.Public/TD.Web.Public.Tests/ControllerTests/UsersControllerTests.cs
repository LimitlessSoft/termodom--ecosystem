using FluentAssertions;
using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using Moq;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Public.Api.Controllers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class UsersControllerTests
{
	private readonly Mock<IUserManager> _managerMock;
	private readonly Mock<ILSCoreAuthUserPassManager<string>> _authManagerMock;
	private readonly UsersController _controller;

	public UsersControllerTests()
	{
		_managerMock = new Mock<IUserManager>();
		_authManagerMock = new Mock<ILSCoreAuthUserPassManager<string>>();
		_controller = new UsersController(_managerMock.Object, _authManagerMock.Object);
	}

	[Fact]
	public void Login_ShouldReturnToken()
	{
		// Arrange
		var request = new UserLoginRequest { Username = "user", Password = "pass" };
		var expectedToken = "token";
		_authManagerMock
			.Setup(m => m.Authenticate(request.Username, request.Password))
			.Returns(new LSCoreJwt { AccessToken = expectedToken, RefreshToken = "refresh" });

		// Act
		var result = _controller.Login(request);

		// Assert
		result.Should().Be(expectedToken);
	}

	[Fact]
	public void Register_ShouldCallManager()
	{
		// Arrange
		var request = new UserRegisterRequest();

		// Act
		_controller.Register(request);

		// Assert
		_managerMock.Verify(m => m.Register(request), Times.Once);
	}

	[Fact]
	public void Me_ShouldReturnUserInformation()
	{
		// Arrange
		var expectedDto = new UserInformationDto();
		_managerMock.Setup(m => m.Me()).Returns(expectedDto);

		// Act
		var result = _controller.Me();

		// Assert
		result.Should().Be(expectedDto);
	}

	[Fact]
	public void ResetPassword_ShouldCallManager()
	{
		// Arrange
		var request = new UserResetPasswordRequest();

		// Act
		_controller.ResetPassword(request);

		// Assert
		_managerMock.Verify(m => m.ResetPassword(request), Times.Once);
	}

	[Fact]
	public void SetPassword_ShouldCallManager()
	{
		// Arrange
		var request = new UserSetPasswordRequest();

		// Act
		_controller.SetPassword(request);

		// Assert
		_managerMock.Verify(m => m.SetPassword(request), Times.Once);
	}
}
