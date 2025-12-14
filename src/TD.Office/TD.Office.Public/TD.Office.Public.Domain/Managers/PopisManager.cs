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

	public async Task<bool> CreateAsync(CreatePopisiRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		if (currentUser.StoreId == null)
			throw new LSCoreBadRequestException("Korisnik nema dodeljen magacin.");
		// Create Komercijalno Popis
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			currentUser.StoreId.Value
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		DokumentDto? komercijalnoDokumentDto = null;
		try
		{
			komercijalnoDokumentDto = await client.Dokumenti.CreateAsync(
				new DokumentCreateRequest
				{
					VrDok = 7,
					MagacinId = (short)currentUser.StoreId.Value,
					MagId = (short)currentUser.StoreId.Value,
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
		// ===

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
				KomercijalnoBrDok = komercijalnoDokumentDto.BrDok,
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
		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();
		foreach (var item in dto.Items)
			item.Naziv =
				komercijalnoRoba.FirstOrDefault(x => x.RobaId == item.RobaId)?.Naziv
				?? string.Empty;
		return dto;
	}

	public bool StornirajPopis(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisFilterStorniraj))
			throw new LSCoreForbiddenException();

		var entity = repository.GetMultiple().FirstOrDefault(x => x.IsActive && x.Id == id);
		if (entity == null)
			throw new LSCoreNotFoundException();
		entity.Status = DokumentStatus.Canceled;
		repository.Update(entity);
		return true;
	}

	public void SetStatus(PopisSetStatusRequest request)
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
		entity.Status = request.Status;
		repository.Update(entity);
	}

	public async Task<PopisItemDto> AddItemToPopis(PopisAddItemRequest request)
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
		// Add it to Komercijalno
		var komercijalnoMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(
			(int)entity.MagacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(configurationRoot["DEPLOY_ENV"]!),
			komercijalnoMagacinFirma.ApiFirma
		);
		// Prvo skidam stavku iz dokumenta kako ne bi dodao kao duplikat
		var dokument = await client.Dokumenti.Get(
			new DokumentGetRequest { VrDok = 7, BrDok = (int)entity.KomercijalnoBrDok }
		);
		var stavkaUDokumentu = dokument.Stavke?.FirstOrDefault(x => x.RobaId == request.RobaId);
		if (stavkaUDokumentu is not null)
		{
			await client.Stavke.DeleteAsync(
				new StavkeDeleteRequest
				{
					VrDok = 7,
					BrDok = (int)entity.KomercijalnoBrDok,
					RobaId = stavkaUDokumentu.RobaId,
				}
			);
		}
		// Hvatam sve stavke do dana kada trebam (zavistno od Time popisa)
		// ===
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

	public void RemoveItemFromPopis(long id, long itemId)
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
		var item = entity.Items?.FirstOrDefault(x => x.IsActive && x.Id == itemId);
		if (item == null)
			throw new LSCoreNotFoundException();
		item.IsActive = false;
		repository.Update(entity);
	}

	public void UpdatePopisanaKolicina(long id, long itemId, double popisanaKolicina)
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
		item.PopisanaKolicina = popisanaKolicina;
		repository.Update(entity);
	}

	public void UpdateNarucenaKolicina(long id, long itemId, double narucenaKolicina)
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
		item.NarucenaKolicina = narucenaKolicina;
		repository.Update(entity);
	}
}
