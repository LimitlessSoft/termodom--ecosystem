using TD.Core.Contracts;

namespace TD.Web.Veleprodaja.Contracts.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Unidentified";
        public string ThumbnailImagePath { get; set; } = "Unidentified";
        public string FullSizedImagePath { get; set; } = "Unidentified";
        public string SKU { get; set; } = "Unidentified";
        public string Unit { get; set; } = "Unidentified";
    }
}
