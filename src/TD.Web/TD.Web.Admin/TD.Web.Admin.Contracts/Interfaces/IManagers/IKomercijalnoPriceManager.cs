using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoPriceManager
    {
        LSCoreListResponse<KomercijalnoPriceGetDto> GetMultiple();
    }
}
