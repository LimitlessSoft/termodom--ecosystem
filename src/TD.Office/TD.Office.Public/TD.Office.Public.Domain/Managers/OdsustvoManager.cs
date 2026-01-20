using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Odsustvo;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Odsustvo;

namespace TD.Office.Public.Domain.Managers;

public class OdsustvoManager(
	IOdsustvoRepository odsustvoRepository,
	IUserRepository userRepository,
	OfficeDbContext dbContext
) : IOdsustvoManager
{
	public List<OdsustvoCalendarDto> GetCalendar(GetOdsustvoCalendarRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasEditAllPermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiEditAll);

		var startDate = new DateTime(request.Year, request.Month, 1);
		var endDate = startDate.AddMonths(1).AddDays(-1);

		var userId = hasEditAllPermission ? (long?)null : currentUser.Id;
		var entities = odsustvoRepository.GetByDateRange(startDate, endDate, userId);

		return entities.ToMappedList<OdsustvoEntity, OdsustvoCalendarDto>();
	}

	public OdsustvoDto GetSingle(LSCoreIdRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var entity = dbContext.Odsustva
			.Include(x => x.TipOdsustva)
			.Include(x => x.OdobrenoByUser)
			.FirstOrDefault(x => x.Id == request.Id && x.IsActive);

		if (entity == null)
			throw new LSCoreNotFoundException();

		var hasEditAllPermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiEditAll);
		if (entity.UserId != currentUser.Id && !hasEditAllPermission)
			throw new LSCoreForbiddenException();

		return entity.ToMapped<OdsustvoEntity, OdsustvoDto>();
	}

	public void Save(SaveOdsustvoRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasEditAllPermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiEditAll);
		var hasWritePermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiWrite);

		request.Validate();

		OdsustvoEntity entity;

		if (request.Id.HasValue)
		{
			entity = odsustvoRepository.Get(request.Id.Value);

			// Only users with EditAll permission can modify existing odsustva
			// Once created, even the owner cannot edit their own odsustvo
			if (!hasEditAllPermission)
				throw new LSCoreForbiddenException("Samo korisnici sa pravom izmene mogu menjati odsustva");
		}
		else
		{
			if (!hasWritePermission && !hasEditAllPermission)
				throw new LSCoreForbiddenException();

			entity = new OdsustvoEntity();
		}

		entity.TipOdsustvaId = request.TipOdsustvaId;
		entity.DatumOd = request.DatumOd;
		entity.DatumDo = request.DatumDo;
		entity.Komentar = request.Komentar;

		if (request.UserId.HasValue && hasEditAllPermission)
		{
			entity.UserId = request.UserId.Value;
		}
		else if (!request.Id.HasValue)
		{
			entity.UserId = currentUser.Id;
		}

		if (request.Id.HasValue)
		{
			entity.Id = request.Id.Value;
			odsustvoRepository.Update(entity);
		}
		else
		{
			odsustvoRepository.Insert(entity);
		}
	}

	public void Delete(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasEditAllPermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiEditAll);

		var entity = odsustvoRepository.Get(id);

		if (entity.UserId != currentUser.Id && !hasEditAllPermission)
			throw new LSCoreForbiddenException();

		odsustvoRepository.SoftDelete(id);
	}

	public void Approve(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		var hasApprovePermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiApprove);

		if (!hasApprovePermission)
			throw new LSCoreForbiddenException();

		var entity = odsustvoRepository.Get(id);

		if (entity.Status == OdsustvoStatus.Odobreno)
			throw new LSCoreBadRequestException("Odsustvo je već odobreno");

		entity.Status = OdsustvoStatus.Odobreno;
		entity.OdobrenoAt = DateTime.UtcNow;
		entity.OdobrenoBy = currentUser.Id;

		odsustvoRepository.Update(entity);
	}

	public void UpdateRealizovano(long id, UpdateRealizovanoRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		var entity = odsustvoRepository.Get(id);

		// Realizovano can only be changed after odsustvo is approved
		if (entity.Status != OdsustvoStatus.Odobreno)
			throw new LSCoreBadRequestException("Realizovano se može menjati samo nakon odobrenja odsustva");

		var hasApprovePermission = userRepository.HasPermission(currentUser.Id, Permission.KalendarAktivnostiApprove);
		var isOwner = entity.UserId == currentUser.Id;

		if (request.RealizovanoKorisnik.HasValue)
		{
			if (!isOwner && !hasApprovePermission)
				throw new LSCoreForbiddenException();
			entity.RealizovanoKorisnik = request.RealizovanoKorisnik.Value;
		}

		if (request.RealizovanoOdobravac.HasValue)
		{
			if (!hasApprovePermission)
				throw new LSCoreForbiddenException();
			entity.RealizovanoOdobravac = request.RealizovanoOdobravac.Value;
		}

		odsustvoRepository.Update(entity);
	}
}
