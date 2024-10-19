using LSCore.Contracts.Exceptions;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Public.Contracts.Dtos.Izvestaji;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Izvestaji;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Domain.Managers;

public class IzvestajManager(ITDKomercijalnoApiManager tdKomercijalnoApiManager) : IIzvestajManager
{
    private async Task<
        List<DokumentDto>
    > GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
        GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    )
    {
        return await tdKomercijalnoApiManager.GetMultipleDokumentAsync(
            new DokumentGetMultipleRequest()
            {
                VrDok = [request.VrDok],
                NUID = [request.NUID],
                DatumOd = request.DatumOd,
                DatumDo = request.DatumDo,
                MagacinId = request.MagacinId
            }
        );
    }

    public async Task<GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaDto> GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
        GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    )
    {
        var roba = tdKomercijalnoApiManager.GetMultipleRobaAsync(
            new RobaGetMultipleRequest() { Vrsta = 1 }
        );

        var dokumenti =
            await GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

        var stavke = dokumenti.SelectMany(d => d.Stavke ?? []).ToList();

        var kolicinePoRobaId = stavke
            .GroupBy(s => s.RobaId)
            .Select(g => new
            {
                RobaId = g.Key,
                Naziv = roba.Result.FirstOrDefault(x => x.RobaId == g.Key)?.Naziv ?? "Undefined",
                Kolicina = g.Sum(s => s.Kolicina)
            })
            .ToList();

        return new GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaDto()
        {
            Items = kolicinePoRobaId
                .Select(x => new GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaItemDto()
                {
                    RobaId = x.RobaId,
                    Naziv = x.Naziv,
                    Kolicina = x.Kolicina
                })
                .ToList()
        };
    }

    public async Task ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
        ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    )
    {
        var destinacioniDokument = tdKomercijalnoApiManager.GetDokumentAsync(
            new DokumentGetRequest()
            {
                VrDok = request.DestinationVrDok,
                BrDok = request.DestinationBrDok
            }
        );
        var izvestaj = GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

        var des = await destinacioniDokument;
        var izv = await izvestaj;

        if (des.Flag == 1)
            throw new LSCoreBadRequestException("Dokument mora biti otkljucan!");

        foreach (var stavka in izv.Items)
        {
            await tdKomercijalnoApiManager.CreateStavkaAsync(
                new StavkaCreateRequest
                {
                    RobaId = stavka.RobaId,
                    Kolicina = stavka.Kolicina,
                    VrDok = des.VrDok,
                    BrDok = des.BrDok,
                }
            );
        }
    }

    public async Task PromeniNacinUplateAsync(PromeniNacinUplateRequest request)
    {
        var dokumenti =
            await GetDokumentiZaIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(request);

        foreach (var dokument in dokumenti)
        {
            await tdKomercijalnoApiManager.SetDokumentNacinPlacanjaAsync(
                new DokumentSetNacinPlacanjaRequest()
                {
                    VrDok = dokument.VrDok,
                    BrDok = dokument.BrDok,
                    NUID = request.DestinationNuid
                }
            );
        }
    }
}
