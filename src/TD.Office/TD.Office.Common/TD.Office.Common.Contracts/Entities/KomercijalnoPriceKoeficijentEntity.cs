using LSCore.Repository.Contracts;
namespace TD.Office.Common.Contracts.Entities;

public class KomercijalnoPriceKoeficijentEntity : LSCoreEntity {
    public string Naziv { get; set; }
    public decimal Vrednost { get; set; }
}