using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.TipOdsustva;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.TipOdsustva;

namespace TD.Office.Public.Domain.Managers;

public class TipOdsustvaManager(
	ITipOdsustvaRepository tipOdsustvaRepository,
	IUserRepository userRepository
) : ITipOdsustvaManager
{
	public List<TipOdsustvaDto> GetMultiple() =>
		tipOdsustvaRepository
			.GetMultiple()
			.OrderBy(x => x.Redosled)
			.ToMappedList<TipOdsustvaEntity, TipOdsustvaDto>();

	public void Save(SaveTipOdsustvaRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.TipOdsustvaWrite))
			throw new LSCoreForbiddenException();

		request.Validate();

		var entity = request.Id.HasValue
			? tipOdsustvaRepository.Get(request.Id.Value)
			: new TipOdsustvaEntity();

		entity.InjectFrom(request);

		if (request.Id.HasValue)
		{
			entity.Id = request.Id.Value;
			tipOdsustvaRepository.Update(entity);
		}
		else
		{
			tipOdsustvaRepository.Insert(entity);
		}
	}

	public void Delete(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.TipOdsustvaWrite))
			throw new LSCoreForbiddenException();

		tipOdsustvaRepository.SoftDelete(id);
	}
}
