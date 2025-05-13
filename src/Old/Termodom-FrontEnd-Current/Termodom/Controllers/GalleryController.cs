using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Termodom.Controllers
{
	public class GalleryController : Controller
	{
		private static string[] _allowedExtensions = new string[3] { ".jpg", ".jpeg", ".png" };

		public enum Quality
		{
			q1024,
			q512,
			q256,
			q128,
			q64,
			q32
		}

		static GalleryController()
		{
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\Source\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\32\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\64\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\128\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\256\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\512\\"));
			Directory.CreateDirectory(Path.Combine(Program.WebRootPath, "img\\gallery\\1024\\"));
		}

		[Route("/Gallery")]
		public IActionResult Index()
		{
			return View();
		}

		#region Partials

		[HttpGet]
		[Route("/p/Gallery/GetImageDisplaySmall")]
		public async Task<IActionResult> GetImageDisplaySmall(string path)
		{
			return await Task.Run(() =>
			{
				List<string> images = new List<string>();
				images.Add(path);
				return View("/Views/Gallery/_pGalleryItemDisplaySmall.cshtml", images);
			});
		}

		[HttpGet]
		[Route("/p/Gallery/GetAllImagesDisplaySmall")]
		public async Task<IActionResult> GetAllImagesDisplaySmall()
		{
			return await Task.Run(() =>
			{
				List<string> images = new List<string>();
				images.AddRange(
					Directory.GetFiles(Path.Combine(Program.WebRootPath, "img\\gallery\\Source\\"))
				);
				return View("/Views/Gallery/_pGalleryItemDisplaySmall.cshtml", images);
			});
		}
		#endregion

		#region API
		/// <summary>
		/// Uploads image to the gallery folders of quality 1024, 512, 256, 128, 64, 32
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorization("administrator")]
		[Route("/api/Gallery/Upload")]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			return await Task.Run<IActionResult>(async () =>
			{
				if (!_allowedExtensions.Contains(Path.GetExtension(file.FileName)))
					return StatusCode(415);

				using (
					Stream fileStream = new FileStream(
						Path.Combine(Program.WebRootPath, "img\\gallery\\Source\\" + file.FileName),
						FileMode.Create
					)
				)
					await file.CopyToAsync(fileStream);

				bool t1024 = SaveTo(file, Quality.q1024);
				bool t512 = SaveTo(file, Quality.q512);
				bool t256 = SaveTo(file, Quality.q256);
				bool t128 = SaveTo(file, Quality.q128);
				bool t64 = SaveTo(file, Quality.q64);
				bool t32 = SaveTo(file, Quality.q32);

				if (t1024 && t512 && t256 && t128 && t64 && t32)
					return StatusCode(
						200,
						Path.Combine(
							Program.WebRootPath,
							"\\img\\gallery\\Source\\" + file.FileName
						)
					);

				return StatusCode(500);
			});
		}
		#endregion

		private bool SaveTo(IFormFile file, Quality quality)
		{
			try
			{
				using (Stream memoryStream = new MemoryStream())
				{
					file.CopyTo(memoryStream);
					Bitmap origin = new Bitmap(memoryStream);

					int q = Convert.ToInt32(
						quality.ToString().Substring(1, quality.ToString().Length - 1)
					);

					double K = q / (double)Math.Min(origin.Height, origin.Width);

					Bitmap newPic = new Bitmap(
						origin,
						new Size(
							Convert.ToInt32(origin.Width * K),
							Convert.ToInt32(origin.Height * K)
						)
					);
					newPic.Save(
						Path.Combine(
							Program.WebRootPath,
							"img\\gallery\\"
								+ quality.ToString().Substring(1, quality.ToString().Length - 1)
								+ "/"
								+ file.FileName
						)
					);
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
