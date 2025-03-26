using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IKomentarRepository
{
	Komentar Get(int vrDok, int brDok);
	void Flush(int vrDok, int brDok);
	void Insert(Komentar komentar);
}
