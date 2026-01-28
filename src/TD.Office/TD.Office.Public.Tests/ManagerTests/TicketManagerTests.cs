using FluentAssertions;
using Moq;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Domain.Managers;
using Xunit;

namespace TD.Office.Public.Tests.ManagerTests;

public class TicketManagerTests : TestBase
{
	private readonly TicketManager _manager;
	private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();
	private readonly Mock<IUserRepository> _userRepositoryMock = new();

	public TicketManagerTests()
	{
		_manager = new TicketManager(
			_ticketRepositoryMock.Object,
			_userRepositoryMock.Object,
			_dbContext
		);
	}

	private static UserEntity CreateTestUser(long id = 1, string nickname = "TestUser")
	{
		return new UserEntity
		{
			Id = id,
			Username = $"user{id}",
			Password = "test",
			Nickname = nickname,
			Type = UserType.User,
			Permissions = new List<UserPermissionEntity>(),
		};
	}

	private static TicketEntity CreateTicketEntity(
		long id,
		TicketStatus status,
		UserEntity submittedBy,
		DateTime? updatedAt = null,
		DateTime? resolvedAt = null,
		UserEntity? resolvedBy = null
	)
	{
		return new TicketEntity
		{
			Id = id,
			Title = $"Ticket {id}",
			Description = $"Description for ticket {id}",
			Type = TicketType.Bug,
			Status = status,
			Priority = TicketPriority.Medium,
			SubmittedByUserId = submittedBy.Id,
			SubmittedByUser = submittedBy,
			IsActive = true,
			CreatedAt = DateTime.UtcNow.AddDays(-10),
			UpdatedAt = updatedAt ?? DateTime.UtcNow,
			ResolvedAt = resolvedAt,
			ResolvedByUserId = resolvedBy?.Id,
			ResolvedByUser = resolvedBy,
		};
	}

	#region GetInProgress Tests

	[Fact]
	public void GetInProgress_ReturnsInProgressTickets()
	{
		// Arrange
		var user = CreateTestUser();
		var inProgressTickets = new List<TicketEntity>
		{
			CreateTicketEntity(1, TicketStatus.InProgress, user),
			CreateTicketEntity(2, TicketStatus.InProgress, user),
		};

		_ticketRepositoryMock
			.Setup(r => r.GetInProgress(5))
			.Returns(inProgressTickets);

		// Act
		var result = _manager.GetInProgress();

		// Assert
		result.Should().HaveCount(2);
		result.Should().AllSatisfy(t => t.Status.Should().Be(TicketStatus.InProgress));
	}

	[Fact]
	public void GetInProgress_WhenNoInProgressTickets_ReturnsEmptyList()
	{
		// Arrange
		_ticketRepositoryMock
			.Setup(r => r.GetInProgress(5))
			.Returns(new List<TicketEntity>());

		// Act
		var result = _manager.GetInProgress();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public void GetInProgress_MapsTicketPropertiesCorrectly()
	{
		// Arrange
		var user = CreateTestUser(1, "JohnDoe");
		var ticket = CreateTicketEntity(42, TicketStatus.InProgress, user);
		ticket.Title = "Fix login bug";
		ticket.Description = "Users cannot login";
		ticket.Type = TicketType.Bug;
		ticket.Priority = TicketPriority.High;

		_ticketRepositoryMock
			.Setup(r => r.GetInProgress(5))
			.Returns(new List<TicketEntity> { ticket });

		// Act
		var result = _manager.GetInProgress();

		// Assert
		result.Should().HaveCount(1);
		var dto = result.First();
		dto.Id.Should().Be(42);
		dto.Title.Should().Be("Fix login bug");
		dto.Description.Should().Be("Users cannot login");
		dto.Type.Should().Be(TicketType.Bug);
		dto.Status.Should().Be(TicketStatus.InProgress);
		dto.Priority.Should().Be(TicketPriority.High);
		dto.SubmittedByUserNickname.Should().Be("JohnDoe");
	}

	[Fact]
	public void GetInProgress_CallsRepositoryWithLimitOf5()
	{
		// Arrange
		_ticketRepositoryMock
			.Setup(r => r.GetInProgress(5))
			.Returns(new List<TicketEntity>());

		// Act
		_manager.GetInProgress();

		// Assert
		_ticketRepositoryMock.Verify(r => r.GetInProgress(5), Times.Once);
	}

	#endregion

	#region GetRecentlySolved Tests

	[Fact]
	public void GetRecentlySolved_ReturnsResolvedTickets()
	{
		// Arrange
		var submitter = CreateTestUser(1, "Submitter");
		var resolver = CreateTestUser(2, "Resolver");
		var resolvedTickets = new List<TicketEntity>
		{
			CreateTicketEntity(
				1,
				TicketStatus.Resolved,
				submitter,
				resolvedAt: DateTime.UtcNow.AddHours(-1),
				resolvedBy: resolver
			),
			CreateTicketEntity(
				2,
				TicketStatus.Resolved,
				submitter,
				resolvedAt: DateTime.UtcNow.AddHours(-2),
				resolvedBy: resolver
			),
		};

		_ticketRepositoryMock
			.Setup(r => r.GetRecentlySolved(5))
			.Returns(resolvedTickets);

		// Act
		var result = _manager.GetRecentlySolved();

		// Assert
		result.Should().HaveCount(2);
		result.Should().AllSatisfy(t => t.Status.Should().Be(TicketStatus.Resolved));
	}

	[Fact]
	public void GetRecentlySolved_WhenNoResolvedTickets_ReturnsEmptyList()
	{
		// Arrange
		_ticketRepositoryMock
			.Setup(r => r.GetRecentlySolved(5))
			.Returns(new List<TicketEntity>());

		// Act
		var result = _manager.GetRecentlySolved();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public void GetRecentlySolved_MapsResolverInformationCorrectly()
	{
		// Arrange
		var submitter = CreateTestUser(1, "BugReporter");
		var resolver = CreateTestUser(2, "Developer");
		var resolvedAt = DateTime.UtcNow.AddMinutes(-30);

		var ticket = CreateTicketEntity(
			1,
			TicketStatus.Resolved,
			submitter,
			resolvedAt: resolvedAt,
			resolvedBy: resolver
		);
		ticket.ResolutionNotes = "Fixed in commit abc123";

		_ticketRepositoryMock
			.Setup(r => r.GetRecentlySolved(5))
			.Returns(new List<TicketEntity> { ticket });

		// Act
		var result = _manager.GetRecentlySolved();

		// Assert
		result.Should().HaveCount(1);
		var dto = result.First();
		dto.ResolvedByUserNickname.Should().Be("Developer");
		dto.ResolvedAt.Should().Be(resolvedAt);
		dto.ResolutionNotes.Should().Be("Fixed in commit abc123");
	}

	[Fact]
	public void GetRecentlySolved_CallsRepositoryWithLimitOf5()
	{
		// Arrange
		_ticketRepositoryMock
			.Setup(r => r.GetRecentlySolved(5))
			.Returns(new List<TicketEntity>());

		// Act
		_manager.GetRecentlySolved();

		// Assert
		_ticketRepositoryMock.Verify(r => r.GetRecentlySolved(5), Times.Once);
	}

	#endregion
}
