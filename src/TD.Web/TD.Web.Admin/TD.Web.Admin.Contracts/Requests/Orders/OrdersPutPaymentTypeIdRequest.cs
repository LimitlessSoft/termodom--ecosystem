using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Orders
{
	public class OrdersPutPaymentTypeIdRequest
	{
		public string OneTimeHash { get; set; }
		public int PaymentTypeId { get; set; }
	}
}
