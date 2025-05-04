using System;
using System.Linq;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Http;
using Termodom.Models;

namespace Termodom
{
	public static class Extenstions
	{
		/// <summary>
		/// Vraca LimitlessSoft.AspNet.Authorization.Client za trenutni context.
		/// Ukoliko korisnik nije logovan vratice null
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Client GetClient(this HttpContext context)
		{
			return Client.Get(context);
		}

		/// <summary>
		/// Vraca WebShop.Korisnik ucitavanjem iz context-a.
		/// Vratice null ukoliko nije ulogovan ili ukoliko je pozvan pre nego
		/// sto je korisnik dodeljen contextu u pipeline-u
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Korisnik GetKorisnik(this HttpContext context)
		{
			return context.Items["korisnik"] as Korisnik;
		}

		/// <summary>
		/// Proverava da li je dati korisnik administrator. Ukoliko je null ili nije adminsitrator vraca false
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		public static bool IsAdministrator(this Korisnik sender)
		{
			if (sender != null && sender.Tip == 1)
				return true;

			return false;
		}

		/// <summary>
		/// Vraca tip kupca
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Enums.TipKupca GetTipKupca(this HttpContext context)
		{
			string tipKupcaCookie = context.Request.Cookies["tip-kupca"];

			if (string.IsNullOrWhiteSpace(tipKupcaCookie))
				return Enums.TipKupca.NULL;

			if (tipKupcaCookie == "jednokratni")
				return Enums.TipKupca.Jednokratni;
			else if (tipKupcaCookie == "profi")
				return Enums.TipKupca.Profi;

			return Enums.TipKupca.NULL;
		}

		/// <summary>
		/// Vraca tip kupca
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Enums.TipKupca GetTipKupca(this HttpRequest request)
		{
			// Ovo radim za slucaj da se uloguje pa obrise kolacice
			if (request.HttpContext.GetKorisnik() != null)
				return Enums.TipKupca.Profi;

			string tipKupcaCookie = request.Cookies["tip-kupca"];

			if (string.IsNullOrWhiteSpace(tipKupcaCookie))
				return Enums.TipKupca.NULL;

			tipKupcaCookie = tipKupcaCookie.ToLower();

			if (tipKupcaCookie == "jednokratni".ToLower())
				return Enums.TipKupca.Jednokratni;
			else if (tipKupcaCookie == "profi".ToLower())
				return Enums.TipKupca.Profi;

			return Enums.TipKupca.NULL;
		}

		/// <summary>
		/// Vraca tip kupca
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Enums.TipKupca GetTipKupca(this HttpResponse response)
		{
			string tipKupcaCookie = response.HttpContext.Request.Cookies["tip-kupca"];

			if (string.IsNullOrWhiteSpace(tipKupcaCookie))
				return Enums.TipKupca.NULL;

			if (tipKupcaCookie.ToLower() == "jednokratni")
				return Enums.TipKupca.Jednokratni;
			else if (tipKupcaCookie.ToLower() == "profi")
				return Enums.TipKupca.Profi;

			return Enums.TipKupca.NULL;
		}

		/// <summary>
		/// Vraca korpu za kupca. Ukoliko je ne pronadje kreira novu praznu
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Korpa GetKorpa(this HttpContext context)
		{
			return context.Request.GetKorpa();
		}

		/// <summary>
		/// Vraca korpu za kupca. Ukoliko je ne pronadje kreira novu praznu
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Korpa GetKorpa(this HttpRequest request)
		{
			string korpaID = request.Cookies["korpa"];
			if (string.IsNullOrEmpty(korpaID))
			{
				string indetifikator = Korpe.RandomString();
				Korpa nova = Program.Korpe[indetifikator];
				request.HttpContext.Response.Cookies.Append("korpa", nova.Identifikator);
				korpaID = nova.Identifikator;
			}
			return Program.Korpe[korpaID];
		}

		/// <summary>
		/// Vraca korpu za kupca. Ukoliko je ne pronadje kreira novu praznu
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Korpa GetKorpa(this HttpResponse context)
		{
			return context.HttpContext.GetKorpa();
		}

		/// <summary>
		/// Clears cart for user from given request
		/// </summary>
		/// <param name="request"></param>
		public static void ClearKorpa(this HttpRequest request)
		{
			string korpaID = request.Cookies["korpa"];
			if (string.IsNullOrEmpty(korpaID))
				return;
			Program.Korpe.Remove(korpaID);
		}

		/// <summary>
		/// Vraca cenovnik za korisnika trenutne sesije
		/// Ukoliko koirisnik nije logovan, vraca cenovnik kao za jednokratnog korisnika (zavisno od velicine korpe)
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Cenovnik GetCenovnik(this HttpContext context)
		{
			Korisnik k = context.GetKorisnik();

			if (k == null)
			{
				return Models.JednokratnaKupovina.GetCenovnik(context.GetKorpa());
			}
			else
			{
				return k.GetCenovnik();
			}
		}

		/// <summary>
		/// Javlja serveru da je klijent na sajtu. Klijent ne mora biti logovan!
		/// </summary>
		/// <param name="context"></param>
		public static void PrijaviDaSamIDaljeNaSajtu(this HttpContext context)
		{
			string sessionID = context.Request.Cookies["session-id"].ToStringOrDefault();

			if (string.IsNullOrWhiteSpace(sessionID))
			{
				sessionID = Random.Next(Int32.MaxValue).ToString();
				while (Program.AktivneSesije.Count(x => x.ID == sessionID) > 0)
					sessionID = Random.Next(Int32.MaxValue).ToString();

				context.Response.Cookies.Append("session-id", sessionID);
			}

			Program.AktivneSesije.RemoveAll(x => x.ID == sessionID);

			Program.AktivneSesije.Add(
				new Session() { ID = sessionID, PoslednjiPutAktivna = DateTime.Now }
			);

			Korisnik logovaniKorisnik = context.GetKorisnik();

			if (logovaniKorisnik != null)
			{
				Program.AktivneSesijeLogovanihKorisnika.RemoveAll(x =>
					x.ID == logovaniKorisnik.ID.ToString()
				);

				Program.AktivneSesijeLogovanihKorisnika.Add(
					new Session()
					{
						ID = logovaniKorisnik.ID.ToString(),
						PoslednjiPutAktivna = DateTime.Now
					}
				);
			}
		}

		/// <summary>
		/// Konvertuje objekat u string ili null
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		public static string ToStringOrDefault(this object sender)
		{
			if (sender == null)
				return "";

			return sender.ToString();
		}

		/// <summary>
		/// Trims string and divide it on capital letters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="FirstCapital"></param>
		/// <param name="CapitalOnSeparate"></param>
		/// <returns></returns>
		public static string DivideOnCapital(
			this object sender,
			CharacterState FirstCharacterState = CharacterState.Initial,
			CharacterState SeparatedCharacterState = CharacterState.Lowercase
		)
		{
			return sender.ToString().DivideOnCapital(FirstCharacterState, SeparatedCharacterState);
		}

		/// <summary>
		/// Trims string and divide it on capital letters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="FirstCapital"></param>
		/// <param name="CapitalOnSeparate"></param>
		/// <returns></returns>
		public static string DivideOnCapital(
			this string sender,
			CharacterState FirstCharacterState = CharacterState.Initial,
			CharacterState SeparatedCharacterState = CharacterState.Lowercase
		)
		{
			string returned = "";

			sender = sender.Trim();

			returned +=
				FirstCharacterState == CharacterState.Initial
					? sender[0]
					: FirstCharacterState == CharacterState.Capital
						? char.ToUpper(sender[0])
						: char.ToLower(sender[0]);

			sender = sender.Substring(1, sender.Length - 1);

			foreach (char c in sender)
			{
				if (char.IsUpper(c))
				{
					returned += ' ';
					returned +=
						SeparatedCharacterState == CharacterState.Initial
							? c
							: SeparatedCharacterState == CharacterState.Capital
								? char.ToUpper(c)
								: char.ToLower(c);
					;
					continue;
				}
				returned += c;
			}

			return returned.Trim();
		}

		public enum CharacterState
		{
			Initial = 0,
			Capital = 1,
			Lowercase = 2
		}
	}
}
