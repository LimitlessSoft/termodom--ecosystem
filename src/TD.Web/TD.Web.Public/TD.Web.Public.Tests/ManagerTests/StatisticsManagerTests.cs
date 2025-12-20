using Microsoft.Extensions.Logging;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Requests.Statistics;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class StatisticsManagerTests
{
	private readonly Mock<ILogger<StatisticsManager>> _loggerMock;
	private readonly Mock<IStatisticsItemRepository> _repositoryMock;
	private readonly StatisticsManager _manager;

	public StatisticsManagerTests()
	{
		_loggerMock = new Mock<ILogger<StatisticsManager>>();
		_repositoryMock = new Mock<IStatisticsItemRepository>();
		_manager = new StatisticsManager(_loggerMock.Object, _repositoryMock.Object);
	}

	[Fact]
	public void Log_SearchKeyword_InsertsEntity()
	{
		// Arrange
		var request = new ProductSearchKeywordRequest { SearchPhrase = "test" };

		// Act
		_manager.Log(request);

		// Assert
		_repositoryMock.Verify(r => r.Insert(It.Is<StatisticsItemEntity>(e => e.Value == "test")), Times.Once);
	}
}
