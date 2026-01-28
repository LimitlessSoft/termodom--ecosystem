using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Ticket;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Ticket;

namespace TD.Office.Public.Domain.Managers;

public class TicketManager(
	ITicketRepository ticketRepository,
	IUserRepository userRepository,
	OfficeDbContext dbContext
) : ITicketManager
{
	public LSCoreSortedAndPagedResponse<TicketListDto> GetMultiple(GetMultipleTicketsRequest request)
	{
		return ticketRepository
			.GetFiltered(
				request.Types,
				request.Statuses,
				request.Priorities,
				request.SubmittedByUserId)
			.ToSortedAndPagedResponse<TicketEntity, TicketsSortColumnCodes.Tickets, TicketListDto>(
				request,
				TicketsSortColumnCodes.TicketsSortRules,
				x => x.ToMapped<TicketEntity, TicketListDto>());
	}

	public TicketDto GetSingle(LSCoreIdRequest request)
	{
		var entity = dbContext.Tickets
			.Include(x => x.SubmittedByUser)
			.Include(x => x.ResolvedByUser)
			.FirstOrDefault(x => x.Id == request.Id && x.IsActive);

		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity.ToMapped<TicketEntity, TicketDto>();
	}

	public List<TicketDto> GetRecentlySolved()
	{
		var entities = ticketRepository.GetRecentlySolved(5);
		return entities.ToMappedList<TicketEntity, TicketDto>();
	}

	public List<TicketDto> GetInProgress()
	{
		var entities = ticketRepository.GetInProgress(5);
		return entities.ToMappedList<TicketEntity, TicketDto>();
	}

	public void Save(SaveTicketRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasCreateBugPermission = userRepository.HasPermission(currentUser.Id, Permission.TicketsCreateBug);
		var hasCreateFeaturePermission = userRepository.HasPermission(currentUser.Id, Permission.TicketsCreateFeature);

		request.Validate();

		TicketEntity entity;

		if (request.Id.HasValue)
		{
			entity = ticketRepository.Get(request.Id.Value);

			// Only the author can edit their own ticket
			if (entity.SubmittedByUserId != currentUser.Id)
				throw new LSCoreForbiddenException("Možete menjati samo svoje tikete");

			// Cannot edit resolved or rejected tickets
			if (entity.Status == TicketStatus.Resolved || entity.Status == TicketStatus.Rejected)
				throw new LSCoreBadRequestException("Ne možete menjati zatvorene tikete");
		}
		else
		{
			// Check permission based on ticket type
			if (request.Type == TicketType.Bug && !hasCreateBugPermission)
				throw new LSCoreForbiddenException("Nemate dozvolu za kreiranje bug prijava");

			if (request.Type == TicketType.Feature && !hasCreateFeaturePermission)
				throw new LSCoreForbiddenException("Nemate dozvolu za kreiranje zahteva za funkcionalnost");

			entity = new TicketEntity
			{
				SubmittedByUserId = currentUser.Id,
				Status = TicketStatus.New,
				Priority = TicketPriority.Medium
			};
		}

		entity.Title = request.Title;
		entity.Description = request.Description;
		entity.Type = request.Type;

		if (request.Id.HasValue)
		{
			ticketRepository.Update(entity);
		}
		else
		{
			ticketRepository.Insert(entity);
		}
	}

	public void Delete(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		var entity = ticketRepository.Get(id);

		// Only the author can delete their own ticket
		if (entity.SubmittedByUserId != currentUser.Id)
			throw new LSCoreForbiddenException("Možete brisati samo svoje tikete");

		ticketRepository.SoftDelete(id);
	}

	public void UpdatePriority(long id, UpdateTicketPriorityRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasManagePriorityPermission = userRepository.HasPermission(currentUser.Id, Permission.TicketsManagePriority);

		if (!hasManagePriorityPermission)
			throw new LSCoreForbiddenException("Nemate dozvolu za upravljanje prioritetom");

		var entity = ticketRepository.Get(id);
		entity.Priority = request.Priority;

		ticketRepository.Update(entity);
	}

	public void UpdateStatus(long id, UpdateTicketStatusRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasManageStatusPermission = userRepository.HasPermission(currentUser.Id, Permission.TicketsManageStatus);

		if (!hasManageStatusPermission)
			throw new LSCoreForbiddenException("Nemate dozvolu za upravljanje statusom");

		var entity = ticketRepository.Get(id);
		entity.Status = request.Status;

		if (request.Status == TicketStatus.Resolved || request.Status == TicketStatus.Rejected)
		{
			entity.ResolvedAt = DateTime.UtcNow;
			entity.ResolvedByUserId = currentUser.Id;
			entity.ResolutionNotes = request.ResolutionNotes;
		}
		else
		{
			entity.ResolvedAt = null;
			entity.ResolvedByUserId = null;
			entity.ResolutionNotes = null;
		}

		ticketRepository.Update(entity);
	}

	public void UpdateDeveloperNotes(long id, UpdateDeveloperNotesRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasDeveloperNotesPermission = userRepository.HasPermission(currentUser.Id, Permission.TicketsDeveloperNotes);

		if (!hasDeveloperNotesPermission)
			throw new LSCoreForbiddenException("Nemate dozvolu za developer napomene");

		var entity = ticketRepository.Get(id);
		entity.DeveloperNotes = request.DeveloperNotes;

		ticketRepository.Update(entity);
	}
}
