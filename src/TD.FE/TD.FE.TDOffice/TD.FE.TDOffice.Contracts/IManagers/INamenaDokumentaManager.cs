using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface INamenaDokumentaManager
    {
        LSCoreListResponse<NamenaDto> GetMultiple();
    }
}
