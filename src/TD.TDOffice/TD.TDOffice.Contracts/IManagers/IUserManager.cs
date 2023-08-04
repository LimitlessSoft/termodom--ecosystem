using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.Users;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IUserManager
    {
        public Response<UserDto> Get(IdRequest request);
        public ListResponse<UserDto> GetMultiple();
    }
}
