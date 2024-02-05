namespace TD.Web.Common.Contracts.Dtos.ProductsGroups
{
    public class GetProductGroupSequentialDto
    {
        public string Name { get; set; }
        public GetProductGroupSequentialDto? Child { get; set; }
    }
}
