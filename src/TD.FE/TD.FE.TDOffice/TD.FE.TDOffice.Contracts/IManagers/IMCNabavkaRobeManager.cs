using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe;
using TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMCNabavkaRobeManager : ILSCoreBaseManager
    {
        Task<LSCoreResponse<bool>> ProveriPostojanjeCenovnikaNaDan(MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest request);
        Task<LSCoreListResponse<MCNabavkaRobeUporediCenovnikeItemDto>> UporediCenovnikeAsync();
        Task<LSCoreListResponse<CenovnikItem>> UvuciFajlAsync(MCNabavkaRobeUvuciFajlRequest request);
    }
}
