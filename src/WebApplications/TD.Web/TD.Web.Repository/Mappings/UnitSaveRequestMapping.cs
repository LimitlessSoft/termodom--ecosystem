using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Helpers.Units;
using TD.Web.Contracts.Requests.Units;

namespace TD.Web.Repository.Mappings
{
    public class UnitSaveRequestMapping : IMap<UnitEntity, UnitSaveRequest>
    {
        public void Map(UnitEntity entity, UnitSaveRequest request)
        {
            entity.InjectFrom(request);
            entity.Name = entity.Name.NormalizeName();
        }
    }
}
