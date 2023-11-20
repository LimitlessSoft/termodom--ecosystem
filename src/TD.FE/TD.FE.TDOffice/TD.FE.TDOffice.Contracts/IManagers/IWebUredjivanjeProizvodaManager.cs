using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IWebUredjivanjeProizvodaManager
    {
        Task<LSCoreListResponse<WebUredjivanjeProizvodaProizvodiGetDto>> ProizvodiGet();
    }
}
