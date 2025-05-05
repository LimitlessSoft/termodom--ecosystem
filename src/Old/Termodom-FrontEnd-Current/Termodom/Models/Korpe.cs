using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Termodom.Models
{
	public class Korpe
	{
		private static List<Korpa> _korpe = new List<Korpa>();

		//Korz kontroler spakuj u kolac
		public Korpa this[string identifikator]
		{
			get
			{
				Korpa k = _korpe.Where(x => x.Identifikator == identifikator).FirstOrDefault();
				if (k == null)
				{
					// Logika za dodavanje nove korpe u listu
					k = new Korpa();
					k.Identifikator = identifikator;
					while (_korpe.Any(x => x.Identifikator == k.Identifikator))
						k.Identifikator = RandomString();
					_korpe.Add(k);
				}
				return k;
			}
		}

		public void Remove(string identifikator)
		{
			_korpe.RemoveAll(x => x.Identifikator == identifikator);
		}

		public static string RandomString(int duzina = 15)
		{
			string svi = "qwertzuiop134567890'asdfghjklyxcvbnmQWERTZUIOPASDFGHJKLYXCVBNM";
			char[] nasumicno = new char[duzina];
			for (int i = 0; i < nasumicno.Length; i++)
				nasumicno[i] = svi[Random.Next(0, svi.Length - 1)];
			return new string(nasumicno);
		}
	}
}
