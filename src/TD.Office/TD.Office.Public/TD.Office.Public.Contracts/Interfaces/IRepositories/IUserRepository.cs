using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IUserRepository
{
    /// <summary>
    /// Returns currently authenticated user's entity.
    /// If user is not authenticated, throws unauthenticated.
    /// If user is authenticated, but not found in database or is found but isActive = false, throws not found.
    /// </summary>
    /// <returns></returns>
    UserEntity GetCurrentUser();
    UserEntity Get(long id);
    UserEntity? GetOrDefault(long id);
    void Update(UserEntity currentUser);
    IQueryable<UserEntity> GetMultiple();
    void UpdateNickname(long requestId, string requestNickname);
    void Create(UserEntity entity);
}
