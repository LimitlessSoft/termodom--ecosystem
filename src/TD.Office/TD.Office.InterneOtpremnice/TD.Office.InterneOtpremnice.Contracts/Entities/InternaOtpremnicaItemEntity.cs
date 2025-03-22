using LSCore.Repository.Contracts;

namespace TD.Office.InterneOtpremnice.Contracts.Entities;

public class InternaOtpremnicaItemEntity : LSCoreEntity
{
	public int RobaId { get; set; }
	public decimal Kolicina { get; set; }

	public InternaOtpremnicaEntity InternaOtpremnica { get; set; }
}
