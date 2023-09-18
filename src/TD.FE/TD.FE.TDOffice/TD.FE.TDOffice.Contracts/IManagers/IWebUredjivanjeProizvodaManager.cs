using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IWebUredjivanjeProizvodaManager
    {
        Task<ListResponse<WebUredjivanjeProizvodaProizvodiGetDto>> ProizvodiGet();
    }
}
