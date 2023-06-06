using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Managers.Komercijalno;
using TDBrain_v3.RequestBodies.Komercijalno;
using static TDBrain_v3.Settings;

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
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/Komercijalno/Stavka")]
        [Route("/Komercijalno/Stavka/Insert")]
        [Consumes("application/json")]
        public Task<IActionResult> Insert([FromBody] StavkaInsertRequestBody request)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[request.BazaId, request.GodinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        return StatusCode(201, StavkaManager.Insert(con, request.VrDok, request.BrDok, request.RobaId, request.Kolicina, request.Rabat, request.ProdajnaCenaBezPdv));
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/Komercijalno/Stavka")]
        [Route("/Komercijalno/Stavka/NapraviUslugu")]
        [Consumes("application/json")]
        public Task<IActionResult> NapraviUslugu([FromBody] NapraviUsluguRequestBody request)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[request.BazaId, request.GodinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        return StatusCode(201, ProceduraManager.NapraviUslugu(con, request.VrDok, request.BrDok, request.RobaId, request.CenaBezPdv, request.Kolicina, request.Rabat));
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
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

                    DB.Komercijalno.DokumentManager? dokIzvor = DB.Komercijalno.DokumentManager.Get(conIzvor, dto.izvorniDokument.VrDok, dto.izvorniDokument.BrDok);
                    DB.Komercijalno.DokumentManager? dokDestinacija = DB.Komercijalno.DokumentManager.Get(conDestinacija, dto.destinacioniDokument.VrDok, dto.destinacioniDokument.BrDok);

                    if(dokIzvor == null)
                        return StatusCode(400, $"Izvorni dokument [{dto.izvorniDokument.VrDok}, {dto.izvorniDokument.BrDok}] ne postoji u izvornoj bazi {dto.izvornaBaza}!");

                    if (dokDestinacija == null)
                        return StatusCode(400, $"Destinacioni dokument [{dto.destinacioniDokument.VrDok}, {dto.destinacioniDokument.BrDok}] ne postoji u destinacionoj bazi {dto.destinacionaBaza}!");

                    if (dto.destinacioniDokumentMoraBitiOtkljucan && dokDestinacija.Flag != 0)
                        return StatusCode(400, $"Destinacioni dokument nije otkljucan!");

                    Termodom.Data.Entities.Komercijalno.StavkaDictionary izvorneStavke = DB.Komercijalno.StavkaManager.Dictionary(conIzvor, new List<string>() {
                        $"VRDOK = {dokIzvor.VrDok}",
                        $"BRDOK = {dokDestinacija.BrDok}"
                    });

                    if (izvorneStavke.Count() == 0)
                        return StatusCode(400, $"Izvorni dokument je prazan!");

                    Termodom.Data.Entities.Komercijalno.StavkaDictionary destinacioneStavke = DB.Komercijalno.StavkaManager.Dictionary(conDestinacija, new List<string>()
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

                    Termodom.Data.Entities.Komercijalno.RobaDictionary robaCollection = DB.Komercijalno.RobaManager.Collection(conDestinacija);
                    DB.Komercijalno.RobaUMagacinuManager.RobaUMagacinuCollection robaUMagacinuCollection = DB.Komercijalno.RobaUMagacinuManager.Collection(conDestinacija);

                    foreach(Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka in izvorneStavke.Values)
                    {
                        if (!dto.destinacioniDokumentMoraBitiPrazan)
                            if (!destinacioneStavke.ContainsKey(izvornaStavka.RobaID))
                                if (dto.destinacioniDokumentOnDuplikatStavkeOpcije.OnDuplikatStavke == DTO.Komercijalno.Stavka.KopirajDestinacioniDokumentMoraBitiPrazanOptionsOnDuplikatStavke.ZaobidjiStavku)
                                    continue;

                        Termodom.Data.Entities.Komercijalno.Roba roba = robaCollection[izvornaStavka.RobaID];
                        DB.Komercijalno.RobaUMagacinuManager robaUMagacinu = robaUMagacinuCollection[izvornaStavka.MagacinID][izvornaStavka.RobaID];

                        int novaDestinacionaStavkaID = DB.Komercijalno.StavkaManager.Insert(conDestinacija, dokDestinacija, roba, robaUMagacinu, izvornaStavka.Kolicina, izvornaStavka.Rabat);

                        if(dto.nabavneCeneKaoUIzvornomDokumentu)
                        {
                            DB.Komercijalno.StavkaManager? destinacionaStavka = DB.Komercijalno.StavkaManager.Get(conDestinacija, novaDestinacionaStavkaID);

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
                            DB.Komercijalno.StavkaManager? destinacionaStavka = DB.Komercijalno.StavkaManager.Get(conDestinacija, novaDestinacionaStavkaID);

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

        /// <summary>
        /// Vraca listu objekata stavki iz komercijalnog
        /// </summary>
        /// <param name="bazaID">Proslediti MagacinID. Akcija ce se vrsiti nad bazom tog magacina.</param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="godina">Godina nad kojom se vrsi akcija. Ukoliko se ne prosledi akcija ce biti izvrsena na trenutnoj godini.</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Stavka")]
        [Route("/Komercijalno/Stavka/Dictionary")]
        public Task<IActionResult> Dictionary(
            [FromQuery][Required] int bazaID,
            [FromQuery][Required] int vrDok,
            [FromQuery][Required] int brDok,
            [FromQuery] int? godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaID, godina ?? DateTime.Now.Year]))
                {
                    con.Open();
                    return Json(DB.Komercijalno.StavkaManager.Dictionary(con, new List<string>() { $"VRDOK = {vrDok} AND BRDOK = {brDok}" }));
                }
            });
        }
    }
}
