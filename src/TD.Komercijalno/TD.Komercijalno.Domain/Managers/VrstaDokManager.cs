using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.DtoMappings.VrstaDoks;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class VrstaDokManager : BaseManager<VrstaDokManager, VrstaDok>, IVrstaDokManager
    {
        public VrstaDokManager(ILogger<VrstaDokManager> logger, KomercijalnoDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<VrstaDokDto> GetMultiple()
        {
            return new ListResponse<VrstaDokDto>(Queryable().ToList().ToVrstaDokDtoList());
        }
    }
}
