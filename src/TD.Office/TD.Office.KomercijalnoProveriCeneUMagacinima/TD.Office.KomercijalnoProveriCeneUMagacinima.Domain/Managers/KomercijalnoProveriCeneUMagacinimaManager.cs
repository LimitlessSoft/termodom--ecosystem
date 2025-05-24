using LSCore.Exceptions;
using Newtonsoft.Json;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Constants;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Dtos;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Enums;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Interfaces.IManagers;

namespace TD.Office.KomercijalnoProveriCeneUMagacinima.Domain.Managers;

public class KomercijalnoProveriCeneUMagacinimaManager(ISettingRepository settingRepository)
	: IKomercijalnoProveriCeneUMagacinimaManager
{
	public async Task GenerateReportAsync()
	{
		var currState = settingRepository.GetValueOrDefault<string>(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_STATUS
		);
		if (
			currState != null
			&& currState != KomercijalnoProveriCeneUmagacinimaStatus.Idle.ToString()
		)
			throw new LSCoreBadRequestException(
				"Proveri cene u magacinima je vec pokrenut. Ne moze se pokrenuti ponovo dok se ne zavrsi prethodni."
			);
		settingRepository.SetValue(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_STATUS,
			KomercijalnoProveriCeneUmagacinimaStatus.InProgress
		);
#if DEBUG
		var env = TDKomercijalnoEnvironment.Development
#else
		var env = TDKomercijalnoEnvironment.Production
#endif
		;
		var clients = new Dictionary<TDKomercijalnoFirma, TDKomercijalnoClient>
		{
			{
				TDKomercijalnoFirma.TCMDZ,
				new TDKomercijalnoClient(
					GeneralConstants.Danas.Year,
					env,
					TDKomercijalnoFirma.TCMDZ
				)
			}
		};
		var reperneCeneDanasTask = clients[TDKomercijalnoFirma.TCMDZ]
			.Procedure.GetProdajnaCenaNaDanOptimizedAsync(
				new ProceduraGetProdajnaCenaNaDanOptimizedRequest
				{
					Datum = GeneralConstants.Danas,
					MagacinId = GeneralConstants.ReperniMagacin,
				}
			);
		var report = new List<ReportItemDto>();
		foreach (var t in GeneralConstants.ProveriUOvimMagacinima)
		{
			if (!clients.ContainsKey(t.Item1))
				clients.Add(
					t.Item1,
					new TDKomercijalnoClient(GeneralConstants.Danas.Year, env, t.Item1)
				);

			var ceneUMagacinu = clients[t.Item1]
				.Procedure.GetProdajnaCenaNaDanOptimizedAsync(
					new ProceduraGetProdajnaCenaNaDanOptimizedRequest
					{
						Datum = GeneralConstants.Danas,
						MagacinId = t.Item2
					}
				);

			var reperneCeneDanas = await reperneCeneDanasTask;
			var ceneUMagacinuDanas = await ceneUMagacinu;
			foreach (var c in reperneCeneDanas)
			{
				var cenaUMagacinu = ceneUMagacinuDanas.FirstOrDefault(x => x.RobaId == c.RobaId);
				if (cenaUMagacinu == null)
				{
					report.Add(
						new ReportItemDto()
						{
							MagacinId = t.Item2,
							RobaId = c.RobaId,
							Baza = t.Item1.ToString(),
							ProblemSaRobom = new ReportItemDto.ReportItemProblemSaRobomDto()
							{
								Opis =
									"Proizvod koji postoji u repernom magacinu ne postoji u trenutnom."
							}
						}
					);
					continue;
				}

				if (Math.Abs(c.ProdajnaCenaBezPDV - cenaUMagacinu.ProdajnaCenaBezPDV) < 0.02)
					continue;

				report.Add(
					new ReportItemDto()
					{
						MagacinId = t.Item2,
						RobaId = c.RobaId,
						Baza = t.Item1.ToString(),
						ProblemSaCenom = new ReportItemDto.ReportItemProblemSaCenomDto()
						{
							TrenutnaCena = cenaUMagacinu.ProdajnaCenaBezPDV,
							CenaTrebaDaBude = c.ProdajnaCenaBezPDV
						}
					}
				);
			}
		}

		var text = JsonConvert.SerializeObject(report);
		Console.WriteLine(text);
		settingRepository.SetValue(SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_REPORT, text);
		settingRepository.SetValue(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_LAST_RUN,
			DateTime.UtcNow
		);
		settingRepository.SetValue(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_REPORT_ITMES_COUNT,
			report.Count
		);
		settingRepository.SetValue(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_STATUS,
			KomercijalnoProveriCeneUmagacinimaStatus.Idle
		);
	}

	public string GetIzvestajNeispravnihCenaUMagacinimaStatus()
	{
		var setting = settingRepository.GetValueOrDefault<string>(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_STATUS
		);
		return setting ?? KomercijalnoProveriCeneUmagacinimaStatus.Idle.ToString();
	}

	public DateTime? GetIzvjestajNeispravnihCenaUMagacinimaLastRun()
	{
		var setting = settingRepository.GetValueOrDefault<DateTime>(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_LAST_RUN
		);
		return setting;
	}
}
