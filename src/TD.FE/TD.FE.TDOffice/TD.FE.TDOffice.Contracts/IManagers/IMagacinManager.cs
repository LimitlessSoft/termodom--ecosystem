using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMagacinManager
    {
        ListResponse<MagacinDto> GetMultiple();
    }
}
