using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class ProcedureManager(ILogger<ProcedureManager> logger, KomercijalnoDbContext dbContext)
        : LSCoreManagerBase<ProcedureManager>(logger, dbContext),
            IProcedureManager
    {
        public double GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request)
        {
            request.Validate();

            var poslednjaStavka = dbContext
                .Stavke.Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x =>
                    x.Dokument.Datum <= request.Datum
                    && (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999")
                    && x.Dokument.KodDok == 0
                    && x.RobaId == request.RobaId
                    && (
                        request.ZaobidjiBrDok == null
                        || x.Dokument.VrDok != request.ZaobidjiVrDok
                            && x.Dokument.BrDok != request.ZaobidjiBrDok
                    )
                    && x.MagacinId == request.MagacinId
                    && x.Dokument.VrstaDok.DefiniseCenu == 1
                    && x.Dokument.VrstaDok.ImaKarticu.HasValue
                    && x.Dokument.VrstaDok.ImaKarticu == 1
                )
                .OrderByDescending(x => x.Dokument.Datum)
                .ThenByDescending(x => x.Dokument.Linked)
                .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                .ThenByDescending(x => x.VrDok)
                .ThenByDescending(x => x.BrDok)
                .ThenByDescending(x => x.Id)
                .FirstOrDefault();

            if (poslednjaStavka == null)
                return 0;

            return poslednjaStavka.Magacin.VodiSe == 4
                ? poslednjaStavka.NabavnaCena // Nisam siguran da li trebam ovde da handlam bez/sa pdv na VP magacinima, pa za sada zaobilazim
                : poslednjaStavka.Magacin.Vrsta == 1
                    ? poslednjaStavka.ProdajnaCena * (1 + poslednjaStavka.Tarifa.Stopa / 100)
                    : poslednjaStavka.ProdajnaCena;
        }

        public List<NabavnaCenaNaDanDto> GetNabavnaCenaNaDan(
            ProceduraGetNabavnaCenaNaDanRequest request
        )
        {
            var list = new List<NabavnaCenaNaDanDto>();

            var dokumentiNabavke = dbContext
                .Dokumenti.Where(x =>
                    x.MagacinId == Constants.MainNabavneCeneMagacin
                    && Constants.VrDokKojiDefinisuNabavneCene.Contains(x.VrDok)
                )
                .ToList();

            var stavkeNabavke = dbContext
                .Stavke.Where(x =>
                    x.MagacinId == Constants.MainNabavneCeneMagacin
                    && Constants.VrDokKojiDefinisuNabavneCene.Contains(x.VrDok)
                )
                .ToList();

            var roba = dbContext
                .Roba.Where(x => request.RobaId == null || request.RobaId.Contains(x.Id))
                .ToList();

            Parallel.ForEach(
                roba,
                r =>
                {
                    var stavkeNabavkeZaRobu = stavkeNabavke.Where(x => x.RobaId == r.Id).ToList();
                    var dokumentiNabavkeZaRobu = dokumentiNabavke
                        .Where(x =>
                            stavkeNabavkeZaRobu.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok)
                        )
                        .ToList();
                    var dokument36 = dokumentiNabavkeZaRobu.FirstOrDefault(x =>
                        x.VrDok == 36 && request.Datum >= x.Datum && request.Datum <= x.DatRoka
                    );

                    if (dokument36 != null)
                    {
                        list.Add(
                            new NabavnaCenaNaDanDto()
                            {
                                RobaId = r.Id,
                                NabavnaCenaBezPDV = stavkeNabavkeZaRobu
                                    .First(x =>
                                        x.VrDok == dokument36.VrDok && x.BrDok == dokument36.BrDok
                                    )
                                    .NabavnaCena
                            }
                        );
                        return;
                    }

                    var dokumentiKojiDolazeUObzir = dokumentiNabavkeZaRobu
                        .Where(x => x.Datum <= request.Datum)
                        .ToList();
                    dokumentiKojiDolazeUObzir.Sort((y, x) => x.Datum.CompareTo(y.Datum));

                    var vazeciDokumentNabavke = dokumentiKojiDolazeUObzir.FirstOrDefault();

                    if (vazeciDokumentNabavke == null)
                    {
                        list.Add(
                            new NabavnaCenaNaDanDto() { RobaId = r.Id, NabavnaCenaBezPDV = 0 }
                        );
                        return;
                    }

                    list.Add(
                        new NabavnaCenaNaDanDto()
                        {
                            RobaId = r.Id,
                            NabavnaCenaBezPDV = stavkeNabavkeZaRobu
                                .First(x =>
                                    x.VrDok == vazeciDokumentNabavke.VrDok
                                    && x.BrDok == vazeciDokumentNabavke.BrDok
                                )
                                .NabavnaCena
                        }
                    );
                }
            );

            return list;
        }

        public List<ProdajnaCenaNaDanDto> GetProdajnaCenaNaDanOptimized(
            ProceduraGetProdajnaCenaNaDanOptimizedRequest request
        )
        {
            var list = new List<ProdajnaCenaNaDanDto>();

            var robaUMagacinu = dbContext.RobaUMagacinu.Where(x =>
                x.MagacinId == request.MagacinId
            );

            var stavke = dbContext
                .Stavke.Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x =>
                    x.MagacinId == request.MagacinId
                    && x.Dokument.Datum <= request.Datum
                    && x.Dokument.KodDok == 0
                    && x.MagacinId == request.MagacinId
                    && x.Dokument.VrstaDok.DefiniseCenu == 1
                    && x.Dokument.VrstaDok.ImaKarticu.HasValue
                    && x.Dokument.VrstaDok.ImaKarticu == 1
                    &&
                    // This logic was pasted from GetProdajnaCenaNaDan method and this one was used there
                    // Commented it out here in case we need to use it in the future
                    // (request.ZaobidjiBrDok == null ||
                    //  x.Dokument.VrDok != request.ZaobidjiVrDok &&
                    //  x.Dokument.BrDok != request.ZaobidjiBrDok) &&
                    (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999")
                )
                .OrderByDescending(x => x.Dokument.Datum)
                .ThenByDescending(x => x.Dokument.Linked)
                .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                .ThenByDescending(x => x.VrDok)
                .ThenByDescending(x => x.BrDok)
                .ThenByDescending(x => x.Id)
                .ToList();

            foreach (var rum in robaUMagacinu)
            {
                var poslednjaStavka = stavke.FirstOrDefault(x => x.RobaId == rum.RobaId);

                list.Add(
                    new ProdajnaCenaNaDanDto()
                    {
                        RobaId = rum.RobaId,
                        ProdajnaCenaBezPDV =
                            poslednjaStavka == null
                                ? 0
                                : poslednjaStavka.Magacin.VodiSe == 4
                                    ? poslednjaStavka.NabavnaCena
                                    : poslednjaStavka.ProdajnaCena
                    }
                );
            }

            return list;
        }
    }
}
