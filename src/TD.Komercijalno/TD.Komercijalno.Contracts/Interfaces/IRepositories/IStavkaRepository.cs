using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IStavkaRepository
{
	void Delete(int vrDok, int brDok);
	void Insert(Stavka stavka);
}
