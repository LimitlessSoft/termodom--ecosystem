using System;

namespace Termodom.Data.Entities.Komercijalno
{
	public class VrstaDok
	{
		public int VrDok { get; set; }
		public int Interni { get; set; }
		public string NazivDok { get; set; }
		public int? Poslednji { get; set; }
		public DateTime? DatumPosled { get; set; }
		public int KljucMenjaDatum { get; set; }
		public int IzmenaMenjaDatum { get; set; }
	}
}
