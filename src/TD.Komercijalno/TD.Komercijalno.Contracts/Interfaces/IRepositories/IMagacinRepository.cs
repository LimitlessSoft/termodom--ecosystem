using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IMagacinRepository
{
	Magacin Get(short id);
	Magacin? GetOrDefault(short id);
}
