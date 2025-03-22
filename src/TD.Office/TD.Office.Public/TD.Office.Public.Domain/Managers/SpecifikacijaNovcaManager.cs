using System.ComponentModel;
using LSCore.Auth.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Domain.Managers;

public class SpecifikacijaNovcaManager(
	ILogger<SpecifikacijaNovcaManager> logger,
	OfficeDbContext dbContext,
	LSCoreAuthContextEntity<string> contextEntity,
	ISpecifikacijaNovcaRepository specifikacijaNovcaRepository,
	IUserRepository userRepository,
	ITDKomercijalnoApiManager komercijalnoApiManager
) : ISpecifikacijaNovcaManager
{
	/// <summary>
	/// Returns current specifikacija novca for current user.
	/// If it doesn't exist, it will create new one.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="LSCoreBadRequestException"></exception>
	public async Task<GetSpecifikacijaNovcaDto> GetCurrentAsync()
	{
		var user = userRepository.GetCurrentUser();

		if (user.StoreId == null)
			throw new LSCoreBadRequestException(
				SpecifikacijaNovcaValidationCodes.SNVC_001.GetDescription()!
			);

		var entity = specifikacijaNovcaRepository.GetCurrentOrDefaultByMagacinId((int)user.StoreId);
		if (entity == null)
		{
			entity = new SpecifikacijaNovcaEntity
			{
				MagacinId = (int)user.StoreId,
				Datum = DateTime.UtcNow,
				IsActive = true,
				CreatedBy = user.Id
			};
			specifikacijaNovcaRepository.Insert(entity);
		}

		var response = entity.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();
		response.Racunar = await CalculateRacunarDataAsync((int)user.StoreId);
		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetNextAsync(
		GetNextSpecifikacijaNovcaRequest request
	)
	{
		var user = userRepository.GetCurrentUser();

		var response = specifikacijaNovcaRepository
			.GetNext(request.RelativeToId, request.FixMagacin, ListSortDirection.Ascending)
			.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

		if (
			response.Id != user.StoreId
			&& !user.Permissions.Any(x =>
				x.IsActive && x.Permission == Permission.SpecifikacijaNovcaSviMagacini
			)
		)
			throw new LSCoreForbiddenException();

		response.Racunar = await CalculateRacunarDataAsync((int)response.Id);

		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetPrevAsync(
		GetPrevSpecifikacijaNovcaRequest request
	)
	{
		var user = userRepository.GetCurrentUser();

		var response = specifikacijaNovcaRepository
			.GetNext(request.RelativeToId, request.FixMagacin, ListSortDirection.Descending)
			.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

		if (
			response.Id != user.StoreId
			&& !user.Permissions.Any(x =>
				x.IsActive && x.Permission == Permission.SpecifikacijaNovcaSviMagacini
			)
		)
			throw new LSCoreForbiddenException();

		response.Racunar = await CalculateRacunarDataAsync((int)response.Id);

		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetSingleAsync(
		GetSingleSpecifikacijaNovcaRequest request
	)
	{
		var user = userRepository.GetCurrentUser();
		if (
			user.StoreId != request.Id
			&& !user.Permissions.Any(x =>
				x.IsActive && x.Permission == Permission.SpecifikacijaNovcaSviMagacini
			)
		)
			throw new LSCoreForbiddenException();

		var response = specifikacijaNovcaRepository
			.Get(request.Id)
			.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

		response.Racunar = await CalculateRacunarDataAsync((int)request.Id);

		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetSpecifikacijaByDate(
		GetSpecifikacijaByDateRequest request
	)
	{
		var user = userRepository.GetCurrentUser();

		if (
			(
				!user.Permissions.Any(x =>
					x.Permission == Permission.SpecifikacijaNovcaSviMagacini && x.IsActive
				)
				|| request.MagacinId != user.StoreId
			)
			|| (
				request.Date.Date.AddDays(7) > DateTime.UtcNow.Date
				&& !user.Permissions.Any(x =>
					(
						x.Permission == Permission.SpecifikacijaNovcaPrethodnih7Dana
						|| x.Permission == Permission.SpecifikacijaNovcaSviDatumi
					) && x.IsActive
				)
			)
		)
			throw new LSCoreForbiddenException();

		var response = specifikacijaNovcaRepository
			.GetByDate(request.Date, request.MagacinId)
			.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

		response.Racunar = await CalculateRacunarDataAsync((int)response.MagacinId);

		return response;
	}

	public void Save(SaveSpecifikacijaNovcaRequest request)
	{
		request.Validate();
		var entity = specifikacijaNovcaRepository.Get((long)request.Id!);
		var user = userRepository.GetCurrentUser();
		if (
			entity.MagacinId != user.StoreId
			&& !user.Permissions.Any(permission =>
				permission.IsActive && permission.Permission == Permission.SpecifikacijaNovcaSave
			)
		)
			throw new LSCoreForbiddenException();

		entity.InjectFrom(request);
		specifikacijaNovcaRepository.Update(entity);
	}

	private async Task<SpecifikacijaNovcaRacunarDto> CalculateRacunarDataAsync(int storeId)
	{
		var racuniIPovratnice = await komercijalnoApiManager.GetMultipleDokumentAsync(
			new DokumentGetMultipleRequest()
			{
				VrDok = [15, 22],
				DatumOd = DateTime.UtcNow.Date,
				DatumDo = DateTime.UtcNow.Date.AddDays(1),
				MagacinId = storeId,
				Flag = 1
			}
		);

		return new SpecifikacijaNovcaRacunarDto
		{
			GotovinskiRacuni = racuniIPovratnice
				.Where(x => x is { NuId: (short)NacinUplate.Gotovina, VrDok: 15 })
				.Sum(x => x.Potrazuje),
			Kartice = racuniIPovratnice
				.Where(x => x is { NuId: (short)NacinUplate.Kartica, VrDok: 15 })
				.Sum(x => x.Potrazuje),
			VirmanskiRacuni = racuniIPovratnice
				.Where(x => x is { NuId: (short)NacinUplate.Virman, VrDok: 15 })
				.Sum(x => x.Potrazuje),
			GotovinskePovratnice = racuniIPovratnice
				.Where(x => x is { NuId: (short)NacinUplate.Gotovina, VrDok: 22 })
				.Sum(x => x.Potrazuje),
			VirmanskePovratnice = racuniIPovratnice
				.Where(x => x is { NuId: (short)NacinUplate.Virman, VrDok: 22 })
				.Sum(x => x.Potrazuje),
			OstalePovratnice = racuniIPovratnice
				.Where(x =>
					x.NuId != (short)NacinUplate.Gotovina
					&& x.NuId != (short)NacinUplate.Virman
					&& x.VrDok == 22
				)
				.Sum(x => x.Potrazuje)
		};
	}
}
