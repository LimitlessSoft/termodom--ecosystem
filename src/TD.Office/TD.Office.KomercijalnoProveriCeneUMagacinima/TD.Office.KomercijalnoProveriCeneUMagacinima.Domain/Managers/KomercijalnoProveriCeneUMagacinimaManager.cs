using Newtonsoft.Json;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Constants;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Dtos;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Interfaces.IManagers;

namespace TD.Office.KomercijalnoProveriCeneUMagacinima.Domain.Managers;

public class KomercijalnoProveriCeneUMagacinimaManager(ISettingRepository settingRepository)
	: IKomercijalnoProveriCeneUMagacinimaManager
{
	public async Task GenerateReportAsync()
	{
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

				if (c.ProdajnaCenaBezPDV == cenaUMagacinu.ProdajnaCenaBezPDV)
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
		settingRepository.SetValue(
			SettingKey.PARTNERI_PO_GODINAMA_KOMERCIJALNO_FINANSIJSKO_PERIOD_GODINA,
			text
		);
	}
}
