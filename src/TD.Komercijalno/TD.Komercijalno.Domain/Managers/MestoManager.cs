using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class MestoManager(ILogger<MestoManager> logger, KomercijalnoDbContext dbContext)
	: IMestoManager
{
	public List<MestoDto> GetMultiple()
	{
		return dbContext
			.Mesta.Select(
				(m) =>
					new MestoDto
					{
						MestoId = m.MestoId,
						Naziv = m.Naziv,
						OkrugId = m.OkrugId,
						UKorist = m.UKorist,
						NaTeretZR = m.NaTeretZR,
						Hitno = m.Hitno,
						SifraPlac = m.SifraPlac,
						UplRac = m.UplRac,
						UplModul = m.UplModul,
						UplPozNaBroj = m.UplPozNaBroj,
						EkspId = m.EkspId
					}
			)
			.ToList();
	}
}
