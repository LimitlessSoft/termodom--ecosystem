using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IRobaUMagacinuManager
    {
        List<RobaUMagacinuGetDto> GetMultiple(RobaUMagacinuGetMultipleRequest request);
    }
}
