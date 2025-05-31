using System.Collections.Concurrent;
using System.Net;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Dtos;
using TD.Office.Public.Contracts.Dtos.Izvestaji;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Izvestaji;
using Constants = TD.Common.Environments.Constants;

namespace TD.Office.Public.Domain.Managers;

public class IzvestajManager(
	IConfigurationRoot configurationRoot,
	ITDKomercijalnoClientFactory tdKomercijalnoClientFactory,
	TDKomercijalnoClient tdKomercijalnoClient,
	ITDKomercijalnoApiManager tdKomercijalnoApiManager,
	IMagacinCentarRepository magacinCentarRepository,
	IKomercijalnoMagacinFirmaRepository komercijalnoMagacinFirmaRepository,
	ISettingRepository settingRepository
) : IIzvestajManager
{
	private async Task<
		List<DokumentDto>
	> GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
		GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
	)
	{
		return await tdKomercijalnoApiManager.GetMultipleDokumentAsync(
			new DokumentGetMultipleRequest()
			{
				VrDok = [request.VrDok],
				NUID = [request.NUID],
				DatumOd = request.DatumOd,
				DatumDo = request.DatumDo,
				MagacinId = request.MagacinId
			}
		);
	}

	public async Task<GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaDto> GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
		GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
	)
	{
		var roba = tdKomercijalnoApiManager.GetMultipleRobaAsync(
			new RobaGetMultipleRequest() { Vrsta = 1 }
		);

		var dokumenti =
			await GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

		var stavke = dokumenti.SelectMany(d => d.Stavke ?? []).ToList();

		var kolicinePoRobaId = stavke
			.GroupBy(s => s.RobaId)
			.Select(g => new
			{
				RobaId = g.Key,
				Naziv = roba.Result.FirstOrDefault(x => x.RobaId == g.Key)?.Naziv ?? "Undefined",
				Kolicina = g.Sum(s => s.Kolicina)
			})
			.ToList();

		return new GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaDto()
		{
			Items = kolicinePoRobaId
				.Select(x => new GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaItemDto()
				{
					RobaId = x.RobaId,
					Naziv = x.Naziv,
					Kolicina = x.Kolicina
				})
				.ToList()
		};
	}

	public async Task ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
		ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
	)
	{
		var destinacioniDokument = tdKomercijalnoApiManager.GetDokumentAsync(
			new DokumentGetRequest()
			{
				VrDok = request.DestinationVrDok,
				BrDok = request.DestinationBrDok
			}
		);
		var izvestaj = GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

		var des = await destinacioniDokument;
		var izv = await izvestaj;

		if (des.Flag == 1)
			throw new LSCoreBadRequestException("Dokument mora biti otkljucan!");

		foreach (var stavka in izv.Items)
		{
			await tdKomercijalnoApiManager.CreateStavkaAsync(
				new StavkaCreateRequest
				{
					RobaId = stavka.RobaId,
					Kolicina = stavka.Kolicina,
					VrDok = des.VrDok,
					BrDok = des.BrDok,
				}
			);
		}
	}

	public async Task PromeniNacinUplateAsync(PromeniNacinUplateRequest request)
	{
		var dokumenti =
			await GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

		foreach (var dokument in dokumenti)
		{
			await tdKomercijalnoApiManager.SetDokumentNacinPlacanjaAsync(
				new DokumentSetNacinPlacanjaRequest()
				{
					VrDok = dokument.VrDok,
					BrDok = dokument.BrDok,
					NUID = request.DestinationNuid
				}
			);
		}
	}

	public async Task<
		Dictionary<string, Dictionary<string, object>>
	> GetIzvestajIzlazaRobePoGodinamaAsync(GetIzvestajIzlazaRobePoGodinamaRequest request)
	{
		request.Validate();

		request.OdDatuma = request.OdDatuma.AddHours(1);
		request.DoDatuma = request.DoDatuma.AddHours(1);
		var centri = magacinCentarRepository.GetAllContainingMagacinIds(request.Magacin);
		var sumKolonaDugeVrDoks = new[] { 13, 34 };
		var dict = new Dictionary<string, Dictionary<string, object>>();

		var tdKomercijalnoEnvironment = TDKomercijalnoClientHelpers.ParseEnvironment(
			configurationRoot[Constants.DeployVariable]!
		);
		var apisByYear = new Dictionary<int, Dictionary<int, TDKomercijalnoClient>>();
		foreach (var godina in request.Godina)
		{
			if (!apisByYear.ContainsKey(godina))
				apisByYear.Add(godina, new Dictionary<int, TDKomercijalnoClient>());
			foreach (var magacin in request.Magacin)
			{
				var magacinFirma = komercijalnoMagacinFirmaRepository.GetBymagacinIdOrDefault(
					magacin
				);
				if (magacinFirma == null)
					throw new LSCoreBadRequestException(
						$"Magacin {magacin} nije povezan sa firmom!"
					);
				try
				{
					var api = tdKomercijalnoClientFactory.Create(
						godina,
						tdKomercijalnoEnvironment,
						magacinFirma.ApiFirma
					);
					apisByYear[godina].Add(magacin, api);
				}
				catch (InvalidOperationException) { } // This is totally valid since VHEMZA or some other have no older DBs
			}
		}

		foreach (var centar in centri)
		{
			var dokumentiIzlazaSvihmagacinaUCentru = new ConcurrentBag<DokumentDto>();
			dict.Add(centar.Naziv, new Dictionary<string, object>());
			Parallel.ForEach(
				request.Magacin.Intersect(centar.MagacinIds).ToList(),
				magacin =>
				{
					var dokumentiIzlaza = new ConcurrentBag<DokumentDto>();
					Parallel.ForEach(
						request.Godina,
						godina =>
						{
							if (!apisByYear.TryGetValue(godina, out var apiForYear)) // this is valid since some FIRMAs do not have apis for some years (earlier years)
								return;
							if (!apiForYear.TryGetValue(magacin, out var api))
								return; // this is valid since some magacins do not have apis for some years (earlier years)
							var di = api
								.Dokumenti.GetMultiple(
									new DokumentGetMultipleRequest
									{
										VrDok = request.VrDok.ToArray(),
										DatumOd = new DateTime(request.Godina.Min(), 1, 1),
										DatumDo = new DateTime(request.Godina.Max(), 12, 31),
										MagacinId = magacin
									}
								)
								.GetAwaiter()
								.GetResult();

							foreach (var d in di)
							{
								dokumentiIzlaza.Add(d);
								dokumentiIzlazaSvihmagacinaUCentru.Add(d);
							}
						}
					);
					dict[centar.Naziv].Add($"magacin{magacin}", new Dictionary<string, object>());

					var node =
						dict[centar.Naziv][$"magacin{magacin}"] as Dictionary<string, object>;
					node.Add("naziv", $"magacin{magacin}");

					foreach (var year in request.Godina)
					{
						node.Add($"godina{year}", new Dictionary<string, object>());
						var yearNode = node[$"godina{year}"] as Dictionary<string, object>;
						var odD = new DateTime(year, request.OdDatuma.Month, request.OdDatuma.Day);
						var doD = new DateTime(year, request.DoDatuma.Month, request.DoDatuma.Day);
						yearNode!.Add(
							"vrednost",
							dokumentiIzlaza
								.Where(d =>
									d.Datum.Year == year && d.Datum >= odD && d.Datum <= doD
								)
								.Sum(d =>
									sumKolonaDugeVrDoks.Contains(d.VrDok) ? d.Duguje : d.Potrazuje
								)
						);

						yearNode.Add("dokumenti", new Dictionary<string, object>());
						var dokumentiNode = yearNode["dokumenti"] as Dictionary<string, object>;

						var vrDoks = dokumentiIzlaza
							.Where(d => d.Datum.Year == year && d.Datum >= odD && d.Datum <= doD)
							.GroupBy(d => d.VrDok)
							.Select(g => new
							{
								VrDok = g.Key,
								Vrednost = g.Sum(d =>
									sumKolonaDugeVrDoks.Contains(d.VrDok) ? d.Duguje : d.Potrazuje
								)
							})
							.ToDictionary(
								x => $"v{x.VrDok}",
								x => new Dictionary<string, object>()
								{
									{ "naziv", x.VrDok },
									{ "vrednost", x.Vrednost }
								}
							);

						foreach (var key in vrDoks.Keys)
							dokumentiNode!.Add(key, vrDoks[key]);
					}
				}
			);

			foreach (var godina in request.Godina)
			{
				var odD = new DateTime(godina, request.OdDatuma.Month, request.OdDatuma.Day);
				var doD = new DateTime(godina, request.DoDatuma.Month, request.DoDatuma.Day);
				dict[centar.Naziv]
					.TryAdd(
						$"godina{godina}",
						dokumentiIzlazaSvihmagacinaUCentru
							.Where(x => x.Datum.Year == godina && x.Datum >= odD && x.Datum <= doD)
							.Sum(d =>
								sumKolonaDugeVrDoks.Contains(d.VrDok) ? d.Duguje : d.Potrazuje
							)
					);
			}
		}

		return dict;
	}

	public GetIzvestajNeispravnihCenaUMagacinimaDto GetIzvestajNeispravnihCenaUMagacinima()
	{
		var reportSetting = settingRepository.GetOrDefault(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_REPORT
		);
		if (reportSetting == null)
			return new GetIzvestajNeispravnihCenaUMagacinimaDto();

		var report = JsonConvert.DeserializeObject<List<ReportItemDto>>(reportSetting.Value)!;
		return new GetIzvestajNeispravnihCenaUMagacinimaDto()
		{
			Items = report.ToMapped<
				List<ReportItemDto>,
				List<GetIzvestajNeispravnihCenaUMagacinimaDto.Item>
			>()
		};
	}

	public int GetIzvestajNeispravnihCenaUMagacinimaCount()
	{
		var setting = settingRepository.GetOrDefault(
			SettingKey.KOMERCIJALNO_PROVERI_CENE_U_MAGACINIMA_REPORT_ITMES_COUNT
		);
		return setting == null ? 0 : Convert.ToInt32(setting.Value);
	}
}
