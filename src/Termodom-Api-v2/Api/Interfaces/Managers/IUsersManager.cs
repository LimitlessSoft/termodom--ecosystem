using Infrastructure.Entities.ApiV2;

namespace Api.Interfaces.Managers
{
    public interface IUsersManager
    {
        public bool Authenticate(string username, string password);
        public IQueryable<User> List();
    }
}
