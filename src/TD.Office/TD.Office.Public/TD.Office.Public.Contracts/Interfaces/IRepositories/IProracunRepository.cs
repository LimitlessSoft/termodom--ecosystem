using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IProracunRepository
{
    /// <summary>
    /// Gets proracun by id. If not found or isActive == false, throw NotFound.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ProracunEntity Get(long id);
}
