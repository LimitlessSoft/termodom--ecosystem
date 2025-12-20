using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Dtos.Popisi;
using TD.Office.Public.Contracts.Enums;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Domain.Managers;

public class PopisManager(
	ILogger<PopisManager> logger,
	IPopisRepository repository,
	IUserRepository userRepository,
	TDKomercijalnoClient defaultKomercijalnoClient,
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	IKomercijalnoMagacinFirmaRepository komercijalnoMagacinFirmaRepository,
	IConfigurationRoot configurationRoot
) : IPopisManager
{
	public LSCoreSortedAndPagedResponse<PopisDto> GetMultiple(GetPopisiRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		// Module access permission check
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		if (currentUser is { Type: UserType.User, StoreId: null })
			throw new LSCoreBadRequestException(
				"Korisnik nema dodeljen magacin i nije administrator."
			);
		var magacinId =
			currentUser.Type == UserType.SuperAdministrator
				? request.MagacinId
				: currentUser.StoreId;

		// Date filter permission checks
		var hasAllDatesPermission = userRepository.HasPermission(
			currentUser.Id,
			Permission.RobaPopisFilterSviDatumi
		);
		var hasLast7DaysPermission = userRepository.HasPermission(
			currentUser.Id,
			Permission.RobaPopisFilter7DanaUnazad
		);

		if (request.FromDate != null || request.ToDate != null)
		{
			if (!hasAllDatesPermission)
			{
				var nowDate = DateTime.UtcNow;
				var minAllowed = nowDate.AddDays(hasLast7DaysPermission ? -7 : -1);
				var from = request.FromDate ?? nowDate;
				if (from < minAllowed)
					throw new LSCoreForbiddenException();
			}
		}
		return repository
			.GetMultiple()
			.Where(x =>
				(magacinId == null || x.MagacinId == magacinId)
				&& (request.FromDate == null || x.CreatedAt >= request.FromDate)
				&& (request.ToDate == null || x.CreatedAt <= request.ToDate)
			)
			.ToSortedAndPagedResponse<PopisDokumentEntity, PopisiSortColumnCodes.Popisi, PopisDto>(
				request,
				PopisiSortColumnCodes.PopisiSortRules,
				x => x.ToMapped<PopisDokumentEntity, PopisDto>()
			);
	}

	DateTime GetPreviousSunday(DateTime date) =>
		date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Sunday);

	async Task<DokumentDto> CreateKomercijalnoPopisAsync(
		TDKomercijalnoClient client,
		int magacinId,
		PopisDokumentTime popisDokumentTime
	)
	{
		try
		{
			return await client.Dokumenti.CreateAsync(
				new DokumentCreateRequest
				{
					VrDok = 7,
					MagacinId = (short)magacinId,
					MagId = (short?)magacinId,
					Datum =
						popisDokumentTime == PopisDokumentTime.Jucerasnji
							? DateTime.UtcNow.AddDays(-1)
							: GetPreviousSunday(DateTime.UtcNow),
					ZapId = 107,
					RefId = 107,
					Flag = 0,
					KodDok = 0,
					Linked = "0000000000",
					Placen = 0,
					NrId = 1,
					NuId = 1,
					Razlika = 0,
					DodPorez = 0,
					Porez = 0,
					Popust1Procenat = 0,
					Popust2Procenat = 0,
					Popust3Procenat = 0,
				}
			);
		}
		catch (Exception e)
		{
			const string msg = "Greska prilikom kreiranja popisa u Komercijalnom!";
			logger.LogError(e, msg);
			throw new LSCoreBadRequestException(msg);
		}
	}

	async Task<DokumentDto> CreateKomercijalnoNarudzbenicaAsync(
		TDKomercijalnoClient client,
		int magacinId
	)
	{
		try
		{
			return await client.Dokumenti.CreateAsync(
				new DokumentCreateRequest
				{
					VrDok = 33,
					MagacinId = (short)magacinId,
					MagId = (short?)magacinId,
					ZapId = 107,
					RefId = 107,
					Flag = 0,
					KodDok = 0,
					Linked = "0000000000",
					Placen = 0,
					NrId = 1,
					NuId = 1,
					Razlika = 0,
					DodPorez = 0,
					Porez = 0,
					Popust1Procenat = 0,
					Popust2Procenat = 0,
					Popust3Procenat = 0,
				}
			);
		}
		catch (Exception e)
		{
			const string msg = "Greska prilikom kreiranja popisa u Komercijalnom!";
			logger.LogError(e, msg);
			throw new LSCoreBadRequestException(msg);
		}
	}

	public async Task<bool> CreateAsync(CreatePopisiRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		if (currentUser.StoreId == null)
			throw new LSCoreBadRequestException("Korisnik nema dodeljen magacin.");
		var magacinId = currentUser.StoreId.Value;
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(magacinId);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		var komercijalnoPopis = await CreateKomercijalnoPopisAsync(client, magacinId, request.Time);
		DokumentDto? komercijalnoNarudzbenica = null;
		if (request.Type == PopisDokumentType.ZaNabavku)
			komercijalnoNarudzbenica = await CreateKomercijalnoNarudzbenicaAsync(client, magacinId);
		repository.Insert(
			new PopisDokumentEntity
			{
				MagacinId = (long)currentUser.StoreId,
				CreatedBy = currentUser.Id,
				CreatedAt = DateTime.UtcNow,
				Status = DokumentStatus.Open,
				Type = request.Type,
				Time = request.Time,
				IsActive = true,
				KomercijalnoPopisBrDok = komercijalnoPopis.BrDok,
				KomercijalnoNarudzbenicaBrDok = komercijalnoNarudzbenica?.BrDok,
			}
		);
		return true;
	}

	public PopisDetailedDto GetById(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		var entity = repository
			.GetMultiple()
			.Include(x => x.Items!.Where(z => z.IsActive))
			.FirstOrDefault(x => x.IsActive && x.Id == id);
		var dto =
			entity == null
				? throw new LSCoreNotFoundException()
				: entity.ToMapped<PopisDokumentEntity, PopisDetailedDto>();

		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);

		var komercijalnoPopis = client
			.Dokumenti.Get(
				new DokumentGetRequest { VrDok = 7, BrDok = (int)entity.KomercijalnoPopisBrDok }
			)
			.GetAwaiter()
			.GetResult();
		dto.PopisDate = komercijalnoPopis?.Datum;

		if (entity.KomercijalnoNarudzbenicaBrDok is not null)
		{
			var komercijalnoNarudzbenica = client
				.Dokumenti.Get(
					new DokumentGetRequest
					{
						VrDok = 33,
						BrDok = (int)entity.KomercijalnoNarudzbenicaBrDok,
					}
				)
				.GetAwaiter()
				.GetResult();
			dto.NarudzbenicaDate = komercijalnoNarudzbenica?.Datum;
		}

		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();
		foreach (var item in dto.Items)
		{
			var roba = komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId);
			item.Naziv = roba?.Naziv ?? string.Empty;
			item.Unit = roba?.JM ?? string.Empty;
		}

		dto.Items = dto.Items.OrderBy(x => x.Naziv).ToList();

		return dto;
	}

	public async Task<bool> StornirajPopisAsync(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisFilterStorniraj))
			throw new LSCoreForbiddenException();
		var entity = repository.GetMultiple().FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();

		// "storniram" popis u komercijalnom
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		await client.Dokumenti.SetDokumenFlag(
			new DokumentSetFlagRequest()
			{
				VrDok = 7,
				BrDok = (int)entity.KomercijalnoPopisBrDok,
				Flag = 1,
			}
		);
		await client.Stavke.DeleteAsync(
			new StavkeDeleteRequest { VrDok = 7, BrDok = (int)entity.KomercijalnoPopisBrDok }
		);
		// ===

		// "storniram" narudzbenicu u komercijalnom
		if (entity.KomercijalnoNarudzbenicaBrDok is not null)
		{
			await client.Dokumenti.SetDokumenFlag(
				new DokumentSetFlagRequest
				{
					VrDok = 33,
					BrDok = (int)entity.KomercijalnoNarudzbenicaBrDok,
					Flag = 1,
				}
			);
			await client.Stavke.DeleteAsync(
				new StavkeDeleteRequest
				{
					VrDok = 33,
					BrDok = (int)entity.KomercijalnoNarudzbenicaBrDok,
				}
			);
		}
		// ===

		entity.Status = DokumentStatus.Canceled;
		repository.Update(entity);
		return true;
	}

	public async Task SetStatusAsync(PopisSetStatusRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		// Permission checks based on desired status
		if (request.Status == DokumentStatus.Closed)
		{
			if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisFilterLock))
				throw new LSCoreForbiddenException();
		}
		else if (request.Status == DokumentStatus.Open)
		{
			if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisFilterUnlock))
				throw new LSCoreForbiddenException();
		}

		var entity = repository.GetMultiple().FirstOrDefault(x => x.IsActive && x.Id == request.Id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status == DokumentStatus.Canceled)
			throw new LSCoreBadRequestException("Stornirani dokument ne moze menjati status.");

		// Zakljucaj u komercijalnom
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		await client.Dokumenti.SetDokumenFlag(
			new DokumentSetFlagRequest()
			{
				VrDok = 7,
				BrDok = (int)entity.KomercijalnoPopisBrDok,
				Flag = (short)(request.Status == DokumentStatus.Open ? 0 : 1),
			}
		);
		if (entity.KomercijalnoNarudzbenicaBrDok is not null)
			await client.Dokumenti.SetDokumenFlag(
				new DokumentSetFlagRequest()
				{
					VrDok = 33,
					BrDok = (int)entity.KomercijalnoNarudzbenicaBrDok,
					Flag = (short)(request.Status == DokumentStatus.Open ? 0 : 1),
				}
			);
		// ===

		entity.Status = request.Status;
		repository.Update(entity);
	}

	async Task UpdatePopisanaKolicinaInKomercijalnoAsync(
		TDKomercijalnoClient client,
		int magacinId,
		int komercijalnoBrDok,
		int robaId,
		double popisanaKolicina,
		bool forceZeroKolicina = false
	)
	{
		// Prvo skidam stavku iz dokumenta kako ne bi dodao kao duplikat
		var dokument = await client.Dokumenti.Get(
			new DokumentGetRequest { VrDok = 7, BrDok = komercijalnoBrDok }
		);
		var stavkaUDokumentu = dokument.Stavke?.FirstOrDefault(x => x.RobaId == robaId);
		if (stavkaUDokumentu is not null)
		{
			await client.Stavke.DeleteAsync(
				new StavkeDeleteRequest
				{
					VrDok = 7,
					BrDok = komercijalnoBrDok,
					RobaId = stavkaUDokumentu.RobaId,
				}
			);
		}
		if (forceZeroKolicina)
		{
			await client.Stavke.CreateAsync(
				new StavkaCreateRequest()
				{
					BrDok = komercijalnoBrDok,
					VrDok = 7,
					Kolicina = 0,
					RobaId = robaId,
				}
			);
			return;
		}
		// Hvatam sve stavke do dana kada trebam (zavistno od Time popisa)
		var stavkeZaOvuRobaIdNakonDokumentaPopisaUKomercijalnom =
			await client.Stavke.GetMultipleByRobaIdAsync(
				new StavkeGetMultipleByRobaId
				{
					RobaId = robaId,
					From = dokument.Datum.AddMinutes(1),
					MagacinId = magacinId,
				}
			);

		List<int> izlazniVrDok = [15, 19, 17, 23, 13, 14, 27, 25, 28];
		List<int> ulazniVrDok = [0, 11, 18, 16, 22, 1, 2, 26, 29, 30];

		stavkeZaOvuRobaIdNakonDokumentaPopisaUKomercijalnom.RemoveAll(x =>
			(!izlazniVrDok.Contains(x.VrDok) && !ulazniVrDok.Contains(x.VrDok)) || x.VrDok == 18 // nemam pojma zasto 18 ali postoji u starom
		);

		var summaryKolicineZaOvuRobaIdNakonDokumentaPopisaUKomercijalnom =
			stavkeZaOvuRobaIdNakonDokumentaPopisaUKomercijalnom.Sum(x =>
				x.Kolicina * (ulazniVrDok.Contains(x.VrDok) ? 1 : -1)
			);

		var popisanaKolicinaNaDanDokumentaPopisaKomercijalnogPoslovanja =
			popisanaKolicina - summaryKolicineZaOvuRobaIdNakonDokumentaPopisaUKomercijalnom;

		await client.Stavke.CreateAsync(
			new StavkaCreateRequest()
			{
				BrDok = komercijalnoBrDok,
				VrDok = 7,
				Kolicina = popisanaKolicinaNaDanDokumentaPopisaKomercijalnogPoslovanja,
				RobaId = robaId,
			}
		);
	}

	async Task UpdateNarucenaKolicinaInKomercijalnoAsync(
		TDKomercijalnoClient client,
		int komercijalnoNarudzbenicaBrDok,
		int robaId,
		double narucenaKolicina
	)
	{
		// Prvo skidam stavku iz dokumenta kako ne bi dodao kao duplikat
		var dokument = await client.Dokumenti.Get(
			new DokumentGetRequest { VrDok = 33, BrDok = komercijalnoNarudzbenicaBrDok }
		);
		var stavkaUDokumentu = dokument.Stavke?.FirstOrDefault(x => x.RobaId == robaId);
		if (stavkaUDokumentu is not null)
		{
			await client.Stavke.DeleteAsync(
				new StavkeDeleteRequest
				{
					VrDok = 33,
					BrDok = komercijalnoNarudzbenicaBrDok,
					RobaId = stavkaUDokumentu.RobaId,
				}
			);
		}
		await client.Stavke.CreateAsync(
			new StavkaCreateRequest()
			{
				BrDok = komercijalnoNarudzbenicaBrDok,
				VrDok = 33,
				Kolicina = narucenaKolicina,
				RobaId = robaId,
			}
		);
	}

	public async Task<PopisItemDto> AddItemToPopisAsync(PopisAddItemRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		var entity = repository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == request.PopisId);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status != DokumentStatus.Open)
			throw new LSCoreBadRequestException("Moguce je dodavati stavke samo otvorenom popisu.");
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		await UpdatePopisanaKolicinaInKomercijalnoAsync(
			client,
			(int)entity.MagacinId,
			(int)entity.KomercijalnoPopisBrDok,
			(int)request.RobaId,
			request.Kolicina,
			request.ForceZeroKolicina
		);
		if (entity.KomercijalnoNarudzbenicaBrDok is not null)
			await UpdateNarucenaKolicinaInKomercijalnoAsync(
				client,
				(int)entity.KomercijalnoNarudzbenicaBrDok,
				(int)request.RobaId,
				0
			);
		entity.Items ??= [];
		// Add it to TDOffice DB
		entity.Items.Add(
			new PopisItemEntity
			{
				PopisanaKolicina = request.Kolicina,
				PopisDokumentId = request.PopisId,
				RobaId = request.RobaId,
				CreatedAt = DateTime.UtcNow,
				NarucenaKolicina = 0,
				IsActive = true,
				CreatedBy = currentUser.Id,
			}
		);
		repository.Update(entity);
		// ===
		var addedItem = entity.Items.Last();
		// Load item dto with komercijalno naziv
		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();
		if (komercijalnoRoba.All(x => x.RobaId != addedItem.RobaId))
			throw new LSCoreBadRequestException("Roba sa zadatim Id-em ne postoji u sistemu.");
		var dto = addedItem.ToMapped<PopisItemEntity, PopisItemDto>();
		var robaDetails = komercijalnoRoba.First(x => x.RobaId == addedItem.RobaId);
		dto.Naziv = robaDetails.Naziv;
		// ===
		return dto;
	}

	public async Task RemoveItemFromPopisAsync(long id, long itemId)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		var entity = repository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status != DokumentStatus.Open)
			throw new LSCoreBadRequestException(
				"Moguce je ukloniti stavke samo iz otvorenog popisa."
			);

		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		var item = entity.Items?.FirstOrDefault(x => x.IsActive && x.Id == itemId);
		if (item == null)
			throw new LSCoreNotFoundException();
		client
			.Stavke.DeleteAsync(
				new StavkeDeleteRequest()
				{
					BrDok = (int)entity.KomercijalnoPopisBrDok,
					VrDok = 7,
					RobaId = (int)item!.RobaId,
				}
			)
			.GetAwaiter()
			.GetResult();
		entity.Items?.Remove(item);
		repository.Update(entity);
	}

	public async Task UpdatePopisanaKolicinaAsync(long id, long itemId, double popisanaKolicina)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		var entity = repository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status != DokumentStatus.Open)
			throw new LSCoreBadRequestException(
				"Moguce je menjati stavke samo u otvorenom popisu."
			);
		var item = entity.Items?.FirstOrDefault(x => x.IsActive && x.Id == itemId);
		if (item == null)
			throw new LSCoreNotFoundException();

		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);

		await UpdatePopisanaKolicinaInKomercijalnoAsync(
			client,
			(int)entity.MagacinId,
			(int)entity.KomercijalnoPopisBrDok,
			(int)item.RobaId,
			popisanaKolicina
		);

		item.PopisanaKolicina = popisanaKolicina;
		repository.Update(entity);
	}

	public async Task UpdateNarucenaKolicinaAsync(long id, long itemId, double narucenaKolicina)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();

		var entity = repository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status != DokumentStatus.Open)
			throw new LSCoreBadRequestException(
				"Moguce je menjati stavke samo u otvorenom popisu."
			);
		if (entity.KomercijalnoNarudzbenicaBrDok is null)
			throw new LSCoreBadRequestException("Ovaj popis nema povezanu narudzbenicu!");
		var item = entity.Items?.FirstOrDefault(x => x.IsActive && x.Id == itemId);
		if (item == null)
			throw new LSCoreNotFoundException();

		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);

		await UpdateNarucenaKolicinaInKomercijalnoAsync(
			client,
			(int)entity.KomercijalnoNarudzbenicaBrDok,
			(int)item.RobaId,
			narucenaKolicina
		);
		item.NarucenaKolicina = narucenaKolicina;
		repository.Update(entity);
	}

	public async Task MasovnoDodavanjeStavkiAsync(
		long id,
		PopisMasovnoDodavanjeStavkiRequest request
	)
	{
		switch (request.ActionType)
		{
			case PopisMasovnoDodavanjeStavkiActionType.StavkePocetnogStanajSaKolicinom:
				await MasovnoDodavanjeStavkiPopisaSaKolicinamaAsync(id);
				return;
			case PopisMasovnoDodavanjeStavkiActionType.StavkeSaPrometom:
				await MasovnoDodavanjeStavkiPrometaAsync(id);
				return;
		}
		throw new InvalidOperationException();
	}

	Task MasovnoDodavanjeStavkiPrometaAsync(long id)
	{
		return Task.CompletedTask;
	}

	async Task MasovnoDodavanjeStavkiPopisaSaKolicinamaAsync(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		var entity = repository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		if (entity.Status != DokumentStatus.Open)
			throw new LSCoreBadRequestException(
				"Moguce je menjati stavke samo u otvorenom popisu."
			);

		var magacinid = (int)entity.MagacinId;

		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(magacinid);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);

		var dokumentiPopisa = await client.Dokumenti.GetMultipleAsync(
			new DokumentGetMultipleRequest() { VrDok = [0], MagacinId = magacinid }
		);
		var robaSaKolicinom = new HashSet<int>();
		foreach (var popis in dokumentiPopisa)
		{
			if (popis.Stavke is null || popis.Stavke.Count == 0)
				continue;
			foreach (var stavka in popis.Stavke)
			{
				if (stavka.Kolicina <= 0)
					continue;
				robaSaKolicinom.Add(stavka.RobaId);
			}
		}

		var robaIdsToAdd = robaSaKolicinom
			.Where(robaId => !entity.Items!.Any(x => x.RobaId == robaId))
			.ToList();

		if (robaIdsToAdd.Count == 0)
			return;

		var optimizedRequestPopis = new StavkeCreateOptimizedRequest();
		var optimizedRequestNarudzbenica = new StavkeCreateOptimizedRequest();

		entity.Items ??= [];
		foreach (var robaId in robaIdsToAdd)
		{
			optimizedRequestPopis.Stavke.Add(
				new StavkaCreateRequest
				{
					BrDok = (int)entity.KomercijalnoPopisBrDok,
					VrDok = 7,
					Kolicina = 0,
					RobaId = robaId,
				}
			);

			if (entity.KomercijalnoNarudzbenicaBrDok is not null)
			{
				optimizedRequestNarudzbenica.Stavke.Add(
					new StavkaCreateRequest
					{
						BrDok = (int)entity.KomercijalnoNarudzbenicaBrDok,
						VrDok = 33,
						Kolicina = 0,
						RobaId = robaId,
					}
				);
			}

			entity.Items.Add(
				new PopisItemEntity
				{
					PopisanaKolicina = 99999,
					PopisDokumentId = entity.Id,
					RobaId = robaId,
					CreatedAt = DateTime.UtcNow,
					NarucenaKolicina = 0,
					IsActive = true,
					CreatedBy = currentUser.Id,
				}
			);
		}

		await client.Stavke.CreateOptimizedAsync(optimizedRequestPopis);
		if (optimizedRequestNarudzbenica.Stavke.Count > 0)
			await client.Stavke.CreateOptimizedAsync(optimizedRequestNarudzbenica);
		repository.Update(entity);
	}
}
