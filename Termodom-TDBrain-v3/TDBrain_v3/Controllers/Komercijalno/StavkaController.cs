using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// Api Controllerk oji se koristi za upravljanje stavkama komercijalnog poslovanja
    /// </summary>
    [ApiController]
    public class StavkaController : Controller
    {
        private static int[] VPDokumenti = { 0, 1, 13, 26 };
        private ILogger<StavkaController> _logger { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public StavkaController(ILogger<StavkaController> logger)
        {
            this._logger = logger;
        }

        public class KopirajDTO
        {
            [Required]
            public DTO.Komercijalno.Stavka.KopirajIzvornaBazaDTO izvornaBaza { get; set; }
            [Required]
            public DTO.Komercijalno.Stavka.KopirajDestinacionaBazaDTO destinacionaBaza { get; set; }
            [Required]
            public DTO.Komercijalno.Stavka.KopirajIzvorniDokumentDTO izvorniDokument { get; set; }
            [Required]
            public DTO.Komercijalno.Stavka.KopirajDestinacioniDokumentDTO destinacioniDokument { get; set; }
            [Required]
            public bool destinacioniDokumentMoraBitiOtkljucan { get; set; }
            [Required]
            public bool destinacioniDokumentMoraBitiPrazan { get; set; }
            [Required]
            public DTO.Komercijalno.Stavka.KopirajDestinacioniDokumentMoraBitiPrazanOptionsDTO destinacioniDokumentOnDuplikatStavkeOpcije { get; set; }
            [Required]
            public bool nabavneCeneKaoUIzvornomDokumentu { get; set; }
            [Required]
            public bool prodajneCeneKaoUIzvornomDokumentu { get; set; }
        }
        /// <summary>
        /// Kopira stavke iz dokumenta u dokument.
        /// Moguce je kopiranje iz razlicitih baza.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Stavka")]
        [Route("/Komercijalno/Stavka/Kopiraj")]
        [Consumes("application/json")]
        public Task<IActionResult> Kopiraj([FromBody] KopirajDTO dto)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    string izvornaBazaConnString = DB.Settings.ConnectionStringKomercijalno[dto.izvornaBaza.magacinID, dto.izvornaBaza.godina];
                    string destinacionaBazaConnString = DB.Settings.ConnectionStringKomercijalno[dto.destinacionaBaza.magacinID, dto.destinacionaBaza.godina];
                    FbConnection conIzvor = new FbConnection(izvornaBazaConnString);
                    FbConnection conDestinacija = new FbConnection(destinacionaBazaConnString);
                    conIzvor.Open();
                    conDestinacija.Open();

                    DB.Komercijalno.Dokument? dokIzvor = DB.Komercijalno.Dokument.Get(conIzvor, dto.izvorniDokument.VrDok, dto.izvorniDokument.BrDok);
                    DB.Komercijalno.Dokument? dokDestinacija = DB.Komercijalno.Dokument.Get(conDestinacija, dto.destinacioniDokument.VrDok, dto.destinacioniDokument.BrDok);

                    if(dokIzvor == null)
                        return StatusCode(400, $"Izvorni dokument [{dto.izvorniDokument.VrDok}, {dto.izvorniDokument.BrDok}] ne postoji u izvornoj bazi {dto.izvornaBaza}!");

                    if (dokDestinacija == null)
                        return StatusCode(400, $"Destinacioni dokument [{dto.destinacioniDokument.VrDok}, {dto.destinacioniDokument.BrDok}] ne postoji u destinacionoj bazi {dto.destinacionaBaza}!");

                    if (dto.destinacioniDokumentMoraBitiOtkljucan && dokDestinacija.Flag != 0)
                        return StatusCode(400, $"Destinacioni dokument nije otkljucan!");

                    DB.Komercijalno.Stavka.StavkaCollection izvorneStavke = DB.Komercijalno.Stavka.Dict(conIzvor, new List<string>() {
                        $"VRDOK = {dokIzvor.VrDok}",
                        $"BRDOK = {dokDestinacija.BrDok}"
                    });

                    if (izvorneStavke.Count() == 0)
                        return StatusCode(400, $"Izvorni dokument je prazan!");

                    DB.Komercijalno.Stavka.StavkaCollection destinacioneStavke = DB.Komercijalno.Stavka.Dict(conDestinacija, new List<string>()
                    {
                        $"VRDOK = {dokIzvor.VrDok}",
                        $"BRDOK = {dokDestinacija.BrDok}"
                    });

                    if (dto.destinacioniDokumentMoraBitiPrazan)
                        if(destinacioneStavke.Count() != 0)
                            return StatusCode(400, $"Destinacioni dokument nije prazan!");
                    else
                        if (dto.destinacioniDokumentOnDuplikatStavkeOpcije == null)
                            return StatusCode(400, $"U slucaju da destinacioni dokument ne mora biti prazan morate proslediti parametar 'destinacioniDokumentOnDuplikatStavkeOpcije'!");

                    Termodom.Data.Entities.Komercijalno.RobaDictionary robaCollection = DB.Komercijalno.Roba.Collection(conDestinacija);
                    DB.Komercijalno.RobaUMagacinu.RobaUMagacinuCollection robaUMagacinuCollection = DB.Komercijalno.RobaUMagacinu.Collection(conDestinacija);

                    foreach(DB.Komercijalno.Stavka izvornaStavka in izvorneStavke)
                    {
                        if (!dto.destinacioniDokumentMoraBitiPrazan)
                            if (destinacioneStavke.FirstOrDefault(x => x.RobaID == izvornaStavka.RobaID) != null)
                                if (dto.destinacioniDokumentOnDuplikatStavkeOpcije.OnDuplikatStavke == DTO.Komercijalno.Stavka.KopirajDestinacioniDokumentMoraBitiPrazanOptionsOnDuplikatStavke.ZaobidjiStavku)
                                    continue;

                        Termodom.Data.Entities.Komercijalno.Roba roba = robaCollection[izvornaStavka.RobaID];
                        DB.Komercijalno.RobaUMagacinu robaUMagacinu = robaUMagacinuCollection[izvornaStavka.MagacinID][izvornaStavka.RobaID];

                        int novaDestinacionaStavkaID = DB.Komercijalno.Stavka.Insert(conDestinacija, dokDestinacija, roba, robaUMagacinu, izvornaStavka.Kolicina, izvornaStavka.Rabat);

                        if(dto.nabavneCeneKaoUIzvornomDokumentu)
                        {
                            DB.Komercijalno.Stavka? destinacionaStavka = DB.Komercijalno.Stavka.Get(conDestinacija, novaDestinacionaStavkaID);

                            if (destinacionaStavka == null)
                            {
                                _logger.LogError("Greska prilikom ucitavanja novokreirane destinacione stavke!");
                                return StatusCode(500, $"Greska prilikom kopiranja stavki!");
                            }

                            destinacionaStavka.NabavnaCena = izvornaStavka.NabavnaCena;
                            destinacionaStavka.Update(conDestinacija);
                        }

                        if(dto.prodajneCeneKaoUIzvornomDokumentu)
                        {
                            DB.Komercijalno.Stavka? destinacionaStavka = DB.Komercijalno.Stavka.Get(conDestinacija, novaDestinacionaStavkaID);

                            if (destinacionaStavka == null)
                            {
                                _logger.LogError("Greska prilikom ucitavanja novokreirane destinacione stavke!");
                                return StatusCode(500, $"Greska prilikom kopiranja stavki!");
                            }

                            double prodajnaCenaBP = VPDokumenti.Contains(dto.izvorniDokument.VrDok) ? izvornaStavka.ProdajnaCena : izvornaStavka.ProdCenaBP;
                            destinacionaStavka.ProdajnaCena = VPDokumenti.Contains(dto.destinacioniDokument.VrDok) ? prodajnaCenaBP : izvornaStavka.ProdajnaCena;
                            destinacionaStavka.ProdCenaBP = izvornaStavka.ProdCenaBP;
                            destinacionaStavka.Korekcija = izvornaStavka.Korekcija;
                            destinacionaStavka.Update(conDestinacija);
                        }
                    }

                    conIzvor.Close();
                    conDestinacija.Close();
                    conIzvor.Dispose();
                    conDestinacija.Close();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
