// using TD.Web.Admin.Contracts.Requests.ProductsGroups;
// using TD.Web.Common.Contracts.Entities;
// using System.Text.RegularExpressions;
// using LSCore.Contracts.Interfaces;
// using Omu.ValueInjecter;
//
// namespace TD.Web.Admin.Repository.Mappings
// {
//     public class ProductsGroupsSaveRequestMapping : LSCore<ProductGroupEntity, ProductsGroupsSaveRequest>
//     {
//         public void Map(ProductGroupEntity entity, ProductsGroupsSaveRequest request)
//         {
//             entity.InjectFrom(request);
//             entity.Name = Regex.Replace(entity.Name, @"\s+", " ").Trim();
//         }
//     }
// }