using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Namene;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class NamenaManager(ILogger<NamenaManager> logger, KomercijalnoDbContext dbContext)
	: INamenaManager
{
	public List<NamenaDto> GetMultiple() => dbContext.Namene.ToList().ToNamenaDtoList();
}
