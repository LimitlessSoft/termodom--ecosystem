using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly IProcedureManager _procedureManager;

        public ProcedureController(IProcedureManager procedureManager)
        {
            _procedureManager = procedureManager;
        }

        [HttpGet]
        [Route("/procedure/prodajna-cena-na-dan")]
        public LSCoreResponse<double> ProdajnaCenaNaDan([FromQuery] ProceduraGetProdajnaCenaNaDanRequest request)
        {
            return _procedureManager.GetProdajnaCenaNaDan(request);
        }
    }
}
