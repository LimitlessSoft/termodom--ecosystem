
using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INacinPlacanjaManager
    {
        LSCoreListResponse<NacinPlacanjaDto> GetMultiple();
    }
}
