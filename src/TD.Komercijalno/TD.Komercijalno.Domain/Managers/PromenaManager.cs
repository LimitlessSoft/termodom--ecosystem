using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Promene;
using TD.Komercijalno.Contracts.Dtos.Promene;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Promene;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class PromenaManager(ILogger<PromenaManager> logger, KomercijalnoDbContext dbContext)
	: IPromenaManager
{
	public List<PromenaDto> GetMultiple(PromenaGetMultipleRequest request) =>
		dbContext
			.Promene.Where(x =>
				(
					string.IsNullOrWhiteSpace(request.KontoStartsWith)
					|| x.Konto.StartsWith(request.KontoStartsWith)
				)
				&& (
					request.PPID == null
					|| request.PPID.Length == 0
					|| (x.PPID != null && request.PPID.Any(z => z == x.PPID.Value))
				)
			)
			.ToList()
			.ToPromenaDtoList();
}
