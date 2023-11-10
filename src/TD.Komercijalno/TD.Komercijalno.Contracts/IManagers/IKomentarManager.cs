using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IKomentarManager
    {
        LSCoreResponse<KomentarDto> Get(GetKomentarRequest request);
        LSCoreResponse<KomentarDto> Create(CreateKomentarRequest request);
    }
}
