using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Public.Contracts.Enums
{
	public static class ProductsSortColumnCodes
	{
		public enum Products
		{
			Id
		}

		public static Dictionary<
			Products,
			Expression<Func<ProductEntity, object>>
		> ProductsSortRules = new() { { Products.Id, x => x.Id } };
	}
}
