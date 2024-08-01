using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Office.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Office.Common.Repository;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;

namespace TD.Office.Public.Domain.Managers
{
    public class NalogZaPrevozManager (
        ILogger<NalogZaPrevozManager> logger,
        OfficeDbContext dbContext,
        ITDKomercijalnoApiManager komercijalnoApiManager)
        : LSCoreManagerBase<NalogZaPrevozManager, NalogZaPrevozEntity>(logger, dbContext), INalogZaPrevozManager
    {
        public void SaveNalogZaPrevoz(SaveNalogZaPrevozRequest request) =>
            Save(request);
        
        public async Task<GetReferentniDokumentNalogZaPrevozDto> GetReferentniDokumentAsync(
            GetReferentniDokumentNalogZaPrevozRequest request)
        {
            var dokument = await komercijalnoApiManager.GetDokumentAsync(new DokumentGetRequest
            {
                VrDok = request.VrDok,
                BrDok = request.BrDok
            });

            var stavkePrevoza = dokument.Stavke!
                .Where(x => x.Naziv!.ToLower().Contains("prevoz"))
                .ToList();
            
            return new GetReferentniDokumentNalogZaPrevozDto
            {
                Datum = dokument.Datum,
                Zakljucan = dokument.Flag == 1,
                VrednostStavkePrevozaBezPdv = stavkePrevoza.Count > 0
                    ? (decimal)stavkePrevoza.Sum(x => x.ProdajnaCena * (100 + x.Rabat) / 100 * x.Kolicina * (request.VrDok == 13 ? 1 : 0.8333334))
                    : null,
                PlacenVirmanom = stavkePrevoza.Count > 0
            };
        }

        public List<GetNalogZaPrevozDto> GetMultiple(GetMultipleNalogZaPrevozRequest request)
        {
            return Queryable().Where(x => x.IsActive
                           && x.CreatedAt.Date >= request.DateFrom.Date
                           && x.CreatedAt.Date <= request.DateTo.Date
                           && x.StoreId == request.StoreId)
                .ToDtoList<NalogZaPrevozEntity, GetNalogZaPrevozDto>();
        }

        public GetNalogZaPrevozDto GetSingle(LSCoreIdRequest request)
        {
            var nalogZaPrevoz = Queryable()
                .FirstOrDefault(x => x.IsActive && x.Id == request.Id);

            if (nalogZaPrevoz == null)
                throw new LSCoreNotFoundException();
            
            return nalogZaPrevoz.ToDto<NalogZaPrevozEntity, GetNalogZaPrevozDto>();
        }
    }
}