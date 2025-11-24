using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class DokumentRepository(KomercijalnoDbContext dbContext) : IDokumentRepository
{
	public void Create(Dokument dokument)
	{
		dbContext.Dokumenti.Add(dokument);
		dbContext.SaveChanges();
	}

	public Dokument Get(int vrDok, int brDok)
	{
		var entity = GetOrDefault(vrDok, brDok);
		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity;
	}

	public Dokument? GetOrDefault(int vrDok, int brDok) =>
		dbContext.Dokumenti.Include(x => x.VrstaDok).FirstOrDefault(x => x.VrDok == vrDok && x.BrDok == brDok);

	public void Update(Dokument dokument)
	{
		dbContext.Dokumenti.Update(dokument);
		dbContext.SaveChanges();
	}
}
