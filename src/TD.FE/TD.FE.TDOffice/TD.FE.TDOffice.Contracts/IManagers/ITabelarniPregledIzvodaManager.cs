using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface ITabelarniPregledIzvodaManager
    {
        ListResponse<TabelarniPregledIzvodaGetDto> Get();
    }
}
