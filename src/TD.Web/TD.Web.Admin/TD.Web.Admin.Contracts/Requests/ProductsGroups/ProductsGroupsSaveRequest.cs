using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups
{
    public class ProductsGroupsSaveRequest : SaveRequest
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
