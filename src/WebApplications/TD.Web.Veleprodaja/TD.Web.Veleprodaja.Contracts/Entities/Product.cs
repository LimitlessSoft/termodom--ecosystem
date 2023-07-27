using TD.Core.Contracts;

namespace TD.Web.Veleprodaja.Contracts.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string FullSizedImagePath { get; set; }
        public string SKU { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
    }
}
