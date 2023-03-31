using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace API.Controllers.Webshop
{
    /// <summary>
    /// Koristi se za kontrolisanje stavki unutar porudzbine
    /// </summary>
    [ApiController]
    [RequireBearer]
    public class StavkaController : ControllerBase
    {
        /// <summary>
        /// Azurira podatke stavke po zadatom ID-u
        /// </summary>
        /// <param name="stavka"></param>
        /// <returns></returns>
        /// <response code="404">Stavka sa datim ID-em nije pronadjena</response>
        /// <response code="400">Neki od podataka nije validan</response>
        [HttpPost]
        [Route("/Webshop/Stavka/Update")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Stavka" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Update([Required] Models.Webshop.Stavka stavka)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (stavka.VPCena <= 0)
                        return StatusCode(400, "Neispravna VP cena!");

                    if (stavka.Kolicina <= 0)
                        return StatusCode(400, "Neispravna kolicina");

                    if (stavka.Rabat < 0)
                        return StatusCode(400, "Neispravan rabat!");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(ID) FROM STAVKA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", stavka.ID);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    if (Convert.ToInt32(dr[0]) == 0)
                                        return StatusCode(404);
                        }

                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE STAVKA SET
                            KOLICINA = @KOL,
                            VP_CENA = @VPC,
                            RABAT = @RABAT
                            WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@KOL", stavka.Kolicina);
                            cmd.Parameters.AddWithValue("@VPC", stavka.VPCena);
                            cmd.Parameters.AddWithValue("@RABAT", stavka.Rabat);
                            cmd.Parameters.AddWithValue("@ID", stavka.ID);

                            cmd.ExecuteNonQuery();

                            return StatusCode(200);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Kreira novu stavku u bazi
        /// </summary>
        /// <param name="stavka"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Stavka/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Stavka" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required] Models.Webshop.Stavka stavka)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (stavka == null)
                    return StatusCode(400);

                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        int id = 0;
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(MAX(ID), 0) FROM STAVKA", con))
                        using (MySqlDataReader dt = cmd.ExecuteReader())
                            if (dt.Read())
                                id = Convert.ToInt32(dt[0]) + 1;
                        using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO STAVKA
                            (ID, PORUDZBINA_ID, ROBA_ID, KOLICINA, VP_CENA, RABAT)
                            VALUES
                            (@ID, @PID, @RID, @KOLICINA, @VPCENA, @RABAT)", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@PID", stavka.PorudzbinaID);
                            cmd.Parameters.AddWithValue("@RID", stavka.RobaID);
                            cmd.Parameters.AddWithValue("@KOLICINA", stavka.Kolicina);
                            cmd.Parameters.AddWithValue("@VPCENA", stavka.VPCena);
                            cmd.Parameters.AddWithValue("@RABAT", stavka.Rabat);

                            cmd.ExecuteNonQuery();

                            return StatusCode(201, id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca objekat stavke po zadatom id-u
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Stavka/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        public Task<IActionResult> Get(int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, PORUDZBINA_ID, ROBA_ID, KOLICINA, VP_CENA, RABAT FROM STAVKA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                {
                                    return StatusCode(200, new Models.Webshop.Stavka()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PorudzbinaID = Convert.ToInt16(dr["PORUDZBINA_ID"]),
                                        RobaID = Convert.ToInt16(dr["ROBA_ID"]),
                                        Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                                        VPCena = Convert.ToDouble(dr["VP_CENA"]),
                                        Rabat = Convert.ToDouble(dr["RABAT"])
                                    });
                                }
                                else
                                {
                                    return StatusCode(404);
                                }
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca listu stavki
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Stavka/List")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        public Task<IActionResult> List(int? porudzbinaID)
        {
            return Task.Run<IActionResult>(() =>
            {
                List<Models.Webshop.Stavka> stavke = new List<Models.Webshop.Stavka>();
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        string cmd_text = "SELECT ID, PORUDZBINA_ID, ROBA_ID, KOLICINA, VP_CENA, RABAT FROM STAVKA WHERE 1";

                        if (porudzbinaID != null && porudzbinaID > 0)
                            cmd_text += " AND PORUDZBINA_ID = @PORUDZBINA_ID";

                        using (MySqlCommand cmd = new MySqlCommand(cmd_text, con))
                        {
                            if (porudzbinaID != null && porudzbinaID > 0)
                                cmd.Parameters.AddWithValue("@PORUDZBINA_ID", porudzbinaID);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                    stavke.Add(new Models.Webshop.Stavka()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PorudzbinaID = Convert.ToInt16(dr["PORUDZBINA_ID"]),
                                        RobaID = Convert.ToInt16(dr["ROBA_ID"]),
                                        Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                                        VPCena = Convert.ToDouble(dr["VP_CENA"]),
                                        Rabat = Convert.ToDouble(dr["RABAT"])
                                    });
                            }
                        }
                    }
                    return StatusCode(200, stavke);
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Uklanja stavku sa zadatim id-em
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Stavka/Delete")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        public Task<IActionResult> Delete([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM STAVKA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);

                            cmd.ExecuteNonQuery();

                            return StatusCode(200);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

    }
}
