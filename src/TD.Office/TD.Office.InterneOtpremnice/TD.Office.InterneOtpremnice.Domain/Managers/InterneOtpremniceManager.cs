using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Configuration;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;
using TD.Office.Public.Client;

namespace TD.Office.InterneOtpremnice.Domain.Managers;

public class InterneOtpremniceManager(
	IInternaOtpremnicaRepository internaOtpremnicaRepository,
	TDKomercijalnoClient komercijalnoClient,
	IConfigurationRoot configurationRoot,
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	TDOfficeClient tdOfficeClient
) : IInterneOtpremniceManager
{
	public async Task<InternaOtpremnicaDetailsDto> GetAsync(IdRequest request)
	{
		var robaTask = komercijalnoClient.Roba.GetMultipleAsync(
			new RobaGetMultipleRequest { Vrsta = 1 }
		);

		var dto = internaOtpremnicaRepository
			.GetDetailed(request.Id)
			.ToMapped<InternaOtpremnicaEntity, InternaOtpremnicaDetailsDto>();

		var robaDict = (await robaTask).ToDictionary(x => x.RobaId, x => x);
		Parallel.ForEach(
			dto.Items,
			item =>
			{
				var prodajnaCenaNaDan = komercijalnoClient
					.Procedure.GetProdajnaCenaNaDanAsync(
						new ProceduraGetProdajnaCenaNaDanRequest
						{
							Datum = DateTime.Now,
							MagacinId = 150,
							RobaId = item.RobaId
						}
					)
					.GetAwaiter()
					.GetResult();
				robaDict.TryGetValue(item.RobaId, out var r);
				item.Proizvod = r?.Naziv ?? "Nepoznat proizvod";
				item.JM = r?.JM ?? "Nepoznat proizvod";
				item.Pdv = (decimal)(r?.Tarifa.Stopa ?? 0);
				item.Cena = prodajnaCenaNaDan * (100 / (100 + item.Pdv));
			}
		);

		return dto;
	}

	public InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request) =>
		internaOtpremnicaRepository
			.Create(request.PolazniMagacinId, request.DestinacioniMagacinId, request.CreatedBy)
			.ToMapped<InternaOtpremnicaEntity, InternaOtpremnicaDto>();

	public async Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
		GetMultipleRequest request
	)
	{
		request.Validate();
		List<long> magaciniId = [];

		if (request.MagacinId.HasValue)
		{
			magaciniId.Add(request.MagacinId.Value);
		}
		else
		{
			var magaciniIzabraneVrste = await komercijalnoClient.Magacini.GetMultipleAsync(
				new MagaciniGetMultipleRequest
				{
					Vrsta =
					[
						request.Vrsta switch
						{
							InternaOtpremnicaVrsta.VP => MagacinVrsta.Veleprodajni,
							InternaOtpremnicaVrsta.MP => MagacinVrsta.Maloprodajni,
							_ => throw new NotImplementedException()
						}
					]
				}
			);

			magaciniId = magaciniIzabraneVrste.Select(x => x.MagacinId).ToList();
		}
		return internaOtpremnicaRepository
			.GetMultiple()
			.Where(x => magaciniId.Contains(x.PolazniMagacinId))
			.ToSortedAndPagedResponse<
				InternaOtpremnicaEntity,
				InterneOtpremniceSortColumnCodes.InterneOtpremnice,
				InternaOtpremnicaDto
			>(
				request,
				InterneOtpremniceSortColumnCodes.Rules,
				x => x.ToMapped<InternaOtpremnicaEntity, InternaOtpremnicaDto>()
			);
	}

	public InternaOtpremnicaItemDto SaveItem(InterneOtpremniceItemCreateRequest request) =>
		internaOtpremnicaRepository
			.SaveItem(request.Id, request.InternaOtpremnicaId, request.RobaId, request.Kolicina)
			.ToMapped<InternaOtpremnicaItemEntity, InternaOtpremnicaItemDto>();

	public void DeleteItem(InterneOtpremniceItemDeleteRequest request) =>
		internaOtpremnicaRepository.HardDeleteItem(request.Id);

	public void ChangeState(IdRequest request, InternaOtpremnicaStatus state) =>
		internaOtpremnicaRepository.SetStatus(request.Id, state);

	public async Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalnoAsync(IdRequest request)
	{
		var magaciniTask = komercijalnoClient.Magacini.GetMultipleAsync(
			new MagaciniGetMultipleRequest()
		);
		var internaOtpremnica = internaOtpremnicaRepository.GetDetailed(request.Id);

		var polazniMagacinFirma = await tdOfficeClient.KomercijalnoMagacinFirma.Get(
			internaOtpremnica.PolazniMagacinId
		);
		var destinacioniMagacinFirma = await tdOfficeClient.KomercijalnoMagacinFirma.Get(
			internaOtpremnica.DestinacioniMagacinId
		);
		if (destinacioniMagacinFirma.ApiFirma != polazniMagacinFirma.ApiFirma)
			throw new LSCoreForbiddenException(
				"Pokusavate da prebacite robu internim dokumentom izmedju razlicitih firmi (baza) sto nije dozvoljeno!"
			);
		var magacini = await magaciniTask;
		var polazniMagacin = magacini.First(x => x.MagacinId == internaOtpremnica.PolazniMagacinId);
		var destinacioniMagacin = magacini.First(x =>
			x.MagacinId == internaOtpremnica.DestinacioniMagacinId
		);

		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			polazniMagacinFirma.ApiFirma
		);

		#region Otpremnica
		var dokumentOtpremniceKomercijalno = await client.Dokumenti.CreateAsync(
			new DokumentCreateRequest
			{
				VrDok = polazniMagacin.Vrsta switch
				{
					MagacinVrsta.Maloprodajni => 19,
					MagacinVrsta.Veleprodajni => 25,
					_ => throw new LSCoreBadRequestException("Nepoznata vrsta magacina")
				},
				MagacinId = (short)internaOtpremnica.PolazniMagacinId,
				MagId = (short)internaOtpremnica.DestinacioniMagacinId,
				ZapId = 107,
				RefId = 107,
				// IntBroj = "Web: " + request.OneTimeHash[..8],
				Flag = 0,
				KodDok = 0,
				Linked = "0000000000",
				Placen = 0,
				NrId = 1,
			}
		);

		foreach (var item in internaOtpremnica.Items)
		{
			await client.Stavke.CreateAsync(
				new StavkaCreateRequest
				{
					VrDok = dokumentOtpremniceKomercijalno.VrDok,
					BrDok = dokumentOtpremniceKomercijalno.BrDok,
					RobaId = item.RobaId,
					Kolicina = (double)item.Kolicina
				}
			);
		}
		#endregion

		#region Kalkulacija
		var dokumentKalkulacijeKomercijalno = await client.Dokumenti.CreateAsync(
			new DokumentCreateRequest
			{
				VrDok = destinacioniMagacin.Vrsta switch
				{
					MagacinVrsta.Maloprodajni => 18,
					MagacinVrsta.Veleprodajni => 26,
					_ => throw new LSCoreBadRequestException("Nepoznata vrsta magacina")
				},
				MagacinId = (short)internaOtpremnica.DestinacioniMagacinId,
				ZapId = 107,
				RefId = 107,
				// IntBroj = "Web: " + request.OneTimeHash[..8],
				Flag = 0,
				KodDok = 0,
				Linked = "0000000000",
				Placen = 0,
				NrId = 1,
				VrdokIn = (short)dokumentOtpremniceKomercijalno.VrDok,
				BrDokIn = dokumentOtpremniceKomercijalno.BrDok
			}
		);

		foreach (var item in internaOtpremnica.Items)
		{
			await client.Stavke.CreateAsync(
				new StavkaCreateRequest
				{
					VrDok = dokumentKalkulacijeKomercijalno.VrDok,
					BrDok = dokumentKalkulacijeKomercijalno.BrDok,
					RobaId = item.RobaId,
					Kolicina = (double)item.Kolicina
				}
			);
		}

		// Update otpremnica out with kalkulacija values
		await client.Dokumenti.UpdateDokOut(
			new DokumentSetDokOutRequest
			{
				VrDok = dokumentOtpremniceKomercijalno.VrDok,
				BrDok = dokumentOtpremniceKomercijalno.BrDok,
				VrDokOut = (short)dokumentKalkulacijeKomercijalno.VrDok,
				BrDokOut = dokumentKalkulacijeKomercijalno.BrDok
			}
		);

		internaOtpremnica.KomercijalnoVrDok = dokumentOtpremniceKomercijalno.VrDok;
		internaOtpremnica.KomercijalnoBrDok = dokumentOtpremniceKomercijalno.BrDok;
		internaOtpremnicaRepository.Update(internaOtpremnica);
		#endregion

		return internaOtpremnica.ToMapped<InternaOtpremnicaEntity, InternaOtpremnicaDetailsDto>();
	}
}
