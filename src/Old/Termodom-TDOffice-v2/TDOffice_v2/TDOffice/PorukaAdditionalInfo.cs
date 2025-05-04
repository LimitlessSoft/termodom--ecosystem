using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.TDOffice
{
	public class PorukaAdditionalInfo
	{
		public string AttachmentPath { get; set; }
		public PorukaAction Action { get; set; } = PorukaAction.NULL;
		public object AdditionalInfo { get; set; }
		public string Beleska { get; set; }
		public int PolozajPoruke { get; set; } = 0;
	}

	public enum PorukaAction
	{
		NULL = 0,
		NoviPartner = 1, // Adiitional info je novi partner ID
		NoviTDOfficePopis = 2, // Additinoal info je novi Popis ID
		OdgovorNaStickyPoruku = 3, // Additional info je ID poruke izvorne
		RealizovanoRazduzenjeMagacina = 4, // Additional info je ID razduzenja magacina
		RealizovanaZamenaRobe = 5, // Adiitional info je ID zamene robe
		PravoPristupaModulu = 6, // Aditional info je ModulID
		DataTableAttachment = 7, // Additional info je DataTable
		IzvestajProdajeRobe = 8, // Additional info je Tuple<DataTable, ConcurrentDictionary<int, Tuple<DateTime, DateTime>>>
		IzvestajProdajeRobeZbirnoPoMagacinima = 9, // Additional info je Tuple<DataTable, ConcurrentDictionary<int, Tuple<DateTime, DateTime>>>
		IzvestajPrometaMagacina = 10, // Additional info je DataTable
		OdobreniRabatMagacina = 11, // Additional info je DataTable
		BonusZakljucavanje = 12 // Adiitional info je ID korisnika koji pravi zahtev
	}
}
