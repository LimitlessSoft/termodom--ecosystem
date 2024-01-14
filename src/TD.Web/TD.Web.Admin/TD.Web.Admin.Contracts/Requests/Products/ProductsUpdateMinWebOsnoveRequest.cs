namespace TD.Web.Admin.Contracts.Requests.Products
{
    public class ProductsUpdateMinWebOsnoveRequest
    {
        public class Item
        {
            public int ProductId { get; set; }
            public decimal MinWebOsnova { get; set; }
        }

        public List<Item> Items { get; set; }
    }
}
