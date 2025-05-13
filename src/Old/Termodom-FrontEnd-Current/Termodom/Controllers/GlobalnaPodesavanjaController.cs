using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using Termodom.Models;

namespace Termodom.Controllers
{
	public class GlobalnaPodesavanjaController : Controller
	{
		[HttpGet]
		[Route("/GlobalnaPodesavanja")]
		public IActionResult Index()
		{
			return View("/Views/GlobalnaPodesavanja/Index.cshtml");
		}

		#region Magacin
		[HttpGet]
		[Route("/api/GlobalnaPodesavanja/Magacin/List")]
		public Task<IActionResult> MagacinList()
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					List<Models.Magacin> list = Models.Magacin.List();
					return StatusCode(200, list);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		[HttpPost]
		[Route("/api/GlobalnaPodesavanja/Magacin/Update")]
		public Task<IActionResult> MagacinUpdate(Models.Magacin magacin)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					magacin.Update();
					return StatusCode(200);
				}
				catch (API.APIBadRequestException ex)
				{
					return StatusCode(500, ex.Message);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		[HttpPost]
		[Route("/api/GlobalnaPodesavanja/Magacin/Insert")]
		public Task<IActionResult> MagacinInsert(
			string adresa,
			string grad,
			string email,
			string koordinate,
			string telefon,
			string naziv
		)
		{
			return Task.Run<IActionResult>(() =>
			{
				try
				{
					Magacin.Insert(adresa, grad, email, koordinate, telefon, naziv);
					return StatusCode(201);
				}
				catch (API.APIBadRequestException ex)
				{
					return StatusCode(400, ex.Message);
				}
				catch (Exception ex)
				{
					return StatusCode(500);
				}
			});
		}
		#endregion

		#region Zanimanje
		[HttpPost]
		[Route("/api/GlobalnaPodesavanja/Zanimanje/Delete")]
		public async Task<IActionResult> ZanimanjeRemove(int id)
		{
			return await Task.Run(() =>
			{
				try
				{
					Zanimanje.Remove(id);
					return StatusCode(200);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		[HttpPost]
		[Route("/api/GlobalnaPodesavanja/Zanimanje/Insert")]
		public async Task<IActionResult> ZanimanjeInsert(string naziv)
		{
			return await Task.Run<IActionResult>(() =>
			{
				try
				{
					Zanimanje.Insert(naziv);
					return StatusCode(200);
				}
				catch (API.APIBadRequestException ex)
				{
					return StatusCode(400, ex);
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		[HttpPost]
		[Route("/api/GlobalnaPodesavanja/Zanimanje/List")]
		public async Task<IActionResult> ZanimanjeList(int id)
		{
			return await Task.Run<IActionResult>(() =>
			{
				try
				{
					return StatusCode(200, Zanimanje.List());
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}
		#endregion
		#region Validnost porudzbine

		#endregion
	}
}
