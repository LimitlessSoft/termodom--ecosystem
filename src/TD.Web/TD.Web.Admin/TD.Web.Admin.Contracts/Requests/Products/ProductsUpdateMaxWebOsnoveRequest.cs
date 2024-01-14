namespace TD.Web.Admin.Contracts.Requests.Products
{
    public class ProductsUpdateMaxWebOsnoveRequest
    {
        public class Item
        {
            public int ProductId { get; set; }
            public decimal MaxWebOsnova { get; set; }
        }

        public List<Item> Items { get; set; }
    }
}
