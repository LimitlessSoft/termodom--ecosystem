using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

public class ProracunItemEntity : LSCoreEntity
{
	public int RobaId { get; set; }
	public decimal Kolicina { get; set; }
	public decimal CenaBezPdv { get; set; }
	public decimal Pdv { get; set; }
	public decimal Rabat { get; set; }
	public long ProracunId { get; set; }
}
