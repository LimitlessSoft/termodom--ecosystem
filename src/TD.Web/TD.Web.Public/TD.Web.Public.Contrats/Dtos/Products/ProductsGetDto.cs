using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contrats.Dtos.Products
{
    public class ProductsGetDto
    {
        public string ImageSrc { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public ProductClassification Classification { get; set; }
    }
}
