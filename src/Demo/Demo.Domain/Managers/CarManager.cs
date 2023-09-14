using Demo.Contracts.Entities;
using Demo.Contracts.IManagers;
using Demo.Repository;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;

namespace Demo.Domain.Managers
{
    public class CarManager : ICarManager
    {
        public ListResponse<CarEntity> Get()
        {
            var response = new ListResponse<CarEntity>();
            response.Payload = Baza.Cars;
            return response;
        }

        public Response<CarEntity> Get(IdRequest request)
        {
            var response = new Response<CarEntity>();
            var car = Baza.Cars.FirstOrDefault(x => x.Id == request.Id);

            if(car == null)
                return Response<CarEntity>.NotFound();

            response.Payload = car;
            return response;
        }
    }
}
