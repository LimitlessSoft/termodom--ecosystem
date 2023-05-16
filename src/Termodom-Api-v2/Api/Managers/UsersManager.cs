using Api.Interfaces.Managers;
using Infrastructure.Entities.ApiV2;
using System.Security.Cryptography;
using System.Text;

namespace Api.Managers
{
    public class UsersManager : IUsersManager
    {
        private readonly ApiDbContext _dbContext;

        public UsersManager(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Password Hash
        private static string SimpleHash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        private static string HashPassword(string RawPassword)
        {
            return SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(RawPassword))))));
        }
        #endregion

        /// <summary>
        /// Authenticates username and password against database
        /// </summary>
        /// <returns>True if authenticated, false if not</returns>
        public bool Authenticate(string username, string password)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == HashPassword(password)) != null;
        }

        public IQueryable<User> List()
        {
            return _dbContext.Users.AsQueryable();
        }
    }
}
