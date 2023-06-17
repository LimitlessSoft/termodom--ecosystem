using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class MagacinManager : BaseManager<MagacinManager, Magacin>, IMagacinManager
    {
        public MagacinManager(ILogger<MagacinManager> logger, KomercijalnoDbContext komercijalnoDbContext)
            : base(logger, komercijalnoDbContext)
        {

        }

        public Response<Magacin> Get(IdRequest request)
        {
            var response = new Response<Magacin>();
            return response;
        }
    }
}
