using System.Text;
using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Helpers;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Managers;

public class ProracunManager(
	IUserRepository userRepository,
	IProracunItemRepository proracunItemRepository,
	IProracunRepository proracunRepository,
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	TDKomercijalnoClient defaultKomercijalnoClient,
	IConfigurationRoot configurationRoot,
	IKomercijalnoMagacinFirmaRepository komercijalnoMagacinFirmaRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IProracunManager
{
	public void Create(ProracuniCreateRequest request)
	{
		request.Validate();
		var currentUser = userRepository.GetCurrentUser();

		if (
			request.Type is ProracunType.Maloprodajni or ProracunType.NalogZaUtovar
			&& currentUser.StoreId == null
		)
			throw new LSCoreBadRequestException(
				string.Format(ProracuniValidationCodes.PVC_002.GetDescription()!, "MP")
			);

		if (request.Type == ProracunType.Veleprodajni && currentUser.VPMagacinId == null)
			throw new LSCoreBadRequestException(
				string.Format(ProracuniValidationCodes.PVC_002.GetDescription()!, "VP")
			);

		if (request.Type == ProracunType.NalogZaUtovar && currentUser.KomercijalnoNalogId == null)
			throw new LSCoreBadRequestException(ProracuniValidationCodes.PVC_001.GetDescription()!);

		proracunRepository.Insert(
			new ProracunEntity
			{
				MagacinId = request.Type switch
				{
					ProracunType.Maloprodajni => currentUser.StoreId!.Value,
					ProracunType.Veleprodajni => currentUser.VPMagacinId!.Value,
					ProracunType.NalogZaUtovar => currentUser.StoreId!.Value,
					_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
				},
				State = ProracunState.Open,
				Type = request.Type,
				NUID = request.Type switch
				{
					ProracunType.Maloprodajni => LegacyConstants.ProracunDefaultNUID,
					ProracunType.Veleprodajni => LegacyConstants.ProfakturaDefaultNUID,
					ProracunType.NalogZaUtovar => LegacyConstants.NalogZaUtovarDefaultNUID,
					_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
				},
				CreatedBy = currentUser.Id,
			}
		);
	}

	public LSCoreSortedAndPagedResponse<ProracunDto> GetMultiple(
		ProracuniGetMultipleRequest request
	)
	{
		var resp = proracunRepository
			.GetMultiple()
			.Include(x => x.User)
			.Include(x => x.Items)
			.Where(x =>
				x.IsActive
				&& (request.MagacinId == null || x.MagacinId == request.MagacinId)
				&& x.CreatedAt >= request.FromUtc
				&& x.CreatedAt <= request.ToUtc
			)
			.ToSortedAndPagedResponse<
				ProracunEntity,
				ProracuniSortColumnCodes.Proracuni,
				ProracunDto
			>(
				request,
				ProracuniSortColumnCodes.ProracuniSortRules,
				x => x.ToMapped<ProracunEntity, ProracunDto>()
			);

		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();

		foreach (var item in resp.Payload!.SelectMany(proracun => proracun.Items))
		{
			var kRoba = komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId);
			item.Naziv = kRoba?.Naziv ?? LegacyConstants.ProracunRobaNotFoundText;
			item.JM = kRoba?.JM ?? LegacyConstants.ProracunRobaNotFoundText;
		}

		return resp;
	}

	public ProracunDto GetSingle(LSCoreIdRequest request)
	{
		var proracun = proracunRepository
			.GetMultiple()
			.Include(x => x.User)
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == request.Id);

		if (proracun == null)
			throw new LSCoreNotFoundException();

		var dto = proracun.ToMapped<ProracunEntity, ProracunDto>();

		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();

		foreach (var item in dto.Items)
		{
			var kRoba = komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId);
			item.Naziv = kRoba?.Naziv ?? LegacyConstants.ProracunRobaNotFoundText;
			item.JM = kRoba?.JM ?? LegacyConstants.ProracunRobaNotFoundText;
		}

		return dto;
	}

	public void PutState(ProracuniPutStateRequest request) =>
		proracunRepository.UpdateState(request.Id!.Value, request.State);

	public void PutPPID(ProracuniPutPPIDRequest request) =>
		proracunRepository.UpdatePPID(request.Id!.Value, request.PPID);

	public void PutNUID(ProracuniPutNUIDRequest request) =>
		proracunRepository.UpdateNUID(request.Id!.Value, request.NUID);

	public async Task<ProracunItemDto> AddItemAsync(ProracuniAddItemRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var proracun = proracunRepository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.Id == request.Id);

		if (proracun == null)
			throw new LSCoreNotFoundException();

		var roba = await defaultKomercijalnoClient.Roba.Get(
			new LSCoreIdRequest() { Id = request.RobaId }
		);

		var prodajnaCenaNaDan = await defaultKomercijalnoClient.Procedure.GetProdajnaCenaNaDanAsync(
			new ProceduraGetProdajnaCenaNaDanRequest()
			{
				Datum = DateTime.Now,
				MagacinId = proracun.Type switch
				{
					ProracunType.Maloprodajni => proracun.MagacinId,
					ProracunType.NalogZaUtovar => proracun.MagacinId,
					ProracunType.Veleprodajni => 150,
					_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
				},
				RobaId = request.RobaId
			}
		);

		var item = new ProracunItemEntity
		{
			RobaId = request.RobaId,
			Kolicina = request.Kolicina,
			CenaBezPdv = (decimal)prodajnaCenaNaDan * (100 / (100 + (decimal)roba.Tarifa.Stopa)),
			Pdv = (decimal)roba.Tarifa.Stopa,
			Rabat = 0,
			IsActive = true,
			CreatedAt = DateTime.UtcNow,
			CreatedBy = currentUser.Id
		};
		proracun.Items.Add(item);
		proracunRepository.Update(proracun);

		var dto = item.ToMapped<ProracunItemEntity, ProracunItemDto>();
		dto.Naziv = roba.Naziv;
		dto.JM = roba.JM;
		return dto;
	}

	public void DeleteItem(LSCoreIdRequest request) =>
		proracunItemRepository.HardDelete(request.Id);

	public void PutItemKolicina(ProracuniPutItemKolicinaRequest request) =>
		proracunItemRepository.UpdateKolicina(request.StavkaId, request.Kolicina);

	public async Task<ProracunDto> ForwardToKomercijalnoAsync(LSCoreIdRequest request)
	{
		var proracun = proracunRepository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == request.Id);

		if (proracun == null)
			throw new LSCoreNotFoundException();

		if (proracun.State != ProracunState.Closed)
			throw new LSCoreBadRequestException("Proračun nije zaključan!");

		if (proracun.KomercijalnoVrDok != null)
			throw new LSCoreBadRequestException("Proračun je već prosleđen u komercijalno!");

		var userEntity = userRepository.Get(proracun.CreatedBy);
		var currentUserEntity = userRepository.GetCurrentUser();

		ProracuniHelpers.HasPermissionToForwad(currentUserEntity, proracun.Type);

		var vrDok = proracun.Type switch
		{
			ProracunType.Maloprodajni => 32,
			ProracunType.Veleprodajni => 4,
			ProracunType.NalogZaUtovar => 34,
			_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
		};

		if (proracun is { NUID: 1, PPID: null })
			throw new LSCoreBadRequestException("Za ovaj nacin uplate obavezan je partner!");

		var magacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(proracun.MagacinId);
		#region Create document in Komercijalno
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			magacinFirma.ApiFirma
		);
		var komercijalnoDokument = await client.Dokumenti.CreateAsync(
			new DokumentCreateRequest
			{
				VrDok = vrDok,
				MagacinId = (short)proracun.MagacinId,
				ZapId = proracun.Type switch
				{
					ProracunType.Maloprodajni => 107,
					ProracunType.Veleprodajni => 107,
					ProracunType.NalogZaUtovar => (short)userEntity.KomercijalnoNalogId!.Value,
					_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
				},
				RefId = proracun.Type switch
				{
					ProracunType.Maloprodajni => 107,
					ProracunType.Veleprodajni => 107,
					ProracunType.NalogZaUtovar => (short)userEntity.KomercijalnoNalogId!.Value,
					_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
				},
				// IntBroj = "Web: " + request.OneTimeHash[..8],
				Flag = 0,
				KodDok = 0,
				Linked = "0000000000",
				PPID = proracun.PPID,
				Placen = 0,
				NuId = (short)proracun.NUID,
				NrId = 1,
			}
		);
		#endregion

		proracun.KomercijalnoVrDok = komercijalnoDokument.VrDok;
		proracun.KomercijalnoBrDok = komercijalnoDokument.BrDok;

		proracunRepository.Update(proracun);

		#region Insert items into komercijalno dokument
		foreach (var item in proracun.Items)
		{
			await client.Stavke.CreateAsync(
				new StavkaCreateRequest
				{
					VrDok = komercijalnoDokument.VrDok,
					BrDok = komercijalnoDokument.BrDok,
					RobaId = item.RobaId,
					Kolicina = Convert.ToDouble(item.Kolicina),
					ProdajnaCenaBezPdv = Convert.ToDouble(item.CenaBezPdv),
					Rabat = (double)item.Rabat,
					CeneVuciIzOvogMagacina = proracun.Type switch
					{
						ProracunType.Maloprodajni => null,
						ProracunType.Veleprodajni => proracun.MagacinId == 2223 ? 2223 : 150,
						ProracunType.NalogZaUtovar => null,
						_ => throw new LSCoreBadRequestException("Nepoznat tip proračuna")
					}
				}
			);
		}
		#endregion

		#region insert interni komentar
		var interniKomentarSb = new StringBuilder();
		interniKomentarSb.Append("Prosleđeno iz Office aplikacije");
		if (!string.IsNullOrWhiteSpace(proracun.Email))
			interniKomentarSb.Append($"Email: {proracun.Email}");
		await client.Komentari.CreateAsync(
			new CreateKomentarRequest()
			{
				VrDok = komercijalnoDokument.VrDok,
				BrDok = komercijalnoDokument.BrDok,
				InterniKomentar = interniKomentarSb.ToString(),
			}
		);
		#endregion

		return GetSingle(request);
	}

	public void PutItemRabat(ProracuniPutItemRabatRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();

		var item = proracunItemRepository.Get(request.StavkaId);
		var proracun = proracunRepository.Get(request.Id);
		if (
			proracun.Type == ProracunType.Maloprodajni
			|| proracun.Type == ProracunType.NalogZaUtovar
				&& request.Rabat > currentUser.MaxRabatMPDokumenti
		)
			throw new LSCoreBadRequestException("Nemate pravo da date ovako visok rabat!");

		if (
			proracun.Type == ProracunType.Veleprodajni
			&& request.Rabat > currentUser.MaxRabatVPDokumenti
		)
			throw new LSCoreBadRequestException("Nemate pravo da date ovako visok rabat!");

		item.Rabat = request.Rabat;
		proracunItemRepository.Update(item);
	}

	public void PutEmail(ProracuniPutEmailRequest request)
	{
		request.Validate();
		var proracun = proracunRepository.Get(request.Id);
		proracun.Email = request.Email;
		proracunRepository.Update(proracun);
	}
	
	public void PutRecommendedValue(ProracuniPutRecommendedValueRequest request)
	{
		var proracun = proracunRepository.Get(request.Id);
		proracun.RecommendedValue = request.RecommendedValue;
		proracunRepository.Update(proracun);
	}
}
