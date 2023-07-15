using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface ITabelarniPregledIzvodaManager
    {
        ListResponse<TabelarniPregledIzvodaGetDto> Get();
        Response<bool> Put(DokumentTagizvodPutRequest request);
    }
}
