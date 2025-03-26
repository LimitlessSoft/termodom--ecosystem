using LSCore.Exceptions;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class ParametarRepository(KomercijalnoDbContext dbContext) : IParametarRepository
{
	public Parametar Get(string naziv)
	{
		var entity = GetOrDefault(naziv);
		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity;
	}

	public Parametar? GetOrDefault(string naziv) =>
		dbContext.Parametri.FirstOrDefault(x => x.Naziv == naziv);

	public void SetVrednost(string naziv, string vrednost)
	{
		var parametar = Get(naziv);
		parametar.Vrednost = vrednost;
		dbContext.Update(parametar);
		dbContext.SaveChanges();
	}
}
