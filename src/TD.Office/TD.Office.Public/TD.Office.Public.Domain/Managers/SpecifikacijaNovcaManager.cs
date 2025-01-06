using LSCore.Contracts;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Domain.Managers;
public class SpecifikacijaNovcaManager(
    ILogger<SpecifikacijaNovcaManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ISpecifikacijaNovcaRepository specifikacijaNovcaRepository
)
    : LSCoreManagerBase<SpecifikacijaNovcaManager, SpecifikacijaNovcaEntity>(
        logger,
        dbContext,
        currentUser
    ),
    ISpecifikacijaNovcaManager
{
    public GetSpecifikacijaNovcaDto GetCurrent()
    {

        throw new NotImplementedException();
    }

    public GetSpecifikacijaNovcaDto GetNext(GetNextSpecifikacijaNovcaRequest request)
    {
        throw new NotImplementedException();
    }

    public GetSpecifikacijaNovcaDto GetPrev(GetPrevSpecifikacijaNovcaRequest request)
    {
        throw new NotImplementedException();
    }

    public GetSpecifikacijaNovcaDto GetSingle(GetSingleSpecifikacijaNovcaRequest request)
    {
        throw new NotImplementedException();
    }

    public void Save(SaveSpecifikacijaNovcaRequest request)
    {
        throw new NotImplementedException();
    }
}
