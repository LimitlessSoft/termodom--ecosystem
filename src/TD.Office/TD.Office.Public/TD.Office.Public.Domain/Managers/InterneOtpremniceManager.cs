using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Enums;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Domain.Managers;

public class InterneOtpremniceManager(
	TDOfficeInterneOtpremniceClient microserviceApi,
	IUserRepository userRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IInterneOtpremniceManager
{
	public async Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
		GetMultipleRequest request
	)
	{
		var resp = await microserviceApi.GetMultipleAsync(request);
		var userIdsInResponse = resp.Payload!.Select(x => Convert.ToInt64(x.Referent)).Distinct();
		var users = userRepository
			.GetMultiple()
			.Where(x => userIdsInResponse.Contains(x.Id!))
			.ToList();
		foreach (var item in resp.Payload!)
			item.Referent =
				users.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Referent))?.Username
				?? "Nepoznato";

		return resp;
	}

	public async Task<InternaOtpremnicaDto> CreateAsync(InterneOtpremniceCreateRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		request.CreatedBy = currentUser.Id;
		if (!userRepository.HasPermission(currentUser.Id, Permission.OtpremniceRadSaSvimMagacinima))
		{
			var currentuser = userRepository.GetCurrentUser();
			if (
				request.PolazniMagacinId != currentuser.StoreId
				&& request.PolazniMagacinId != currentuser.VPMagacinId
			)
				throw new LSCoreBadRequestException(
					OtpremniceValidationCodes.OVC_001.GetDescription()
				);
		}
		return await microserviceApi.CreateAsync(request);
	}

	public async Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request) =>
		await microserviceApi.GetAsync(request);

	public async Task<InternaOtpremnicaItemDto> SaveItemAsync(
		InterneOtpremniceItemCreateRequest request
	)
	{
		var currentUser = userRepository.GetCurrentUser();
		request.CreatedBy = currentUser.Id;
		return await microserviceApi.SaveItemAsync(request);
	}

	public Task DeleteItemAsync(InterneOtpremniceItemDeleteRequest request) =>
		microserviceApi.DeleteItemAsync(request);

	public Task ChangeStateAsync(LSCoreIdRequest request, InternaOtpremnicaStatus state) =>
		microserviceApi.ChangeStateAsync(request, state);

	public Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalno(LSCoreIdRequest request) =>
		microserviceApi.ForwardToKomercijalno(request);
}
