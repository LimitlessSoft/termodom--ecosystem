using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Termodom.Models;

namespace Termodom.Controllers
{
	public class KalkulatorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GipsPlafon()
		{
			return View("GipsPlafon");
		}

		public IActionResult Fasada()
		{
			return View();
		}
	}
}
