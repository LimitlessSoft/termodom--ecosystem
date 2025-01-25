using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IProracunRepository
{
    /// <summary>
    /// Gets proracun by id. If not found or isActive == false, throw NotFound.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ProracunEntity Get(long id);

    void Insert(ProracunEntity proracunEntity);
    IQueryable<ProracunEntity> GetMultiple();
    void UpdateState(long requestId, ProracunState requestState);
    void UpdatePPID(long requestId, int? requestPpid);
    void UpdateNUID(long requestId, int requestNuid);
    void Update(ProracunEntity proracun);
    void HardDelete(long requestId);
}
