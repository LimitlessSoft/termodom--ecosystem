using Infrastructure.Entities.ApiV2.Enums;
using System.Collections.Generic;

namespace Infrastructure.Entities.ApiV2
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatalogueId { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public double VAT { get; set; }
        public int DisplayIndex { get; set; }
        public string ShortDescription { get; set; }
        public int Visits { get; set; }
        public List<string> Keywords { get; set; }
        public string Description { get; set; }
        public string Rel { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string BaseUnit { get; set; }
        public ProductQuality Quality { get; set; }
        public string TransportPackageUnit { get; set; }
        public double TransportPackageQuantity { get; set; }
        public bool BuyOnlyInTransportPackage { get; set; }
        public int PriceListGroupId { get; set; }
        public List<int> Subgroups { get; set; }
    }
}
