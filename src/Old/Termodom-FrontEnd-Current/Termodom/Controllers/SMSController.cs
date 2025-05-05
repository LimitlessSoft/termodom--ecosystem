using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using Termodom.Models;

namespace Termodom.Controllers
{
	[Authorization("administrator", "staff")]
	public class SMSController : Controller
	{
		[Route("/SMS/Istorija/{korisnikID}")]
		public async Task<IActionResult> Istorija(int korisnikID)
		{
			return await Task.Run(() =>
			{
				Korisnik k = Korisnik.Get(korisnikID);

				if (k == null)
					return View(null);

				List<Models.LSSms> sms = Models.LSSms.GetByMobile(k.Mobilni).Result;
				return View("/Views/SMS/PoslatiSMS.cshtml", sms);
			});
		}

		[HttpPost]
		[Route("/api/SMS/Posalji")]
		public async Task<IActionResult> Posalji(string text, string[] mobilni)
		{
			return await Task.Run(() =>
			{
				Korisnik k = HttpContext.GetKorisnik();

				return StatusCode(207, PosaljiSMS(text, mobilni, k.ID));
			});
		}

		// TODO: public static Dictionary<string, HTTPResponse neki>
		public static List<Tuple<string, string>> PosaljiSMS(
			string text,
			string[] mobilni,
			int posiljalacID
		)
		{
			List<Tuple<string, string>> response = new List<Tuple<string, string>>();
			foreach (string m in mobilni)
			{
				try
				{
					SMS.SendSMS(m, text, posiljalacID);
					response.Add(new(m, "OK"));
				}
				catch (Exception ex)
				{
					response.Add(new(m, ex.Message));
				}
			}
			return response;
		}
	}
}
