using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.Requests.ProductsGroups
{
    public class ProductsGroupsSaveRequest : SaveRequest
    {
        public string Name { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
