using LSCore.Mapper.Domain;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class RobaUMagacinuManager(
	ILogger<RobaUMagacinuManager> logger,
	KomercijalnoDbContext dbContext
) : IRobaUMagacinuManager
{
	public List<RobaUMagacinuGetDto> GetMultiple(RobaUMagacinuGetMultipleRequest request) =>
		dbContext
			.RobaUMagacinu.Where(x =>
				(request.MagacinId == null || x.MagacinId == request.MagacinId)
			)
			.ToMappedList<RobaUMagacinu, RobaUMagacinuGetDto>();
}
