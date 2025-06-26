using LSCore.Validation.Domain;
using Microsoft.Extensions.Configuration;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Office.PregledIUplataPazara.Contracts.Interfaces;
using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.Responses;
using TD.Office.Public.Client;
using Constants = TD.Common.Environments.Constants;

namespace TD.Office.PregledIUplataPazara.Domain.Managers;

public class PregledIUplataPazaraManager(
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	IConfigurationRoot configurationRoot,
	TDOfficeClient officeClient
) : IPregledIUplataPazaraManager
{
	public async Task<PregledIUplataPazaraResponse> GetAsync(GetPregledIUplataPazaraRequest request)
	{
		request.Validate();
		var dto = new PregledIUplataPazaraResponse();

		var temp = request.OdDatumaInclusive;
		var years = new List<int>();
		while (temp.Year <= request.DoDatumaInclusive.Year)
		{
			years.Add(temp.Year);
			temp = temp.AddYears(1);
		}

		foreach (var magacinId in request.Magacin)
		{
			var magacinFirma = await officeClient.KomercijalnoMagacinFirma.Get(magacinId);

			var izvodi = new Dictionary<int, List<IzvodDto>>();
			var dokumenti = new Dictionary<int, Dictionary<int, List<DokumentDto>>>();
			foreach (var year in years)
			{
				var client = komercijalnoClientFactory.Create(
					DateTime.UtcNow.Year,
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
					var resp = await client.Dokumenti.GetMultiple(
						new DokumentGetMultipleRequest()
						{
							VrDok = [15, 22],
							DatumOd = request.OdDatumaInclusive,
							DatumDo = request.DoDatumaInclusive,
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
				}
			}

			var datumObrade = request.OdDatumaInclusive;
			while (datumObrade.Date <= request.DoDatumaInclusive.Date)
			{
				var izvodiNaDan_N = izvodi[datumObrade.Year]
					.Where(x =>
						!string.IsNullOrEmpty(x.Konto)
						&& (x.Konto.Substring(0, 3) == "243" || x.Konto.Substring(0, 3) == "240")
						&& !string.IsNullOrWhiteSpace(x.PozivNaBroj)
						&& x.PozivNaBroj.Length == 7
						&& Convert.ToInt32(x.PozivNaBroj.Substring(0, 2)) == datumObrade.Month
						&& Convert.ToInt32(x.PozivNaBroj.Substring(2, 2)) == datumObrade.Day
						&& Convert.ToInt32(x.PozivNaBroj.Substring(4, 3)) == magacinId
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
						Povratnice = (double)povratnice_N
					};
					dto.UkupnoPovratnice += item.Povratnice;
					dto.UkupnoMpRacuna += item.MPRacuni;
					if (Math.Abs(item.Razlika) > Math.Abs(request.Tolerancija))
					{
						dto.Items.Add(item);
						dto.UkupnaRazlika += item.Razlika;
					}
				}

				datumObrade = datumObrade.AddDays(1);
			}
		}

		return dto;
	}
}
