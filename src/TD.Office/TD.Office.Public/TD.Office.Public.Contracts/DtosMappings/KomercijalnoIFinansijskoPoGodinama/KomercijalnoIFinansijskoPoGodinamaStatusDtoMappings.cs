using LSCore.Contracts.Dtos;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoIFinansijskoPoGodinama;

public class KomercijalnoIFinansijskoPoGodinamaStatusDtoMappings
    : ILSCoreDtoMapper<KomercijalnoIFinansijskoPoGodinamaStatusEntity, LSCoreIdNamePairDto>
{
    public LSCoreIdNamePairDto ToDto(KomercijalnoIFinansijskoPoGodinamaStatusEntity sender) =>
        new LSCoreIdNamePairDto { Id = sender.Id, Name = sender.Naziv };
}
