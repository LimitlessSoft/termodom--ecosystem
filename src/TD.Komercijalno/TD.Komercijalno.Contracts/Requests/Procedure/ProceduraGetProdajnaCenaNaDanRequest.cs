namespace TD.Komercijalno.Contracts.Requests.Procedure
{
	public class ProceduraGetProdajnaCenaNaDanRequest
	{
		public int MagacinId { get; set; }
		public int RobaId { get; set; }
		public DateTime Datum { get; set; }
		public int? ZaobidjiVrDok { get; set; }
		public int? ZaobidjiBrDok { get; set; }
	}
}
