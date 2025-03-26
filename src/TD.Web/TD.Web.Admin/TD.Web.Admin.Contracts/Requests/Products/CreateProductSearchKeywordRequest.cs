using LSCore.Common.Contracts;

namespace TD.Web.Admin.Contracts.Requests.Products;

public class CreateProductSearchKeywordRequest : LSCoreIdRequest
{
	public string Keyword { get; set; }
}
