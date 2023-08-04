using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface INacinPlacanjaManager
    {
        ListResponse<NacinPlacanjaDto> GetMultiple();
    }
}
