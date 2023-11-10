using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IVrstaDokManager
    {
        LSCoreListResponse<VrstaDokDto> GetMultiple();
    }
}
