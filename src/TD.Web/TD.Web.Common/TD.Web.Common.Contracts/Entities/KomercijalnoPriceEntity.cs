using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class KomercijalnoPriceEntity : LSCoreEntity
    {
        public int RobaId { get; set; }
        public double NabavnaCenaBezPDV { get; set; }
        public double ProdajnaCenaBezPDV { get; set; }
    }
}
