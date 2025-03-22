using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class RobaManager(KomercijalnoDbContext dbContext, IRobaRepository robaRepository)
	: IRobaManager
{
	public Roba Create(RobaCreateRequest request)
	{
		request.Validate();

		var roba = new Roba();
		roba.InjectFrom(request);
		robaRepository.Insert(roba);
		return roba;
	}

	public RobaDto Get(LSCoreIdRequest request)
	{
		var roba = dbContext.Roba.Include(x => x.Tarifa).FirstOrDefault(x => x.Id == request.Id);

		if (roba == null)
			throw new LSCoreNotFoundException();

		return roba.ToRobaDto();
	}

	public List<RobaDto> GetMultiple(RobaGetMultipleRequest request) =>
		dbContext
			.Roba.Include(x => x.Tarifa)
			.Where(x => (!request.Vrsta.HasValue || x.Vrsta == request.Vrsta))
			.ToList()
			.ToRobaDtoList();
}
