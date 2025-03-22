using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Dtos;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoIFinansijskoPoGodinama;

public class KomercijalnoIFinansijskoPoGodinamaStatusDtoMappings
	: ILSCoreMapper<KomercijalnoIFinansijskoPoGodinamaStatusEntity, IdNamePairDto>
{
	public IdNamePairDto ToMapped(KomercijalnoIFinansijskoPoGodinamaStatusEntity sender) =>
		new IdNamePairDto { Id = sender.Id, Name = sender.Naziv };
}
