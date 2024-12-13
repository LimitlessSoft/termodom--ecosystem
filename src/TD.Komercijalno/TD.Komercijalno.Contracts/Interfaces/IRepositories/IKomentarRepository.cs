using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;
public interface IKomentarRepository
{
    Komentar Get(int vrDok, int brDok);
    void Flush(int vrDok, int brDok);
}
