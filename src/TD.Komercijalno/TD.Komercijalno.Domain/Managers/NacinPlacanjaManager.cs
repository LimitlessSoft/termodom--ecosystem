using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class NacinPlacanjaManager(
	ILogger<NacinPlacanjaManager> logger,
	KomercijalnoDbContext dbContext
) : INacinPlacanjaManager
{
	public List<NacinPlacanjaDto> GetMultiple() =>
		dbContext.NaciniPlacanja.ToList().ToNacinPlacanjaDtoList();
}
