namespace TDBrain_v3.RequestBodies.TDOffice
{
	/// <summary>
	///
	/// </summary>
	public class KorisnikBeleskaSaveRequestBody
	{
		public int? Id { get; set; }
		public int KorisnikId { get; set; }
		public string Naslov { get; set; }
		public string? Body { get; set; }
	}
}
