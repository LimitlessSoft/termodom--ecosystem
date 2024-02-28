using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities
{
    public class PaymentTypeEntity : LSCoreEntity
    {
        public string Name { get; set; }
        public int KomercijalnoNUID { get; set; }
    }
}
