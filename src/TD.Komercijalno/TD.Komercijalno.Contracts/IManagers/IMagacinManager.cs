using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IMagacinManager
    {
        LSCoreListResponse<MagacinDto> GetMultiple();
    }
}
