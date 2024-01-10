using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities
{
    public class KomercijalnoPriceEntity : LSCoreEntity
    {
        public int RobaId { get; set; }
        public decimal NabavnaCenaBezPDV { get; set; }
        public decimal ProdajnaCenaBezPDV { get; set; }
    }
}
