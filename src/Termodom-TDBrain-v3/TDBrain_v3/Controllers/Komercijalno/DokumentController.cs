using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TDBrain_v3.Managers.Komercijalno;
using TDBrain_v3.RequestBodies.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// Api Controller koji se koristi za upravljanje dokumentima komercijalnog poslovanja
    /// </summary>
    [ApiController]
    public class DokumentController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private ILogger<DokumentController> _logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DokumentController(ILogger<DokumentController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Vraca objekat dokumenta iz baze komercijalnog poslovanja
        /// </summary>
        /// <param name="bazaID">Proslediti MagacinID. Akcija ce se vrsiti nad bazom tog magacina.</param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="godina">Godina nad kojom se vrsi akcija. Ukoliko se ne prosledi akcija ce biti izvrsena na trenutnoj godini.</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Dokument")]
        [Route("/Komercijalno/Dokument/Get")]
        public Task<IActionResult> Get(
            [FromQuery][Required] int bazaID,
            [FromQuery][Required] int vrDok,
            [FromQuery][Required] int brDok,
            [FromQuery] int? godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaID, godina ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        var dok = DokumentManager.Get(con, vrDok, brDok);
                        
                        if (dok == null)
                            return StatusCode(204);

                        return Json(dok);
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
        /// Vraca dictionary objekata dokumenata
        /// </summary>
        /// <param name="idBaze">MagacinID koji je vezan za bazu</param>
        /// <param name="godinaBaze"></param>
        /// <param name="odDatuma">[format: dd-MM-yyyy] (included)</param>
        /// <param name="doDatuma">[format: dd-MM-yyyy] (included)</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Dokument")]
        [Route("/Komercijalno/Dokument/Dictionary")]
        public Task<IActionResult> Dictionary(
            [FromQuery][Required] int idBaze,
            [FromQuery] int? godinaBaze,
            [FromQuery] int[]? vrDok,
            [FromQuery] int[]? magacinId,
            [FromQuery] string? odDatuma,
            [FromQuery] string? doDatuma)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> whereParameters = new List<string>();

                    if (vrDok != null && vrDok.Length > 0)
                        whereParameters.Add($"VRDOK IN ({string.Join(", ", vrDok)})");

                    if (magacinId != null && magacinId.Length > 0)
                        whereParameters.Add($"MAGACINID IN({string.Join(", ", magacinId)})");

                    if (!string.IsNullOrWhiteSpace(odDatuma) && odDatuma.Length != 10)
                        return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
                    else if(!string.IsNullOrWhiteSpace(odDatuma))
                        whereParameters.Add($"DATUM >= '{odDatuma}'");

                    if (!string.IsNullOrWhiteSpace(doDatuma) && doDatuma.Length != 10)
                        return StatusCode(400, "Parametar 'odDatuma' nije u formatu 'dd-MM-yyyy'");
                    else if (!string.IsNullOrWhiteSpace(doDatuma))
                        whereParameters.Add($"DATUM <= '{doDatuma}'");


                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[idBaze, godinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        return Json(new Termodom.Data.Entities.Komercijalno.DokumentDictionary(DB.Komercijalno.DokumentManager.Dictionary(con, whereParameters)));
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
        /// Vraca listu dokumenata komercijalnog poslovanja iz trenutne godine.
        /// Ukoliko zelite da obuhvati drugi period (prosle godine) onda dodati filtere datumOd i/ili datumDo i time ce
        /// se pokrenuti pretraga dokumenata i po prethodnim godinama/bazama. Ako se da samo jedan datum, onda se pretrazuje
        /// samo u toj godini (bilo da je od ili do), ako se daju oba datuma, uzimaju se u obzir te 2 godine, kao i sve
        /// godine izmedju.
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="brDokOd">(included)</param>
        /// <param name="brDokDo">(excluded)</param>
        /// <param name="datumOd">[format: dd-MM-yyyy] (included)</param>
        /// <param name="datumDo">[format: dd-MM-yyyy] (included)</param>
        /// <param name="ppid"></param>
        /// <param name="nuid"></param>
        /// <param name="flag"></param>
        /// <param name="placen"></param>
        /// <param name="zapid"></param>
        /// <param name="magacinID">ID magacina za koje se uzima lista. Ukoliko se ne prosledi nista, hvata za sve magacine. Moze se proslediti vise magacina.</param>
        [HttpGet]
        [Tags("/Komercijalno/Dokument")]
        [Route("/Komercijalno/Dokument/List")]
        public Task<IActionResult> List(
            [FromQuery] int[]? vrDok,
            [FromQuery] int? brDokOd,
            [FromQuery] int? brDokDo,
            [FromQuery] string? datumOd,
            [FromQuery] string? datumDo,
            [FromQuery] int[]? ppid,
            [FromQuery] int[]? nuid,
            [FromQuery] int[]? flag,
            [FromQuery] int? placen,
            [FromQuery] int[] zapid,
            [FromQuery] int[]? magacinID)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> queryParameters = new List<string>();

                    if (vrDok != null && vrDok.Length > 0)
                        queryParameters.Add($"VRDOK IN ({string.Join(", ", vrDok)})");

                    if (brDokOd != null)
                        queryParameters.Add($"BRDOK >= {brDokOd}");

                    if (brDokDo != null)
                        queryParameters.Add($"BRDOK <= {brDokDo}");

                    if (magacinID != null && magacinID.Length > 0)
                        queryParameters.Add($"MAGACINID IN ({string.Join(", ", magacinID)})");

                    if (datumOd != null)
                    {
                        if (datumOd.Length != 10)
                            return StatusCode(400, "Pogresno formatirana vrednost 'datumOd'! Potreban format 'dd-MM-yyyy'.");

                        queryParameters.Add($"DATUM >= '{datumOd}'");
                    }

                    if (datumDo != null)
                    {
                        if (datumDo.Length != 10)
                            return StatusCode(400, "Pogresno formatirana vrednost 'datumDo'! Potreban format 'dd-MM-yyyy'.");

                        queryParameters.Add($"DATUM <= '{datumOd}'");
                    }

                    if (ppid != null && ppid.Length > 0)
                        queryParameters.Add($"PPID IN ({string.Join(", ", ppid)})");

                    if (nuid != null && nuid.Length > 0)
                        queryParameters.Add($"NUID IN ({string.Join(", ", nuid)})");

                    if (flag != null && flag.Length > 0)
                        queryParameters.Add($"FLAG IN ({string.Join(", ", flag)})");

                    if (placen != null)
                        queryParameters.Add($"PLACEN = {placen}");

                    if (zapid != null && zapid.Length > 0)
                        queryParameters.Add($"ZAPID IN ({string.Join(", ", zapid)})");

                    List<int> godine = new List<int>();

                    if (string.IsNullOrWhiteSpace(datumOd) && string.IsNullOrWhiteSpace(datumDo))
                    {
                        godine.Add(DateTime.Now.Year);
                    }
                    else
                    {
                        int? godinaOd = null;
                        int? godinaDo = null;
                        int g = 0;

                        if (!string.IsNullOrWhiteSpace(datumOd))
                        {
                            g = Convert.ToInt32(datumOd.Split('-')[2]);
                            godinaOd = g;
                            godine.Add(g);
                        }

                        if (!string.IsNullOrWhiteSpace(datumDo))
                        {
                            g = Convert.ToInt32(datumDo.Split('-')[2]);
                            godinaDo = g;
                            godine.Add(g);
                        }

                        if (godinaOd != null && godinaDo != null)
                            for (int i = (int)godinaOd; i < (int)godinaDo; i++)
                                godine.Add(i);
                    }
                    List<DB.Komercijalno.DokumentManager> list = new List<DB.Komercijalno.DokumentManager>();

                    foreach(int godina in godine)
                        list.AddRange(DB.Komercijalno.DokumentManager.List(godina, magacinID, queryParameters));

                    return Json(list);
                }
                catch(Exception ex)
                {
                    ex.Log();
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
        [Tags("/Komercijalno/Dokument")]
        [Route("/Komercijalno/Dokument/Insert")]
        public Task<IActionResult> Insert([FromBody] DokumentInsertRequestBody request)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[request.BazaId, request.GodinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        return StatusCode(201, DokumentManager.Insert(con, request.VrDok, request.MagacinId, request.InterniBroj, request.PPID, request.NuId, request.KomercijalnoKorisnikId, request.DozvoliDaljeIzmeneUKomercijalnom));
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
        /// Presabira dokument
        /// </summary>
        /// <param name="bazaId"></param>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Dokument")]
        [Route("/Komercijalno/Dokument/Presaberi")]
        public Task<IActionResult> Presaberi(
            [FromQuery] int bazaId,
            [FromQuery] int vrDok,
            [FromQuery] int brDok,
            [FromQuery] int? godinaBaze = null)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        ProceduraManager.PresaberiDokument(con, vrDok, brDok);
                        return StatusCode(200);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
