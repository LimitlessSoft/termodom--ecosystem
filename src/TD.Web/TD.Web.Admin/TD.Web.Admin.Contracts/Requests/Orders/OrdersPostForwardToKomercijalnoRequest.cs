namespace TD.Web.Admin.Contracts.Requests.Orders
{
    public class OrdersPostForwardToKomercijalnoRequest
    {
        public string OneTimeHash { get; set; }
        public bool? IsPonuda { get; set; }
    }
}