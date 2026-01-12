using LSCore.Validation.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Office.PregledIUplataPazara.Contracts.Constants;
using TD.Office.PregledIUplataPazara.Contracts.Interfaces;
using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.Responses;
using TD.Office.Public.Client;
using Constants = TD.Common.Environments.Constants;

namespace TD.Office.PregledIUplataPazara.Domain.Managers;

public class PregledIUplataPazaraManager(
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	IConfigurationRoot configurationRoot,
	TDOfficeClient officeClient,
	ILogger<PregledIUplataPazaraManager> logger
) : IPregledIUplataPazaraManager
{
	public async Task<PregledIUplataPazaraResponse> GetAsync(GetPregledIUplataPazaraRequest request)
	{
		request.Validate();
		string belgradeTimeZoneId = "Central Europe Standard Time";
		var belgradeTimezone = TimeZoneInfo.FindSystemTimeZoneById(belgradeTimeZoneId);
		request.OdDatumaInclusive = TimeZoneInfo.ConvertTimeFromUtc(
			request.OdDatumaInclusive,
			belgradeTimezone
		);
		request.DoDatumaInclusive = TimeZoneInfo.ConvertTimeFromUtc(
			request.DoDatumaInclusive,
			belgradeTimezone
		);
		try
		{
			var dto = new PregledIUplataPazaraResponse();

			var temp = request.OdDatumaInclusive;
			var years = new List<int>();
			while (temp.Year <= request.DoDatumaInclusive.Year)
			{
				years.Add(temp.Year);
				temp = temp.AddYears(1);
			}
			var narednaGodina = years.Max() + 1;
			if (narednaGodina <= DateTime.UtcNow.Year)
				years.Add(narednaGodina);

			foreach (var magacinId in request.Magacin)
			{
				var magacinFirma = await officeClient.KomercijalnoMagacinFirma.Get(magacinId);

				var izvodi = new Dictionary<int, List<IzvodDto>>();
				var dokumenti = new Dictionary<int, Dictionary<int, List<DokumentDto>>>();
				foreach (var year in years)
				{
					var client = komercijalnoClientFactory.Create(
						year,
						TDKomercijalnoClientHelpers.ParseEnvironment(
							configurationRoot[Constants.DeployVariable]!
						),
						magacinFirma.ApiFirma
					);
					await Task.WhenAll(FetchDokumentiAsync(), FetchIzvodiAsync());
					continue;

					async Task FetchDokumentiAsync()
					{
						if (!dokumenti.ContainsKey(year))
							dokumenti.Add(year, []);
						var resp = await client.Dokumenti.GetMultipleAsync(
							new DokumentGetMultipleRequest()
							{
								VrDok = [15, 22, 90],
								DatumOd = request.OdDatumaInclusive.AddDays(-1),
								DatumDo = request.DoDatumaInclusive.AddDays(1),
							}
						);
						dokumenti[year] = resp.GroupBy(x => x.VrDok)
							.ToDictionary(g => g.Key, g => g.ToList());
					}

					async Task FetchIzvodiAsync()
					{
						if (!izvodi.ContainsKey(year))
							izvodi.Add(year, []);
						izvodi[year]
							.AddRange(
								await client.Izvodi.GetMultipleAsync(new IzvodGetMultipleRequest())
							);
						if (year != DateTime.UtcNow.Year && year == years.Max())
						{
							// Uzimam i narednu godinu izvoda jer nam treba jer tamo postoji referentni prethodne godine
							var y = year + 1;
							if (!izvodi.ContainsKey(y))
								izvodi.Add(y, []);

							var clientTemp = komercijalnoClientFactory.Create(
								y,
								TDKomercijalnoClientHelpers.ParseEnvironment(
									configurationRoot[Constants.DeployVariable]!
								),
								magacinFirma.ApiFirma
							);
							izvodi[y]
								.AddRange(
									await clientTemp.Izvodi.GetMultipleAsync(
										new IzvodGetMultipleRequest()
									)
								);
						}
					}
				}

				var datumObrade = request.OdDatumaInclusive;
				while (datumObrade.Date <= request.DoDatumaInclusive.Date)
				{
					var izvodiNaDan_N = izvodi[datumObrade.Year]
						.Where(x =>
							!string.IsNullOrEmpty(x.Konto)
							&& (
								x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240"
							)
							&& !string.IsNullOrWhiteSpace(x.PozivNaBroj)
							&& x.PozivNaBroj.Length == 7
							&& Convert.ToInt32(x.PozivNaBroj.Substring(0, 2)) == datumObrade.Month
							&& Convert.ToInt32(x.PozivNaBroj.Substring(2, 2)) == datumObrade.Day
							&& Convert.ToInt32(x.PozivNaBroj.Substring(4, 3)) == magacinId
						)
						.ToList();
					izvodiNaDan_N.AddRange(
						izvodi[datumObrade.Year + 1]
							.Where(x =>
								!string.IsNullOrEmpty(x.Konto)
								&& (
									x.Konto.Substring(0, 3) == "243"
									|| x.Konto.Substring(0, 3) == "240"
								)
								&& !string.IsNullOrWhiteSpace(x.PozivNaBroj)
								&& x.PozivNaBroj.Length == 12
								&& Convert.ToInt32(x.PozivNaBroj.Substring(0, 2))
									== datumObrade.Month
								&& Convert.ToInt32(x.PozivNaBroj.Substring(2, 2)) == datumObrade.Day
								&& Convert.ToInt32(x.PozivNaBroj.Substring(4, 3)) == magacinId
								&& Convert.ToInt32(x.PozivNaBroj.Substring(8, 4))
									== datumObrade.Date.Year
							)
							.ToList()
					);

					var konto_N = string.Empty;
					var pozNaBroj_N = string.Empty;
					double potrazuje_N = 0;
					foreach (var i in izvodiNaDan_N)
					{
						konto_N = i.Konto;
						pozNaBroj_N = i.PozivNaBroj;
						potrazuje_N += i.Potrazuje ?? 0;
					}

					var mpRacuni_N = !dokumenti[datumObrade.Year].ContainsKey(15)
						? 0
						: dokumenti[datumObrade.Year]
							[15]
							.Where(x =>
								x.Datum.Date == datumObrade.Date
								&& x.MagacinId == magacinId
								&& x.NuId != 1
							)
							.Sum(x => x.Potrazuje);

					var povratnice_N = !dokumenti[datumObrade.Year].ContainsKey(22)
						? 0
						: dokumenti[datumObrade.Year]
							[22]
							.Where(x =>
								x.Datum.Date == datumObrade.Date
								&& x.MagacinId == magacinId
								&& x.NuId != 1
							)
							.Sum(x => x.Potrazuje);

					if (
						izvodiNaDan_N.Count() > 0
						|| mpRacuni_N != 0
						|| povratnice_N != 0
						|| potrazuje_N != 0
					)
					{
						var item = new PregledIUplataPazaraResponseItem
						{
							Konto = konto_N,
							PozNaBroj = pozNaBroj_N,
							Potrazuje = potrazuje_N,
							Datum = datumObrade.Date,
							MagacinId = magacinId,
							MPRacuni = (double)mpRacuni_N,
							Povratnice = (double)povratnice_N,
						};
						dto.UkupnoPovratnice += item.Povratnice;
						dto.UkupnoMpRacuna += item.MPRacuni;
						dto.UkupnoPotrazuje += item.Potrazuje;
						if (Math.Abs(item.Razlika) > Math.Abs(request.Tolerancija))
						{
							dto.Items.Add(item);
							dto.UkupnaRazlika += item.Razlika;
							item.Izvodi.AddRange(
								izvodiNaDan_N.Select(x =>
								{
									return new PregledIUplataPazaraResponseItemIzvodDto()
									{
										BrojIzvoda = x.BrDok,
										VrDok = x.VrDok,
										ZiroRacun = x.ZiroRacun,
										Konto = x.Konto,
										PozivNaBroj = x.PozivNaBroj,
										MagacinId = magacinId,
										Potrazuje = x.Potrazuje ?? 0,
										Duguje = x.Duguje ?? 0,
									};
								})
							);
						}
					}

					datumObrade = datumObrade.AddDays(1);
				}
			}

			return dto;
		}
		catch (Exception e)
		{
			logger.LogError(e.ToString());
			throw;
		}
	}

	public async Task<PregledIUplataPazaraNeispravneStavkeIzvodaResponse> GetNeispravneStavkeIzvoda()
	{
		try
		{
			var resp = new PregledIUplataPazaraNeispravneStavkeIzvodaResponse();
			var firmaClients = Enum.GetValues<TDKomercijalnoFirma>()
				.Select(firma => new
				{
					client = komercijalnoClientFactory.Create(
						DateTime.UtcNow.Year,
						TDKomercijalnoClientHelpers.ParseEnvironment(
							configurationRoot[Constants.DeployVariable]!
						),
						firma
					),
					firma = firma,
				})
				.ToDictionary(x => x.firma, x => x.client);

			foreach (var firma in firmaClients.Keys)
			{
				var izvodi = await firmaClients[firma]
					.Izvodi.GetMultipleAsync(new IzvodGetMultipleRequest());
				foreach (var izvod in izvodi)
				{
					if (
						!NeispravneStavkeIzvodaConstants.Configuration.Items.ContainsKey(izvod.PPID)
					)
						continue;

					if (izvod.SifraPlacanja != "165")
						continue;

					var configItem = NeispravneStavkeIzvodaConstants.Configuration.Items[
						izvod.PPID
					];
					if (string.IsNullOrWhiteSpace(izvod.PozivNaBroj))
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								"Prazan poziv na broj"
							)
						);
						continue;
					}
					if (izvod.PozivNaBroj.EndsWith($"/{DateTime.UtcNow.Year - 1}"))
					{
						izvod.PozivNaBroj = izvod.PozivNaBroj.Substring(
							0,
							izvod.PozivNaBroj.Length - 5
						);
					}
					if (izvod.PozivNaBroj.Length != 7)
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Neispravna dužina poziva na broj [{izvod.PozivNaBroj}]"
							)
						);
						continue;
					}
					if (
						!int.TryParse(
							izvod.PozivNaBroj.Substring(izvod.PozivNaBroj.Length - 4, 3),
							out _
						)
					)
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Poslednja 3 karaktera diktira na broj [{izvod.PozivNaBroj}] moraju biti cifre!"
							)
						);
						continue;
					}
					if (
						izvod.PozivNaBroj.Substring(izvod.PozivNaBroj.Length - 3, 3)
						!= configItem.PozivNaBrojPoslednjeTriCifre.ToString()
					)
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Podesavanje partnera diktira da poslednja 3 karaktera poziva na broj moraju biti [{configItem.PozivNaBrojPoslednjeTriCifre}]. Trenutno [{izvod.PozivNaBroj.Substring(izvod.PozivNaBroj.Length - 3, 3)}]"
							)
						);
						continue;
					}

					// ====

					if (string.IsNullOrWhiteSpace(izvod.Konto))
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								"Prazan konto"
							)
						);
						continue;
					}

					if (izvod.Konto.Length != 5)
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Konto [{izvod.Konto}] treba ima duzinu 5"
							)
						);
						continue;
					}
					if (izvod.Konto.Substring(0, 3) != configItem.KontoPrveTriCifre.ToString())
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Podesavanje partnera diktira da prve tri cifre konta moraju biti [{configItem.KontoPrveTriCifre}]. Trenutno [{izvod.Konto.Substring(0, 3)}]"
							)
						);
						continue;
					}
					if (izvod.Konto.Substring(3, 2) != configItem.KontoPoslednjeDveCifre.ToString())
					{
						resp.Items.Add(
							new PregledIUplataPazaraNeispravneStavkeIzvodaResponse.Item(
								firma.ToString(),
								izvod.BrDok,
								izvod.PPID,
								$"Podesavanje partnera diktira da poslednje dve cifre konta moraju biti [{configItem.KontoPoslednjeDveCifre}]. Trenutno [{izvod.Konto.Substring(3, 2)}]"
							)
						);
						continue;
					}
				}
			}
			return resp;
		}
		catch (Exception e)
		{
			logger.LogError(e.ToString());
			throw;
		}
	}
}
