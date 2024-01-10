using LSCore.Contracts.Http;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoPriceManager
    {
        LSCoreListResponse<KomercijalnoPriceGetDto> GetMultiple();
    }
}
