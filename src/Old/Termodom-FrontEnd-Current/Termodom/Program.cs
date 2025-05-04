using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Termodom.Models;

namespace Termodom
{
	public class Program
	{
		public static string BelgradeDateTime { get; set; } =
			TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time").Id;

		/// <summary>
		/// Govori da li je aplikacija trenutno pokrenuta
		/// </summary>
		public static bool IsRunning { get; set; } = true;

		/// <summary>
		/// Maksimalan broj pokusaja kada API vrati 403 status kod
		/// </summary>
		public static int ForbiddenRequestMaxTries { get; set; } = 10;

		/// <summary>
		/// Trenutno aktivni konekcioni string u programu
		/// </summary>
		public static string ConnectionString { get; set; } =
			"Server=174.138.184.42;Database=termodom_db_main;Uid=masdos_mdoas;Pwd=j1cnH38$;Pooling=false;";

		/// <summary>
		/// Absolutna putanja do wwwroot-a
		/// </summary>
		public static string WebRootPath { get; set; }

		/// <summary>
		/// Username koji se koristi za logovanje na API
		/// </summary>
		public static string APIUsername { get; set; }

		/// <summary>
		/// Sifra koja se koristi za logovanje na API
		/// </summary>
		public static string APIPassword { get; set; }

		/// <summary>
		/// Token koji se koristi za komunikaciju sa API-jem
		/// </summary>
		public static string APIToken { get; set; }

		/// <summary>
		/// Osnovni API url
		/// </summary>
		public static string BaseAPIUrl { get; set; }

		/// <summary>
		/// Task za osvezvanje API bearer tokena
		/// </summary>
		public static Task APIBearerTokenRefreshLoop { get; set; }

		/// <summary>
		/// Skladisti sve sesije korpi trenutno na sajtu
		/// </summary>
		public static Korpe Korpe { get; set; } = new Korpe();
		public static int nProfiCenovnikNivoa { get; set; } = 4;

		/// <summary>
		/// Trenutno aktivne sesije logovanih i nelogovanih korisnika
		/// </summary>
		public static List<Session> AktivneSesije { get; set; } = new List<Session>();

		/// <summary>
		/// Trenutno aktivne sesije logovanih korisnika
		/// </summary>
		public static List<Session> AktivneSesijeLogovanihKorisnika { get; set; } =
			new List<Session>();

		/// <summary>
		/// Broji koliko posetilaca ima od poslednjeg _posetilacaCounterReset
		/// </summary>
		public static int PosetilacaCounter { get; set; } = 0;

		/// <summary>
		/// Zapisuje kada je PosetilacaCounter resetovan
		/// </summary>
		private static DateTime _posetilacaCounterReset { get; set; } = DateTime.Now;

		static Program()
		{
			StartOldSessionCollectionLoopAsync();
			StartPosetilacaCounterResetLoopAsync();
		}

		/// <summary>
		/// Nemam blage veze. Pogledacu kasnije
		/// </summary>
		private static void StartPosetilacaCounterResetLoopAsync()
		{
			Thread t1 = new Thread(() =>
			{
				while (Program.IsRunning)
				{
					if (
						DateTime.Now.Day != _posetilacaCounterReset.Day
						|| DateTime.Now.Month != _posetilacaCounterReset.Month
						|| DateTime.Now.Year != _posetilacaCounterReset.Year
					)
					{
						Program.PosetilacaCounter = 0;
						_posetilacaCounterReset = DateTime.Now;
					}
					Thread.Sleep(1000);
				}
			});

			t1.IsBackground = true;
			t1.Start();
		}

		/// <summary>
		/// Nemam blage veze. Pogledacu kasnije
		/// </summary>
		private static void StartOldSessionCollectionLoopAsync()
		{
			Thread t1 = new Thread(() =>
			{
				while (Program.IsRunning)
				{
					Program.AktivneSesije.RemoveAll(x =>
						Math.Abs((x.PoslednjiPutAktivna - DateTime.Now).TotalSeconds) > 10
					);
					Program.AktivneSesijeLogovanihKorisnika.RemoveAll(x =>
						Math.Abs((x.PoslednjiPutAktivna - DateTime.Now).TotalSeconds) > 10
					);
					for (int i = 0; i < 5; i++)
						Thread.Sleep(1000);
				}
			});

			t1.IsBackground = true;
			t1.Start();
		}

		/// <summary>
		/// Pokrece loop za osvezavanje API tokena svakih X minuta
		/// </summary>
		public static Task StartAPIBearerTokenRefreshLoopAsync()
		{
			return APIBearerTokenRefreshLoop = Task.Run(() =>
			{
				while (Program.IsRunning)
				{
					RefreshTokenAsync().Wait();
					Thread.Sleep(TimeSpan.FromMinutes(3));
				}
			});
		}

		/// <summary>
		/// Osvezva API token
		/// </summary>
		public static Task RefreshTokenAsync()
		{
			return Task.Run(() =>
			{
				for (int i = 0; i < Program.ForbiddenRequestMaxTries; i++)
				{
					HttpRequestMessage request = new HttpRequestMessage(
						HttpMethod.Post,
						Program.BaseAPIUrl
							+ "/api/Korisnik/GetToken?username="
							+ APIUsername
							+ "&password="
							+ APIPassword
					);
					HttpClient client = new HttpClient();
					HttpResponseMessage response = client.Send(request);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						Program.APIToken = response.Content.ReadAsStringAsync().Result;
						return;
					}

					var a = response;
				}
				throw new Exception();
			});
		}

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
