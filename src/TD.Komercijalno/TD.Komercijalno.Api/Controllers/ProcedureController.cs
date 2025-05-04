using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Api.Controllers
{
	[ApiController]
	public class ProcedureController(IProcedureManager procedureManager) : Controller
	{
		[HttpGet]
		[Route("/procedure/prodajna-cena-na-dan")]
		public double ProdajnaCenaNaDan([FromQuery] ProceduraGetProdajnaCenaNaDanRequest request) =>
			procedureManager.GetProdajnaCenaNaDan(request);

		[HttpGet]
		[Route("/procedure/prodajna-cena-na-dan-optimized")]
		public List<ProdajnaCenaNaDanDto> ProdajnaCenaNaDanOptimized(
			[FromQuery] ProceduraGetProdajnaCenaNaDanOptimizedRequest request
		) => procedureManager.GetProdajnaCenaNaDanOptimized(request);

		[HttpGet]
		[Route("/procedure/nabavna-cena-na-dan")]
		public List<NabavnaCenaNaDanDto> NabavnaCenaNaDan(
			[FromQuery] ProceduraGetNabavnaCenaNaDanRequest request
		) => procedureManager.GetNabavnaCenaNaDan(request);
	}
}
