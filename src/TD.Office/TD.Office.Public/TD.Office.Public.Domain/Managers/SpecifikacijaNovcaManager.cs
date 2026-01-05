using System.ComponentModel;
using LSCore.Auth.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;
using Constants = TD.Common.Environments.Constants;

namespace TD.Office.Public.Domain.Managers;

public class SpecifikacijaNovcaManager(
	ILogger<SpecifikacijaNovcaManager> logger,
	OfficeDbContext dbContext,
	IConfigurationRoot configurationRoot,
	IKomercijalnoMagacinFirmaRepository komercijalnoMagacinFirmaRepository,
	LSCoreAuthContextEntity<string> contextEntity,
	ISpecifikacijaNovcaRepository specifikacijaNovcaRepository,
	IUserRepository userRepository,
	ITDKomercijalnoClientFactory komercijalnoClientFactory
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
				CreatedBy = user.Id,
			};
			specifikacijaNovcaRepository.Insert(entity);
		}

		var response = entity.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();
		response.Racunar = await CalculateRacunarDataAsync(
			(int)user.StoreId,
			response.DatumUTC.Date
		);
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

		response.Racunar = await CalculateRacunarDataAsync(
			(int)response.MagacinId,
			response.DatumUTC.Date
		);
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

		response.Racunar = await CalculateRacunarDataAsync(
			(int)response.MagacinId,
			response.DatumUTC.Date
		);

		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetSingleAsync(
		GetSingleSpecifikacijaNovcaRequest request
	)
	{
		var user = userRepository.GetCurrentUser();
		var response = specifikacijaNovcaRepository
			.Get(request.Id)
			.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();
		if (
			user.StoreId != response.MagacinId
			&& !user.Permissions.Any(x =>
				x.IsActive && x.Permission == Permission.SpecifikacijaNovcaSviMagacini
			)
		)
			throw new LSCoreForbiddenException();

		response.Racunar = await CalculateRacunarDataAsync(
			(int)response.MagacinId,
			response.DatumUTC.Date
		);

		return response;
	}

	public async Task<GetSpecifikacijaNovcaDto> GetSpecifikacijaByDate(
		GetSpecifikacijaByDateRequest request
	)
	{
		var user = userRepository.GetCurrentUser();

		if (
			request.MagacinId != user.StoreId
			&& !user.Permissions.Any(x =>
				x.Permission == Permission.SpecifikacijaNovcaSviMagacini && x.IsActive
			)
		)
			throw new LSCoreForbiddenException();

		if (
			request.Date.Date.AddDays(7) > DateTime.UtcNow.Date
			&& !user.Permissions.Any(x =>
				(
					x.Permission == Permission.SpecifikacijaNovcaPrethodnih7Dana
					|| x.Permission == Permission.SpecifikacijaNovcaSviDatumi
				) && x.IsActive
			)
		)
			throw new LSCoreForbiddenException();

		var entity = specifikacijaNovcaRepository.GetByDateOrDefault(
			request.Date,
			request.MagacinId
		);
		if (entity == null)
		{
			entity = new SpecifikacijaNovcaEntity
			{
				MagacinId = request.MagacinId,
				Datum = request.Date.Date,
				IsActive = true,
				CreatedBy = user.Id,
			};
			specifikacijaNovcaRepository.Insert(entity);
		}
		var response = entity.ToMapped<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>();

		response.Racunar = await CalculateRacunarDataAsync(
			(int)response.MagacinId,
			request.Date.Date
		);

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

		entity.InjectFrom(
			request.ToMapped<SaveSpecifikacijaNovcaRequest, SpecifikacijaNovcaPutTempDto>()
		);
		specifikacijaNovcaRepository.Update(entity);
	}

	private async Task<SpecifikacijaNovcaRacunarDto> CalculateRacunarDataAsync(
		int storeId,
		DateTime date
	)
	{
		var polazniMagacinFirma = komercijalnoMagacinFirmaRepository.GetByMagacinId(storeId);
		var client = komercijalnoClientFactory.Create(
			date.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(
				configurationRoot[Constants.DeployVariable]!
			),
			polazniMagacinFirma.ApiFirma
		);
		var racuniIPovratnice = await client.Dokumenti.GetMultipleAsync(
			new DokumentGetMultipleRequest
			{
				VrDok = [15, 22],
				DatumOd = date.Date.AddDays(-1),
				DatumDo = date.Date.AddDays(1),
				MagacinId = storeId,
				Flag = 1,
			}
		);

		var imaNefiskalizovanih = racuniIPovratnice.Any(x => x.Placen == 0);
		var gotovinskiRacuni = racuniIPovratnice
			.Where(x =>
				x.VrDok == 15 && x.NuId == (short)NacinUplate.Gotovina && x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);
		var kartice = racuniIPovratnice
			.Where(x =>
				x.VrDok == 15 && x.NuId == (short)NacinUplate.Kartica && x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);
		var virmanskiRacuni = racuniIPovratnice
			.Where(x =>
				x.VrDok == 15 && x.NuId == (short)NacinUplate.Virman && x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);
		var gotovinskePovratnice = racuniIPovratnice
			.Where(x =>
				x.VrDok == 22 && x.NuId == (short)NacinUplate.Gotovina && x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);
		var virmanskePovratnice = racuniIPovratnice
			.Where(x =>
				x.VrDok == 22 && x.NuId == (short)NacinUplate.Virman && x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);
		var ostalePovratnice = racuniIPovratnice
			.Where(x =>
				x.VrDok == 22
				&& x.NuId != (short)NacinUplate.Gotovina
				&& x.NuId != (short)NacinUplate.Virman
				&& x.Datum.Date == date.Date
			)
			.Sum(x => x.Potrazuje);

		return new SpecifikacijaNovcaRacunarDto
		{
			ImaNefiskalizovanih = imaNefiskalizovanih,
			GotovinskiRacuniValue = gotovinskiRacuni,
			VirmanskiRacuniValue = virmanskiRacuni,
			KarticeValue = kartice,
			GotovinskePovratniceValue = gotovinskePovratnice,
			VirmanskePovratniceValue = virmanskePovratnice,
			OstalePovratniceValue = ostalePovratnice,
		};
	}
}
