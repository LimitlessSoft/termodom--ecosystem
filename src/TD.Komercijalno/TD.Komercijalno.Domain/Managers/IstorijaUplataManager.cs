using TD.Komercijalno.Contracts.DtoMappings.IstorijaUplata;
using TD.Komercijalno.Contracts.Dtos.IstorijaUplata;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class IstorijaUplataManager(KomercijalnoDbContext dbContext) : IIstorijaUplataManager
{
	public List<IstorijaUplataDto> GetMultiple(IstorijaUplataGetMultipleRequest request) =>
		dbContext
			.IstorijaUplata.Where(x =>
				(
					request.PPID == null
					|| request.PPID.Length == 0
					|| request.PPID.Any(z => z == x.PPID)
				)
			)
			.ToList()
			.ToIstorijaUpaltaDtoList();
}
