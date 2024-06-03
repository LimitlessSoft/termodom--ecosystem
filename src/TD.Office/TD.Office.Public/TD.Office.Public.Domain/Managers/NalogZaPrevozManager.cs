using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Office.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Repository;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;

namespace TD.Office.Public.Domain.Managers
{
    public class NalogZaPrevozManager : LSCoreBaseManager<NalogZaPrevozManager, NalogZaPrevozEntity>, INalogZaPrevozManager
    {
        private readonly ITDKomercijalnoApiManager _komercijalnoApiManager;
        
        public NalogZaPrevozManager(ILogger<NalogZaPrevozManager> logger, OfficeDbContext dbContext, ITDKomercijalnoApiManager komercijalnoApiManager)
            : base(logger, dbContext)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }
        
        public LSCoreResponse SaveNalogZaPrevoz(SaveNalogZaPrevozRequest request)
        {
            var response = new LSCoreResponse();

            if (request.IsRequestInvalid(response))
                return response;

            response.Merge(Save(request));
            return response;
        }

        public async Task<LSCoreResponse<GetReferentniDokumentNalogZaPrevozDto>> GetReferentniDokument(
            GetReferentniDokumentNalogZaPrevozRequest request)
        {
            var response = new LSCoreResponse<GetReferentniDokumentNalogZaPrevozDto>();
            
            var dokumentResponse = await _komercijalnoApiManager.GetDokument(new DokumentGetRequest
            {
                VrDok = request.VrDok,
                BrDok = request.BrDok
            });
            
            response.Merge(dokumentResponse);
            if(response.NotOk)
                return response;

            var stavkePrevoza = dokumentResponse.Payload!.Stavke!
                .Where(x => x.Naziv!.ToLower().Contains("prevoz"))
                .ToList();
            
            response.Payload = new GetReferentniDokumentNalogZaPrevozDto
            {
                Datum = dokumentResponse.Payload!.Datum,
                Zakljucan = dokumentResponse.Payload.Flag == 1,
                VrednostStavkePrevozaBezPdv = stavkePrevoza.Count > 0
                    ? (decimal)stavkePrevoza.Sum(x => x.ProdajnaCena * (100 + x.Rabat) / 100 * x.Kolicina)
                    : null
            };
            return response;
        }

        public LSCoreListResponse<GetNalogZaPrevozDto> GetMultiple(GetMultipleNalogZaPrevozRequest request) =>
            Queryable(x => x.IsActive
                           && x.CreatedAt.Date >= request.DateFrom.Date
                           && x.CreatedAt.Date <= request.DateTo.Date
                           && x.StoreId == request.StoreId)
                .ToLSCoreListResponse<GetNalogZaPrevozDto, NalogZaPrevozEntity>();

        public LSCoreResponse<GetNalogZaPrevozDto> GetSingle(LSCoreIdRequest request)
        {
            var response = new LSCoreResponse<GetNalogZaPrevozDto>();
            
            var nalogZaPrevozResponse = First(x => x.IsActive && x.Id == request.Id);
            response.Merge(nalogZaPrevozResponse);
            if (response.NotOk)
                return response;
            
            response.Payload = nalogZaPrevozResponse.Payload!.ToDto<GetNalogZaPrevozDto, NalogZaPrevozEntity>();
            return response;
        }
    }
}