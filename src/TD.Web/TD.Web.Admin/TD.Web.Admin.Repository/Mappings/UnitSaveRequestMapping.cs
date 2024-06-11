// using LSCore.Contracts.Interfaces;
// using Omu.ValueInjecter;
// using TD.Web.Admin.Contracts.Helpers.Units;
// using TD.Web.Admin.Contracts.Requests.Units;
// using TD.Web.Common.Contracts.Entities;
//
// namespace TD.Web.Admin.Repository.Mappings
// {
//     public class UnitSaveRequestMapping : ILSCoreMap<UnitEntity, UnitSaveRequest>
//     {
//         public void Map(UnitEntity entity, UnitSaveRequest request)
//         {
//             entity.InjectFrom(request);
//             entity.Name = entity.Name.NormalizeName();
//         }
//     }
// }
