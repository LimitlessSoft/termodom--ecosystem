using LSCore.Contracts.Interfaces.Repositories;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IProracunRepository : ILSCoreRepositoryBase<ProracunEntity>
{
    void UpdateState(long requestId, ProracunState requestState);
    void UpdatePPID(long requestId, int? requestPpid);
    void UpdateNUID(long requestId, int requestNuid);
}
