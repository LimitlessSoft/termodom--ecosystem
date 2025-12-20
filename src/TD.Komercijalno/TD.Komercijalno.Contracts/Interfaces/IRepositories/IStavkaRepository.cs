using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IStavkaRepository
{
	void DeleteByRobaId(int vrDok, int brDok, int robaId);
	void Delete(int vrDok, int brDok);
	void Insert(Stavka stavka);
	void InsertRange(List<Stavka> stavke);
}
