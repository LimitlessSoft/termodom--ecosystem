using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.Requests.TabelarniPregledIzvoda;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface ITabelarniPregledIzvodaManager
    {
        LSCoreListResponse<TabelarniPregledIzvodaGetDto> Get(TabelarniPregledIzvodaGetRequest request);
        LSCoreResponse<DokumentTagIzvodGetDto> Put(DokumentTagizvodPutRequest request);
    }
}
