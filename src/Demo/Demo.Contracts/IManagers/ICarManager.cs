using Demo.Contracts.Entities;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;

namespace Demo.Contracts.IManagers
{
    public interface ICarManager
    {
        ListResponse<CarEntity> Get();
        Response<CarEntity> Get(IdRequest request);
    }
}
