using LSCore.Contracts;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Managers;

public class ProracunManager(
    ILogger<ProracunManager> logger,
    IUserManager userManager,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser
)
    : LSCoreManagerBase<ProracunManager, ProracunEntity>(logger, dbContext, currentUser),
        IProracunManager
{
    public ProracunDto Create(ProracuniCreateRequest request)
    {
        var userEntity = userManager.GetSingle(
            new LSCoreIdRequest() { Id = currentUser.Id!.Value }
        );
    }
}
