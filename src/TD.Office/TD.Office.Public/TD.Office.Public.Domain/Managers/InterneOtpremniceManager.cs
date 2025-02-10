using LSCore.Contracts;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.Public.Contracts.Interfaces;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Domain.Managers;

public class InterneOtpremniceManager(TDOfficeInterneOtpremniceClient microserviceApi, IUserRepository userRepository, LSCoreContextUser contextUser)
    : IInterneOtpremniceManager
{
    public async Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(GetMultipleRequest request)
    {
        var resp = await microserviceApi.GetMultipleAsync(request);
        var userIdsInResponse = resp.Payload!.Select(x => Convert.ToInt64(x.Referent)).Distinct();
        var users = userRepository.GetMultiple().Where(x => userIdsInResponse.Contains(x.Id!)).ToList();
        foreach(var item in resp.Payload!)
            item.Referent = users.FirstOrDefault(x => x.Id == Convert.ToInt64(item.Referent))?.Username ?? "Nepoznato";
        
        return resp;
    }

    public async Task<InternaOtpremnicaDto> CreateAsync(InterneOtpremniceCreateRequest request)
    {
        request.CreatedBy = contextUser.Id!.Value;
        return await microserviceApi.CreateAsync(request);
    }

    public async Task<InternaOtpremnicaDetailsDto> GetAsync(LSCoreIdRequest request) =>
        await microserviceApi.GetAsync(request);

    public async Task<InternaOtpremnicaItemDto> SaveItemAsync(InterneOtpremniceItemCreateRequest request)
    {
        request.CreatedBy = contextUser.Id!.Value;
        return await microserviceApi.SaveItemAsync(request);
    }

    public Task DeleteItemAsync(InterneOtpremniceItemDeleteRequest request) =>
        microserviceApi.DeleteItemAsync(request);

    public Task ChangeStateAsync(LSCoreIdRequest request, InternaOtpremnicaStatus state) =>
        microserviceApi.ChangeStateAsync(request, state);

    public Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalno(LSCoreIdRequest request) =>
        microserviceApi.ForwardToKomercijalno(request);
}