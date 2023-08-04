using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface INamenaDokumentaManager
    {
        public ListResponse<NamenaDto> GetMultiple();
    }
}
