using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;

namespace TD.Komercijalno.Repository.Repositories;

public class StavkaRepository(KomercijalnoDbContext dbContext, ILogger<StavkaRepository> logger)
	: IStavkaRepository
{
	/// <summary>
	/// Deletes stavka from specific VrDok/BrDok by RobaId.
	/// If stavka not found, it just skips it.
	/// </summary>
	/// <param name="vrDok"></param>
	/// <param name="brDok"></param>
	/// <param name="robaId"></param>
	public void DeleteByRobaId(int vrDok, int brDok, int robaId)
	{
		var stavka = dbContext.Stavke.FirstOrDefault(x =>
			x.VrDok == vrDok && x.BrDok == brDok && x.RobaId == robaId
		);
		if (stavka is null)
			return;
		dbContext.Stavke.Remove(stavka);
		dbContext.SaveChanges();
	}

	/// <summary>
	/// Deletes all Stavke with the given VrDok and BrDok.
	/// </summary>
	/// <param name="vrDok"></param>
	/// <param name="brDok"></param>
	public void Delete(int vrDok, int brDok)
	{
		EntityFrameworkQueryableExtensions.ExecuteDelete(
			dbContext.Stavke.Where(x => x.VrDok == vrDok && x.BrDok == brDok)
		);
		dbContext.SaveChanges();
	}

	public void Insert(Stavka stavka)
	{
		try
		{
			dbContext.Stavke.Add(stavka);
			dbContext.SaveChanges();
		}
		catch (Exception ex)
		{
			logger.LogError("Failed adding stavka");
			logger.LogError($"Could not insert {ex}");
			logger.LogError(
				"Stavka: {0}",
				JsonConvert.SerializeObject(
					stavka,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					}
				)
			);
			throw;
		}
	}
}
