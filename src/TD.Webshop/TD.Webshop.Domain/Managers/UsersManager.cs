using Microsoft.EntityFrameworkCore;
using TD.Webshop.Contracts.Entities;
using TD.Webshop.Contracts.IManagers;
using TD.Webshop.Repository;

namespace TD.Webshop.Domain.Managers
{
    public class UsersManager : IUsersManager
    {
        private readonly WebshopDbContext _dbContext;

        public UsersManager(WebshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.Users.Add(new User()
            {
                Username = "Test User",
                Password = "Test password",
                Firstname = "Test"
            });

            _dbContext.SaveChanges();

        }


    }
}
