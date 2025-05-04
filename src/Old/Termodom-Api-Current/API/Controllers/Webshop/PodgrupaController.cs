using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Models.Webshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Webshop
{
	/// <summary>
	/// Koristi se za upravljanje podgrupama
	/// </summary>
	[ApiController]
	public class PodgrupaController : Controller
	{
		/// <summary>
		/// Vraca podgrupu
		/// </summary>
		/// <param name="id">ID podgrupe</param>
		/// <param name="naziv">naziv podgrupe</param>
		/// <returns></returns>
		[HttpGet]
		[Route("/Webshop/Podgrupa/Get")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> Get(int id, string naziv)
		{
			return Task.Run<IActionResult>(() =>
			{
				if (id == 0 && string.IsNullOrWhiteSpace(naziv))
					return StatusCode(400, "Morate proslediti naziv ili id");

				if (id > 0 && !string.IsNullOrWhiteSpace(naziv))
					return StatusCode(400, "Ne mozete da prosledite dva parametra");

				try
				{
					string command = "SELECT ID, GROUPID, NAME, THUMBNAIL FROM SUBGROUP WHERE ";
					command += id > 0 ? "ID = @PAR" : "NAME = @PAR";

					using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
					{
						con.Open();
						using (MySqlCommand cmd = new MySqlCommand(command, con))
						{
							cmd.Parameters.AddWithValue("@PAR", id > 0 ? id : naziv);

							using (MySqlDataReader dr = cmd.ExecuteReader())
							{
								if (dr.Read())
								{
									return StatusCode(
										200,
										new Podgrupa()
										{
											ID = Convert.ToInt32(dr["ID"]),
											GrupaID = Convert.ToInt32(dr["GROUPID"]),
											Naziv = dr["NAME"].ToString(),
											Slika =
												dr["THUMBNAIL"] is DBNull
													? null
													: dr["THUMBNAIL"].ToString()
										}
									);
								}
								return StatusCode(204);
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
		/// List podgrupe
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("/Webshop/Podgrupa/List")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public Task<IActionResult> List()
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					List<Podgrupa> list = new List<Podgrupa>();
					using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
					{
						con.Open();
						using (
							MySqlCommand cmd = new MySqlCommand(
								@"SELECT
                        ID,
                        GROUPID,
                        NAME,
                        THUMBNAIL
                        FROM SUBGROUP
                        ",
								con
							)
						)
						{
							using (MySqlDataReader dr = cmd.ExecuteReader())
							{
								while (dr.Read())
								{
									list.Add(
										new Podgrupa()
										{
											ID = Convert.ToInt32(dr["ID"]),
											GrupaID = Convert.ToInt32(dr["GROUPID"]),
											Naziv = dr["NAME"].ToString(),
											Slika =
												dr["THUMBNAIL"] is DBNull
													? null
													: dr["THUMBNAIL"].ToString()
										}
									);
								}
							}
							return StatusCode(200, list);
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
