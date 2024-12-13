using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Contracts.Dtos.Stavke;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IStavkaManager
    {
        List<StavkaDto> GetMultiple(StavkaGetMultipleRequest request);
        StavkaDto Create(StavkaCreateRequest request);
        bool DeleteStavke(StavkeDeleteRequest request);
    }
}
