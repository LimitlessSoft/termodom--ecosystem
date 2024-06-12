using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using Omu.ValueInjecter;

namespace TD.Web.Admin.Domain.Managers;

public class KomercijalnoWebProductLinkManager (
    ILogger<KomercijalnoWebProductLinkManager> logger,
    WebDbContext dbContext)
    : LSCoreManagerBase<KomercijalnoWebProductLinkManager, KomercijalnoWebProductLinkEntity>(logger, dbContext),
        IKomercijalnoWebProductLinkManager
{
    public List<KomercijalnoWebProductLinksGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<KomercijalnoWebProductLinkEntity, KomercijalnoWebProductLinksGetDto>();

    public KomercijalnoWebProductLinksGetDto Put(KomercijalnoWebProductLinksSaveRequest request)
    {
        var savedEntity = Save(request);
        var dto = new KomercijalnoWebProductLinksGetDto();
        dto.InjectFrom(savedEntity);
        return dto;
    }
}