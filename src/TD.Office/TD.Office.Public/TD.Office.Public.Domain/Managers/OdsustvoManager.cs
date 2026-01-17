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

			if (entity.UserId != currentUser.Id && !hasEditAllPermission)
				throw new LSCoreForbiddenException();
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
}
