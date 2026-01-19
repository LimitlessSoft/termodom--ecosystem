using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.TipKorisnika;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.TipKorisnika;

namespace TD.Office.Public.Domain.Managers;

public class TipKorisnikaManager(
	ITipKorisnikaRepository tipKorisnikaRepository,
	IUserRepository userRepository
) : ITipKorisnikaManager
{
	public List<TipKorisnikaDto> GetMultiple() =>
		tipKorisnikaRepository
			.GetMultiple()
			.OrderBy(x => x.Naziv)
			.ToMappedList<TipKorisnikaEntity, TipKorisnikaDto>();

	public void Save(SaveTipKorisnikaRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.TipKorisnikaWrite))
			throw new LSCoreForbiddenException();

		request.Validate();

		var entity = request.Id.HasValue
			? tipKorisnikaRepository.Get(request.Id.Value)
			: new TipKorisnikaEntity();

		entity.InjectFrom(request);

		if (request.Id.HasValue)
		{
			entity.Id = request.Id.Value;
			tipKorisnikaRepository.Update(entity);
		}
		else
		{
			tipKorisnikaRepository.Insert(entity);
		}
	}

	public void Delete(long id)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (!userRepository.HasPermission(currentUser.Id, Permission.TipKorisnikaWrite))
			throw new LSCoreForbiddenException();

		tipKorisnikaRepository.SoftDelete(id);
	}
}
