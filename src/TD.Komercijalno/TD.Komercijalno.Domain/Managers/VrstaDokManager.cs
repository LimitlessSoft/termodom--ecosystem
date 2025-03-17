using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.VrstaDoks;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class VrstaDokManager(ILogger<VrstaDokManager> logger, KomercijalnoDbContext dbContext)
	: IVrstaDokManager
{
	public List<VrstaDokDto> GetMultiple() => dbContext.VrstaDok.ToList().ToVrstaDokDtoList();
}
