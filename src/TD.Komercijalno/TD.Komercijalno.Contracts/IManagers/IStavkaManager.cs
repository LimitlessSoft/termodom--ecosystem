using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IStavkaManager
    {
        ListResponse<StavkaDto> GetMultiple(StavkaGetMultipleRequest request);
        Response<StavkaDto> Create(StavkaCreateRequest request);
    }
}
