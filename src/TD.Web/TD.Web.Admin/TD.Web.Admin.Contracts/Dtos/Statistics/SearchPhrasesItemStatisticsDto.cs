namespace TD.Web.Admin.Contracts.Dtos.Statistics
{
	public class SearchPhrasesItemStatisticsDto
	{
		public int SearchedTimesCount { get; set; } = 0;
		public string Keyword { get; set; } = string.Empty;
		public List<string> Phrases { get; set; } = new();
	}
}
