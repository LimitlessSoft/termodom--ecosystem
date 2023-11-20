using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IStavkaManager
    {
        LSCoreListResponse<StavkaDto> GetMultiple(StavkaGetMultipleRequest request);
        LSCoreResponse<StavkaDto> Create(StavkaCreateRequest request);
    }
}
