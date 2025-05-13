namespace TD.Office.Public.Contracts.Requests.KomercijalnoApi
{
	public class KomercijalnoApiBaseRequest
	{
		public int Godina { get; set; } = DateTime.Now.Year;
	}
}
