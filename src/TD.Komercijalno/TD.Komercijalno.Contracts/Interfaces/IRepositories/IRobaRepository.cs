using System.Linq.Expressions;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Interfaces.IRepositories;

public interface IRobaRepository
{
	Roba Get(int id, params Expression<Func<Roba, object>>[] includes);
	Roba? GetOrDefault(int id, params Expression<Func<Roba, object>>[] includes);
	void Insert(Roba roba);
}
