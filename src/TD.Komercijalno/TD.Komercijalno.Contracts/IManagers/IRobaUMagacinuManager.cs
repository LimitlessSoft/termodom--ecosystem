using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IRobaUMagacinuManager
    {
        LSCoreListResponse<RobaUMagacinuGetDto> GetMultiple();
    }
}
