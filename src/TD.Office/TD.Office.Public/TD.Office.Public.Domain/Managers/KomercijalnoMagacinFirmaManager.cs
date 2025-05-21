using LSCore.Mapper.Domain;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Domain.Managers;

public class KomercijalnoMagacinFirmaManager(IKomercijalnoMagacinFirmaRepository repository)
	: IKomercijalnoMagacinFirmaManager
{
	public GetKomercijalnoMagacinFirmaDto Get(int magacinId) =>
		repository
			.GetByMagacinId(magacinId)
			.ToMapped<KomercijalnoMagacinFirmaEntity, GetKomercijalnoMagacinFirmaDto>();
}
