namespace TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Dtos;

public class ReportItemDto
{
	public string Baza { get; set; }
	public int MagacinId { get; set; }
	public long RobaId { get; set; }
	public ReportItemProblemSaCenomDto? ProblemSaCenom { get; set; }
	public ReportItemProblemSaRobomDto? ProblemSaRobom { get; set; }

	public class ReportItemProblemSaCenomDto
	{
		public double TrenutnaCena { get; set; }
		public double CenaTrebaDaBude { get; set; }
	}

	public class ReportItemProblemSaRobomDto
	{
		public string Opis { get; set; }
	}
}
