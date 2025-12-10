using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.Popisi;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Domain.Managers;

public class PopisManager(
	IPopisRepository repository,
	IUserRepository userRepository,
	TDKomercijalnoClient defaultKomercijalnoClient
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

	public bool Create(CreatePopisiRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			throw new LSCoreForbiddenException();
		if (currentUser.StoreId == null)
			throw new LSCoreBadRequestException("Korisnik nema dodeljen magacin.");
		repository.Insert(
			new PopisDokumentEntity
			{
				MagacinId = (long)currentUser.StoreId,
				CreatedBy = currentUser.Id,
				CreatedAt = DateTime.UtcNow,
				Status = DokumentStatus.Open,
				Type = request.Type,
				IsActive = true,
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

	public PopisItemDto AddItemToPopis(PopisAddItemRequest request)
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
		entity.Items ??= [];
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
		var addedItem = entity.Items.Last();

		var komercijalnoRoba = defaultKomercijalnoClient
			.Roba.GetMultipleAsync(new RobaGetMultipleRequest())
			.GetAwaiter()
			.GetResult();
		if (komercijalnoRoba.All(x => x.RobaId != addedItem.RobaId))
			throw new LSCoreBadRequestException("Roba sa zadatim Id-em ne postoji u sistemu.");
		var dto = addedItem.ToMapped<PopisItemEntity, PopisItemDto>();
		var robaDetails = komercijalnoRoba.First(x => x.RobaId == addedItem.RobaId);
		dto.Naziv = robaDetails.Naziv;
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
