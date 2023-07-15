using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IKomentarManager
    {
        Response<KomentarDto> Get(GetKomentarRequest request);
        Response<KomentarDto> Create(CreateKomentarRequest request);
    }
}
