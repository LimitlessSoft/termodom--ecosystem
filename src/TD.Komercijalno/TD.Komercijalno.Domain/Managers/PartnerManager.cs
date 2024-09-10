using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Entities;
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
}
