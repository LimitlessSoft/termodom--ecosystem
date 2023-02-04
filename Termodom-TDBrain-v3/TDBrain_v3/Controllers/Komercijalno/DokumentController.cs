using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// Api Controller koji se koristi za upravljanje dokumentima komercijalnog poslovanja
    /// </summary>
    [ApiController]
    public class DokumentController : Controller
    {
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
                    List<DB.Komercijalno.Dokument> list = new List<DB.Komercijalno.Dokument>();

                    foreach(int godina in godine)
                        list.AddRange(DB.Komercijalno.Dokument.List(godina, magacinID, queryParameters));

                    return Json(list);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
    }
}
