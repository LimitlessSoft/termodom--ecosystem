namespace TD.Web.Admin.Contracts.Dtos.Products;

public class ProductsGetLinkedDto {
    public string? Link { get; set; }
    public List<string> LinkedProducts { get; set; } = [];
}