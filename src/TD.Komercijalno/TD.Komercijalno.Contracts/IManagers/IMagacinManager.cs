using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Requests.Magacini;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IMagacinManager
    {
        List<MagacinDto> GetMultiple(MagaciniGetMultipleRequest request);
    }
}
