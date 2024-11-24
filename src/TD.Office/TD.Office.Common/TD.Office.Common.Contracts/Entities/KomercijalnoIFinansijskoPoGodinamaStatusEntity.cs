
using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities;
public class KomercijalnoIFinansijskoPoGodinamaStatusEntity : LSCoreEntity
{
    public string Naziv { get; set; }
    public bool IsDefault { get; set; }
}
