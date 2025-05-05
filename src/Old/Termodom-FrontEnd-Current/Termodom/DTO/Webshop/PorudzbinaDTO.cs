using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Termodom.Models;

namespace Termodom.DTO.Webshop
{
	public class PorudzbinaDTO
	{
		public int ID { get; set; }
		public int KorisnikID { get; set; }
		public int BrDokKomercijalno { get; set; }
		public DateTime Datum { get; set; }
		public int Status { get; set; }
		public int MagacinID { get; set; }
		public int? PPID { get; set; }
		public string InterniKomentar { get; set; }
		public int? ReferentObrade { get; set; }
		public int NacinPlacanja { get; set; }
		public string Hash { get; set; }
		public double K { get; set; }
		public double UstedaKorisnik { get; set; }
		public double UstedaKlijent { get; set; }
		public int Dostava { get; set; }
		public string KomercijalnoInterniKomentar { get; set; }
		public string KomercijalnoKomentar { get; set; }
		public string KomercijalnoNapomena { get; set; }
		public string Napomena { get; set; }
		public string AdresaIsporuke { get; set; }
		public string KontaktMobilni { get; set; }
		public string ImeIPrezime { get; set; }

		public static PorudzbinaDTO ConvertPorudzbina(Porudzbina porudzbina)
		{
			return new PorudzbinaDTO()
			{
				ID = porudzbina.ID,
				BrDokKomercijalno = porudzbina.BrDokKom,
				KorisnikID = porudzbina.UserID,
				Datum = porudzbina.Datum,
				Status = (int)porudzbina.Status,
				MagacinID = porudzbina.MagacinID,
				PPID = porudzbina.PPID,
				InterniKomentar = porudzbina.InterniKomentar,
				ReferentObrade = porudzbina.ReferentObrade,
				NacinPlacanja = (int)porudzbina.NacinUplate,
				K = porudzbina.K,
				UstedaKorisnik = porudzbina.UstedaKorisnik,
				UstedaKlijent = porudzbina.UstedaKlijent,
				Dostava = porudzbina.Dostava ? 1 : 0,
				KomercijalnoInterniKomentar = porudzbina.KomercijalnoInterniKomentar,
				KomercijalnoKomentar = porudzbina.KomercijalnoKomentar,
				KomercijalnoNapomena = porudzbina.KomercijalnoNapomena,
				KontaktMobilni = porudzbina.KontaktMobilni,
				ImeIPrezime = porudzbina.ImeIPrezime,
				Napomena = porudzbina.Napomena,
				AdresaIsporuke = porudzbina.AdresaIsporuke
			};
		}
	}
}
