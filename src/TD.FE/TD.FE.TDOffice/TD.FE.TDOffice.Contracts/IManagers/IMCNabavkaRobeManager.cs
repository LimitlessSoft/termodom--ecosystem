using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe;
using TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMCNabavkaRobeManager : IBaseManager
    {
        Task<Response<bool>> ProveriPostojanjeCenovnikaNaDan(MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest request);
        Task<ListResponse<MCNabavkaRobeUporediCenovnikeItemDto>> UporediCenovnikeAsync();
        Task<ListResponse<CenovnikItem>> UvuciFajlAsync(MCNabavkaRobeUvuciFajlRequest request);
    }
}
