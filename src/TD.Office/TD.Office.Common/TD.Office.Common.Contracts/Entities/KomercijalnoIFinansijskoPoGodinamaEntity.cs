using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

public class KomercijalnoIFinansijskoPoGodinamaEntity : LSCoreEntity
{
	public int PPID { get; set; }
	public string? Comment { get; set; }
	public long StatusId { get; set; }

	[NotMapped]
	public KomercijalnoIFinansijskoPoGodinamaStatusEntity Status { get; set; }
}
