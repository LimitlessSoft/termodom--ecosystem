using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Dtos.Komentari;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IKomentarManager
    {
        KomentarDto Get(GetKomentarRequest request);
        KomentarDto Create(CreateKomentarRequest request);
        KomentarDto Update(UpdateKomentarRequest request);
        KomentarDto FlushComments(FlushCommentsRequest request);
    }
}
