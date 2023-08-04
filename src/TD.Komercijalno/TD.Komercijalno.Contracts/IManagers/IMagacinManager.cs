using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IMagacinManager
    {
        ListResponse<MagacinDto> GetMultiple();
    }
}
