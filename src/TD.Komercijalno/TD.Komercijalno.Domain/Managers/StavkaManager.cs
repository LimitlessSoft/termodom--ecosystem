using LSCore.Contracts.Exceptions;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class StavkaManager(
    ILogger<StavkaManager> logger,
    KomercijalnoDbContext dbContext,
    IProcedureManager procedureManager
) : LSCoreManagerBase<StavkaManager>(logger, dbContext), IStavkaManager
{
    public StavkaDto Create(StavkaCreateRequest request)
    {
        request.Validate();

        var stavka = new Stavka();

        var dokument = dbContext.Dokumenti.FirstOrDefault(x =>
            x.VrDok == request.VrDok && x.BrDok == request.BrDok
        );
        if (dokument == null)
            throw new LSCoreNotFoundException();

        var magacin = dbContext.Magacini.FirstOrDefault(x => x.Id == dokument.MagacinId);
        if (magacin == null)
            throw new LSCoreNotFoundException();

        var roba = dbContext
            .Roba.Include(x => x.Tarifa)
            .FirstOrDefault(x => x.Id == request.RobaId);
        if (roba == null)
            throw new LSCoreNotFoundException();

        if (string.IsNullOrWhiteSpace(request.Naziv))
            request.Naziv = roba.Naziv;

        var getCenaNaDanResponse = procedureManager.GetProdajnaCenaNaDan(
            new Contracts.Requests.Procedure.ProceduraGetProdajnaCenaNaDanRequest
            {
                Datum = DateTime.Now,
                MagacinId = request.CeneVuciIzOvogMagacina ?? dokument.MagacinId,
                RobaId = request.RobaId
            }
        );

        var prodajnaCenaBezPdvNaDan = getCenaNaDanResponse / ((100d + roba.Tarifa.Stopa) / 100d);

        if (request.ProdajnaCenaBezPdv == null)
            request.ProdajnaCenaBezPdv = prodajnaCenaBezPdvNaDan;

        if (request.ProdajnaCenaBezPdv.Value != prodajnaCenaBezPdvNaDan)
            request.Rabat +=
                ((request.ProdajnaCenaBezPdv.Value / prodajnaCenaBezPdvNaDan) - 1) * -100;

        if (request.NabavnaCena == null)
        {
            var robaUMagacinu = dbContext.RobaUMagacinu.FirstOrDefault(x =>
                x.MagacinId == (request.CeneVuciIzOvogMagacina ?? dokument.MagacinId)
                && x.RobaId == request.RobaId
            );
            request.NabavnaCena = robaUMagacinu?.NabavnaCena ?? 0;
        }

        var magacinResponse = dbContext.Magacini.FirstOrDefault(x => x.Id == dokument.MagacinId);
        if (magacinResponse == null)
            throw new LSCoreNotFoundException();

        stavka.InjectFrom(request);

        stavka.Vrsta = roba.Vrsta;
        stavka.MagacinId = dokument.MagacinId;
        stavka.ProdCenaBp = request.ProdajnaCenaBezPdv ?? 0;
        stavka.Rabat = request.Rabat;
        stavka.ProdajnaCena = magacin.Vrsta == 1 ? prodajnaCenaBezPdvNaDan : getCenaNaDanResponse;
        stavka.DevProdCena =
            (magacin.Vrsta == 1 ? prodajnaCenaBezPdvNaDan : getCenaNaDanResponse) / dokument.Kurs;
        stavka.TarifaId = roba.TarifaId;
        stavka.Porez = roba.Tarifa.Stopa;
        stavka.Taksa = 0;
        stavka.Akciza = 0;
        stavka.PorezIzlaz = roba.Tarifa.Stopa;
        stavka.PorezUlaz = roba.Tarifa.Stopa;
        stavka.NabCenSaPor = request.NabCenaSaPor ?? 0;
        stavka.FakturnaCena = request.FakturnaCena ?? 0;
        stavka.NabCenaBt = request.NabCenaBt ?? 0;
        stavka.Troskovi = request.Troskovi ?? 0;
        stavka.Korekcija = request.Korekcija ?? 0;
        stavka.MtId = magacinResponse.MtId;

        InsertNonLSCoreEntity(stavka);
        return stavka.ToStavkaDto();
    }

    public bool DeleteStavke(StavkeDeleteRequest request)
    {
        dbContext.Stavke.Where(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok).ExecuteDelete();
        dbContext.SaveChanges();
        return true;
    }

    public List<StavkaDto> GetMultiple(StavkaGetMultipleRequest request)
    {
        var dokumentiFilter = request
            .Dokument?.Select(
                (x) =>
                {
                    var split = x.Split('-');
                    return new
                    {
                        VrDok = Convert.ToInt32(split[0]),
                        BrDok = Convert.ToInt32(split[1])
                    };
                }
            )
            .ToList();

        var query = dbContext.Stavke.Where(x =>
            (request.VrDok == null || request.VrDok.Length == 0 || request.VrDok.Contains(x.VrDok))
            && (
                request.MagacinId == null
                || request.MagacinId.Length == 0
                || request.MagacinId.Contains(x.MagacinId)
            )
            && (
                dokumentiFilter == null
                || !dokumentiFilter.Any()
                || dokumentiFilter.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok)
            )
        );

        if (request.DokumentFilter != null)
        {
            query
                .Include(x => x.Dokument)
                .Where(x =>
                    (
                        request.DokumentFilter.PPID == null
                        || request.DokumentFilter.PPID.Length == 0
                        || (
                            x.Dokument.PPID != null
                            && request.DokumentFilter.PPID.Any(z => z == x.Dokument.PPID.Value)
                        )
                    )
                    && (
                        request.DokumentFilter.DatumOd == null
                        || x.Dokument.Datum >= request.DokumentFilter.DatumOd
                    )
                    && (
                        request.DokumentFilter.DatumDo == null
                        || x.Dokument.Datum >= request.DokumentFilter.DatumDo
                    )
                    && (
                        request.DokumentFilter.Flag == null
                        || x.Dokument.Flag == request.DokumentFilter.Flag
                    )
                );
        }
        throw new NotImplementedException();
    }
}
