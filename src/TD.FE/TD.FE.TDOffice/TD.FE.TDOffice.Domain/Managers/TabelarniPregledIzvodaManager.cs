using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts;
using TD.FE.TDOffice.Contracts.DtoMappings;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
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

        public ListResponse<TabelarniPregledIzvodaGetDto> Get()
        {
            var response = new ListResponse<TabelarniPregledIzvodaGetDto>();

            var komercijalnoApiResponse = _komercijalnoApiManager.GetAsync<DokumentGetMultipleRequest, List<DokumentDto>>(
                Constants.KomercijalnoApiEndpoints.Dokumenti.Get, new DokumentGetMultipleRequest()
                {
                    VrDok = 90
                })
                .GetAwaiter().GetResult();

            if (komercijalnoApiResponse.NotOk || komercijalnoApiResponse.Payload == null)
                return ListResponse<TabelarniPregledIzvodaGetDto>.BadRequest();

            var tdOfficeApiResponse = _tdOfficeApiResposne.GetAsync<DokumentTagIzvodGetMultipleRequest, List<DokumentTagIzvod>>(
                Constants.TDOfficeApiEndpoints.DokumentTagIzvodi.Get, new DokumentTagIzvodGetMultipleRequest()
                {

                })
                .GetAwaiter()
                .GetResult();

            if (tdOfficeApiResponse.NotOk || tdOfficeApiResponse.Payload == null)
                return ListResponse<TabelarniPregledIzvodaGetDto>.BadRequest();

            response.Payload = TabelarniPregledIzvodaGetDtoMappings.ConvertToTabelarniPregledIzvodaGetDtoList(komercijalnoApiResponse.Payload, tdOfficeApiResponse.Payload);
            return response;
        }

        public Response<bool> Put(DokumentTagizvodPutRequest request)
        {
            var response = new Response<bool>();
            var tdOfficeApiResponse = _tdOfficeApiResposne.PostAsync<DokumentTagizvodPutRequest, bool>(
                Constants.TDOfficeApiEndpoints.DokumentTagIzvodi.Put, request)
                .GetAwaiter()
                .GetResult();

            if (tdOfficeApiResponse.NotOk)
                return Response<bool>.BadRequest();

            return new Response<bool>(true);
        }
    }
}
