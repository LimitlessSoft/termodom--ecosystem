using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using TD.Komercijalno.Repository;
using TD.Komercijalno.Contracts;
using LSCore.Domain.Validators;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;

namespace TD.Komercijalno.Domain.Managers
{
    public class ProcedureManager : LSCoreBaseManager<ProcedureManager>, IProcedureManager
    {
        public ProcedureManager(ILogger<ProcedureManager> logger, KomercijalnoDbContext dbContext) : base(logger, dbContext)
        {

        }

        public LSCoreResponse<double> GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request)
        {
            var response = new LSCoreResponse<double>();
            if (request.IsRequestInvalid(response))
                return response;

            var qStavkaResponse = Queryable<Stavka>();
            response.Merge(qStavkaResponse);
            if (response.NotOk)
                return response;

            var poslednjaStavka = qStavkaResponse.Payload!
                .Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x =>
                    x.Dokument.Datum <= request.Datum &&
                    (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999") &&
                    x.Dokument.KodDok == 0 &&
                    x.RobaId == request.RobaId &&
                    (request.ZaobidjiBrDok == null ||
                        x.Dokument.VrDok != request.ZaobidjiVrDok &&
                        x.Dokument.BrDok != request.ZaobidjiBrDok) &&
                    x.MagacinId == request.MagacinId &&
                    x.Dokument.VrstaDok.DefiniseCenu == 1 &&
                    x.Dokument.VrstaDok.ImaKarticu.HasValue &&
                    x.Dokument.VrstaDok.ImaKarticu == 1)
                .OrderByDescending(x => x.Dokument.Datum)
                .ThenByDescending(x => x.Dokument.Linked)
                .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                .ThenByDescending(x => x.VrDok)
                .ThenByDescending(x => x.BrDok)
                .ThenByDescending(x => x.Id)
                .FirstOrDefault();

            if (poslednjaStavka == null)
                return new LSCoreResponse<double>(0);

            return new LSCoreResponse<double>(poslednjaStavka.Magacin.VodiSe == 4 ? poslednjaStavka.NabavnaCena : poslednjaStavka.ProdajnaCena);
        }

        public LSCoreListResponse<NabavnaCenaNaDanDto> GetNabavnaCenaNaDan(ProceduraGetNabavnaCenaNaDanRequest request)
        {
            var response = new LSCoreListResponse<NabavnaCenaNaDanDto>();

            var qDokumentiNabavke = Queryable<Dokument>();
            response.Merge(qDokumentiNabavke);
            if (response.NotOk)
                return response;

            var dokumentiNabavke = qDokumentiNabavke.Payload!
                .Where(x =>
                    x.MagacinId == Constants.MainNabavneCeneMagacin &&
                    Constants.VrDokKojiDefinisuNabavneCene.Contains(x.VrDok))
                .ToList();

            var qStavkeNabavke = Queryable<Stavka>();
            response.Merge(qStavkeNabavke);
            if (response.NotOk)
                return response;
            
            var stavkeNabavke = qStavkeNabavke.Payload!
                .Where(x =>
                    x.MagacinId == Constants.MainNabavneCeneMagacin &&
                    Constants.VrDokKojiDefinisuNabavneCene.Contains(x.VrDok))
                .ToList();
            
            var qRoba = Queryable<Roba>();
            response.Merge(qRoba);
            if (response.NotOk)
                return response;
            
            var roba = qRoba.Payload!
                .Where(x => request.RobaId == null || request.RobaId.Contains(x.Id))
                .ToList();

            Parallel.ForEach(roba, r =>
            {
                var stavkeNabavkeZaRobu = stavkeNabavke.Where(x => x.RobaId == r.Id).ToList();
                var dokumentiNabavkeZaRobu = dokumentiNabavke.Where(x => stavkeNabavkeZaRobu.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok)).ToList();
                var dokument36 = dokumentiNabavkeZaRobu.FirstOrDefault(x => x.VrDok == 36 && request.Datum >= x.Datum && request.Datum <= x.DatRoka);
                
                if (dokument36 != null)
                {
                    response.Payload!.Add(new NabavnaCenaNaDanDto()
                    {
                        RobaId = r.Id,
                        NabavnaCenaBezPDV = stavkeNabavkeZaRobu.First(x => x.VrDok == dokument36.VrDok && x.BrDok == dokument36.BrDok).NabavnaCena
                    });
                    return;
                }
                
                var dokumentiKojiDolazeUObzir = dokumentiNabavkeZaRobu.Where(x => x.Datum <= request.Datum).ToList();
                dokumentiKojiDolazeUObzir.Sort((y, x) => x.Datum.CompareTo(y.Datum));
                
                var vazeciDokumentNabavke = dokumentiKojiDolazeUObzir.FirstOrDefault();
                
                if (vazeciDokumentNabavke == null)
                {
                    response.Payload!.Add(new NabavnaCenaNaDanDto()
                    {
                        RobaId = r.Id,
                        NabavnaCenaBezPDV = 0
                    });
                    return;
                }
                
                response.Payload!.Add(new NabavnaCenaNaDanDto()
                {
                    RobaId = r.Id,
                    NabavnaCenaBezPDV = stavkeNabavkeZaRobu.First(x => x.VrDok == vazeciDokumentNabavke.VrDok && x.BrDok == vazeciDokumentNabavke.BrDok).NabavnaCena
                });
            });
            
            return response;
        }

        public LSCoreListResponse<ProdajnaCenaNaDanDto> GetProdajnaCenaNaDanOptimized(ProceduraGetProdajnaCenaNaDanOptimizedRequest request)
        {
            var response = new LSCoreListResponse<ProdajnaCenaNaDanDto>();

            var qRobaUMagacinu = Queryable<RobaUMagacinu>();
            response.Merge(qRobaUMagacinu);
            if (response.NotOk)
                return response;

            var robaUMagacinu = qRobaUMagacinu.Payload!
                .Where(x => x.MagacinId == request.MagacinId);
            
            var qStavke = Queryable<Stavka>();
            response.Merge(qStavke);
            if (response.NotOk)
                return response;
            
            var stavke = qStavke.Payload!
                .Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x => x.MagacinId == request.MagacinId);

            foreach (var rum in robaUMagacinu)
            {
                var poslednjaStavka = stavke
                    .Where(x =>
                        x.RobaId == rum.RobaId &&
                        x.Dokument.Datum <= request.Datum &&
                        (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999") &&
                        x.Dokument.KodDok == 0 &&
                        // This logic was pasted from GetProdajnaCenaNaDan method and this one was used there
                        // Commented it out here in case we need to use it in the future
                        // (request.ZaobidjiBrDok == null ||
                        //  x.Dokument.VrDok != request.ZaobidjiVrDok &&
                        //  x.Dokument.BrDok != request.ZaobidjiBrDok) &&
                        x.MagacinId == request.MagacinId &&
                        x.Dokument.VrstaDok.DefiniseCenu == 1 &&
                        x.Dokument.VrstaDok.ImaKarticu.HasValue &&
                        x.Dokument.VrstaDok.ImaKarticu == 1)
                    .OrderByDescending(x => x.Dokument.Datum)
                    .ThenByDescending(x => x.Dokument.Linked)
                    .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                    .ThenByDescending(x => x.VrDok)
                    .ThenByDescending(x => x.BrDok)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault();
                
                    response.Payload.Add(new ProdajnaCenaNaDanDto()
                    {
                        RobaId = rum.RobaId,
                        ProdajnaCenaBezPDV = poslednjaStavka == null ?
                            0 :
                            poslednjaStavka.Magacin.VodiSe == 4 ? poslednjaStavka.NabavnaCena : poslednjaStavka.ProdajnaCena
                    });
            }

            return response;
        }
    }
}
