using Infrastructure.Entities.Api;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DBMigrations.Controllers
{
    public class HomeController : Controller
    {
        private readonly OldApi _oldApi;
        private readonly DbMigrationsDbContext _dbContext;

        public HomeController(OldApi oldApi, DbMigrationsDbContext context)
        {
            _oldApi = oldApi;
            _dbContext = context;
        }

        [Route("~/")]
        public async Task<string> IndexAsync()
        {
            #region korisnici
            _dbContext.Users.RemoveRange(_dbContext.Users);
            var oldKorisnici = await _oldApi.GetAsync<List<Korisnik>>("/webshop/korisnik/list");

            foreach(var korisnik in oldKorisnici.Body)
            {
                _dbContext.Users.Add(new Infrastructure.Entities.ApiV2.User()
                {
                    Id = korisnik.id,
                    Username = korisnik.ime,
                    FirstName = korisnik.nadimak,
                    MobileNumber = korisnik.mobilni,
                    Password = korisnik.pw,
                    LastName = " "
                });
            }
            #endregion

            #region proizvodi
            var proizvodi = await _oldApi.GetAsync<List<Proizvod>>("/webshop/proizvod/list");
            var roba = await _oldApi.GetAsync<List<Roba>>("/api/roba/list");

            foreach(var proizvod in proizvodi.Body)
            {
                var r = roba.Body.FirstOrDefault(x => x.ID == proizvod.RobaID);
                _dbContext.Products.Add(new Infrastructure.Entities.ApiV2.Product()
                {
                    Active = proizvod.Aktivan == 1,
                    VAT = proizvod.PDV,
                    BaseUnit = r?.JM ?? "UNKNOWN",
                    BuyOnlyInTransportPackage = proizvod.KupovinaSamoUTransportnomPakovanju == 1,
                    CatalogueId = r?.KatBr ?? "UNKNOWN",
                    Description = proizvod.DetaljanOpis,
                    DisplayIndex = proizvod.DisplayIndex,
                    Id = proizvod.RobaID,
                    ImageUrl = proizvod.Slika,
                    Keywords = proizvod.Keywords.Split(' ').ToList(),
                    MaxPrice = proizvod.ProdajnaCena,
                    MinPrice = proizvod.NabavnaCena,
                    Name = r?.Naziv ?? "UNKNOWN",
                    PriceListGroupId = proizvod.CenovnaGrupaID,
                    Quality = (Infrastructure.Entities.ApiV2.Enums.ProductQuality)proizvod.Klasifikacija,
                    Rel = proizvod.Rel,
                    ShortDescription = proizvod.KratakOpis,
                    Subgroups = proizvod.Podgrupe,
                    TransportPackageQuantity = proizvod.TransportnoPakovanje,
                    TransportPackageUnit = proizvod.TransportnoPakovanjeJM,
                    Visits = 0
                });
            }
            #endregion

            _dbContext.SaveChanges();

            return "Gotovo!";
        }
    }
}
