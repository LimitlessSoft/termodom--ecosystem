using LSCore.Repository.Contracts;

namespace TD.Office.Common.Contracts.Entities;

/// <summary>
/// Used for grouping magacins into centers
/// </summary>
public class MagacinCentarEntity : LSCoreEntity
{
	public string Naziv { get; set; }
	public List<int> MagacinIds { get; set; }
}
