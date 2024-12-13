using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;
public interface IKomentarRepository
{
    Komentar Get(GetKomentarRequest request);
}
