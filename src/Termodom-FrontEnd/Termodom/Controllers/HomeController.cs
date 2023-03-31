using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Termodom.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace Termodom.Controllers
{
    public class HomeController : Controller
    {
       
        [Route("/Dostava")]
        public async Task<IActionResult> Dostava()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View();
            });
        }
        [Route("/Kontakt")]
        public async Task<IActionResult> Kontakt()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View();
            });
        }
        [Route("/NacinKupovine")]
        public async Task<IActionResult> NacinKupovine()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View();
            });
        }

        [HttpGet]
        [Route("/ZivSam")] // TODO: LEL
        public async Task<IActionResult> ZivSam()
        {
            return await Task.Run(() =>
            {
                HttpContext.PrijaviDaSamIDaljeNaSajtu();
                return Ok();
            });
        }
    }
}
