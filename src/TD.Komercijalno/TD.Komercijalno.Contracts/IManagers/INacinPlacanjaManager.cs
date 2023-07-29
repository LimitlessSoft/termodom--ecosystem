
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INacinPlacanjaManager
    {
        public ListResponse<NacinPlacanjaDto> GetMultiple();
    }
}
