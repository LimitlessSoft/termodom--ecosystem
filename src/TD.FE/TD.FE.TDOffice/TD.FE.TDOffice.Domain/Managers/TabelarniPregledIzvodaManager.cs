using LSCore.Contracts.Http;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts;
using TD.FE.TDOffice.Contracts.DtoMappings;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.TabelarniPregledIzvoda;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class TabelarniPregledIzvodaManager : ITabelarniPregledIzvodaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        private readonly ITDOfficeApiManager _tdOfficeApiResposne;
        public TabelarniPregledIzvodaManager(ILogger<TabelarniPregledIzvodaManager> logger, IKomercijalnoApiManager komercijalnoApiManager,
            ITDOfficeApiManager tdOfficeApiResponse)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
            _tdOfficeApiResposne = tdOfficeApiResponse;
        }

        public LSCoreListResponse<TabelarniPregledIzvodaGetDto> Get(TabelarniPregledIzvodaGetRequest request)
        {
            var response = new LSCoreListResponse<TabelarniPregledIzvodaGetDto>();

            var komercijalnoApiResponse = _komercijalnoApiManager.GetAsync<DokumentGetMultipleRequest, List<DokumentDto>>(
                Constants.KomercijalnoApiEndpoints.Dokumenti.Get, new DokumentGetMultipleRequest()
                {
                    VrDok = 90,
                    DatumOd = request.OdDatuma,
                    DatumDo = request.DoDatuma
                })
                .GetAwaiter().GetResult();

            if (komercijalnoApiResponse.NotOk || komercijalnoApiResponse.Payload == null)
                return LSCoreListResponse<TabelarniPregledIzvodaGetDto>.BadRequest();

            var tdOfficeApiResponse = _tdOfficeApiResposne.GetAsync<DokumentTagIzvodGetMultipleRequest, List<DokumentTagIzvod>>(
                Constants.TDOfficeApiEndpoints.DokumentTagIzvodi.Get, new DokumentTagIzvodGetMultipleRequest()
                {

                })
                .GetAwaiter()
                .GetResult();

            if (tdOfficeApiResponse.NotOk || tdOfficeApiResponse.Payload == null)
                return LSCoreListResponse<TabelarniPregledIzvodaGetDto>.BadRequest();

            response.Payload = TabelarniPregledIzvodaGetDtoMappings.ConvertToTabelarniPregledIzvodaGetDtoList(komercijalnoApiResponse.Payload, tdOfficeApiResponse.Payload);
            return response;
        }

        public LSCoreResponse<DokumentTagIzvodGetDto> Put(DokumentTagizvodPutRequest request)
        {
            var response = new LSCoreResponse<DokumentTagIzvodGetDto>();

            var tdOfficeApiResponse = _tdOfficeApiResposne.PutAsync<DokumentTagizvodPutRequest, DokumentTagIzvodGetDto>(
                Constants.TDOfficeApiEndpoints.DokumentTagIzvodi.Put, request)
                .GetAwaiter()
                .GetResult();

            if (tdOfficeApiResponse.NotOk)
                return LSCoreResponse<DokumentTagIzvodGetDto>.BadRequest();

            response.Payload = tdOfficeApiResponse.Payload;
            return response;
        }
    }
}
