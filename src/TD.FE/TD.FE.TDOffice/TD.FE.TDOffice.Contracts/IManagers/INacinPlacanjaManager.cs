using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface INacinPlacanjaManager
    {
        LSCoreListResponse<NacinPlacanjaDto> GetMultiple();
    }
}
