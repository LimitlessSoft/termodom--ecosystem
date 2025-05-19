using LSCore.Repository.Contracts;
using TD.Komercijalno.Client;

namespace TD.Office.Common.Contracts.Entities;

/// <summary>
/// Used to describe which komercijalno magacinId responds to which Firma (database)
/// </summary>
public class KomercijalnoMagacinFirmaEntity : LSCoreEntity
{
	public int MagacinId { get; set; }
	public TDKomercijalnoFirma ApiFirma { get; set; }
}
