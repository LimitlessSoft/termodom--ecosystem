using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Partneri;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Enums.SortColumnCodes;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class PartnerManager(ILogger<PartnerManager> logger, KomercijalnoDbContext dbContext)
    : LSCoreManagerBase<PartnerManager>(logger, dbContext),
        IPartnerManager
{
    public int Create(PartneriCreateRequest request)
    {
        request.Validate();

        var partner = new Partner();
        partner.InjectFrom(request);

        // PPID is a unique identifier for partners, so we need to find the maximum value of PPID
        // and increment it by 1 to get the new PPID
        // We did one-time action and those partners have PPID > 100000
        // So we need to find the maximum value of PPID for partners with PPID < 100000 as regular partners
        var maxPpid = dbContext.Partneri.Where(x => x.Ppid < 100000).Max(x => x.Ppid);
        partner.Ppid = maxPpid + 1;

        // Setting default values as from Komercijalno
        partner.Popust = 0;
        partner.VaziDana = 0;
        partner.VpcId = 1;
        partner.ProcPc = 0;
        partner.Nppid = partner.Ppid;
        partner.ImaUgovor = 0;
        partner.CenovnikId = 0;
        partner.PristupniNivo = 99;
        partner.VrstaCenovnika = 0;
        partner.Valuta = "DIN";
        partner.DozvoljeniMinus = 0;
        partner.UvpcId = 0;
        partner.UprocPc = 0;
        partner.Pdvo = 0;
        partner.DrzavljanstvoId = 0;
        partner.ZanimanjeId = 0;
        partner.PrevozRobe = 0;
        partner.WebStatus = 0;
        partner.WebMagacinId = 0;
        partner.DugKupac = 0;
        partner.PotKupac = 0;
        partner.DugDobav = 0;
        partner.PotDobav = 0;
        partner.VpcRabat = 0;
        partner.WebDocConv = 0;
        partner.Aktivan = 1;
        partner.NazivZaStampu = partner.Naziv;
        partner.WebPrivilegije = 1;
        partner.Gppid = 0;
        partner.CeneOdGrupe = 0;
        partner.Pol = 0;
        partner.Web = 0;
        partner.WebUserId = 0;
        partner.WebShopConv = 0;
        partner.WebStat = 0;
        partner.DozvoljeniMinusRok = 0;
        partner.VrstaUpita = 0;
        partner.DevKl = 0;
        partner.Ekspid = 0;
        partner.SamoAvans = 0;
        partner.PrimaEf = 0;
        partner.Ef13 = 1;
        partner.Ef40 = 1;
        partner.Ef41 = 1;

        dbContext.Partneri.Add(partner);
        dbContext.SaveChanges();
        return partner.Ppid;
    }

    public LSCoreSortedAndPagedResponse<PartnerDto> GetMultiple(PartneriGetMultipleRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            request.SearchKeyword = request.SearchKeyword!.ToLower();

        var query = dbContext.Partneri.Where(x =>
            (
                string.IsNullOrWhiteSpace(request.SearchKeyword)
                || x.Naziv.ToLower().Contains(request.SearchKeyword)
                || x.Pib.ToLower().Contains(request.SearchKeyword)
                || (x.Adresa != null && x.Adresa.ToLower().Contains(request.SearchKeyword))
            )
            && (string.IsNullOrWhiteSpace(request.Pib) || x.Pib == request.Pib)
            && (string.IsNullOrWhiteSpace(request.Mbroj) || x.Mbroj == request.Mbroj)
            && (request.Ppid == null || request.Ppid.Length == 0 || request.Ppid.Contains(x.Ppid))
        );

        return new LSCoreSortedAndPagedResponse<PartnerDto>()
        {
            Pagination = new LSCoreSortedAndPagedResponse<PartnerDto>.PaginationData(
                request.CurrentPage,
                request.PageSize,
                query.Count()
            ),
            Payload = query
                .SortAndPageQuery(request, PartneriSortColumCodes.PartneriSortRules)
                .Select(x => new PartnerDto
                {
                    Ppid = x.Ppid,
                    Naziv = x.Naziv,
                    Adresa = x.Adresa,
                    Posta = x.Posta,
                    Mesto = x.Mesto,
                    Telefon = x.Telefon,
                    Fax = x.Fax,
                    Email = x.Email,
                    Kontakt = x.Kontakt,
                    Kategorija = x.Kategorija,
                    MestoId = x.MestoId,
                    ZapId = x.ZapId,
                    RefId = x.RefId,
                    Pib = x.Pib,
                    Mobilni = x.Mobilni,
                    NazivZaStampu = x.NazivZaStampu
                })
                .ToList()
        };
    }

    public List<PPKategorija> GetKategorije() => dbContext.PPKategorije.ToList();

    public int GetPoslednjiId() => dbContext.Partneri.Where(x => x.Ppid < 100000).Max(x => x.Ppid);

    /// <summary>
    /// Proverava da li postoji duplikat partnera sa nekim od ovih podataka
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public bool GetDuplikat(PartneriGetDuplikatRequest request) =>
        dbContext.Partneri.FirstOrDefault(x => x.Mbroj == request.Mbroj || x.Pib == request.Pib)
        != null;

    public PartnerDto GetSingle(LSCoreIdRequest request)
    {
        var partner = dbContext.Partneri.Find(request.Id);
        if (partner == null)
            throw new LSCoreNotFoundException();

        return new PartnerDto
        {
            Ppid = partner.Ppid,
            Naziv = partner.Naziv,
            Adresa = partner.Adresa,
            Posta = partner.Posta,
            Mesto = partner.Mesto,
            Telefon = partner.Telefon,
            Fax = partner.Fax,
            Email = partner.Email,
            Kontakt = partner.Kontakt,
            Kategorija = partner.Kategorija,
            MestoId = partner.MestoId,
            ZapId = partner.ZapId,
            RefId = partner.RefId,
            Pib = partner.Pib,
            Mobilni = partner.Mobilni,
            NazivZaStampu = partner.NazivZaStampu
        };
    }
}
