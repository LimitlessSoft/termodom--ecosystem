using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;

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
        public LSCoreResponse<double> ProdajnaCenaNaDan([FromQuery] ProceduraGetProdajnaCenaNaDanRequest request) =>
            _procedureManager.GetProdajnaCenaNaDan(request);

        [HttpGet]
        [Route("/procedure/nabavna-cena-na-dan")]
        public LSCoreListResponse<NabavnaCenaNaDanDto> NabavnaCenaNaDan([FromQuery] ProceduraGetNabavnaCenaNaDanRequest request) =>
            _procedureManager.GetNabavnaCenaNaDan(request);
    }
}
