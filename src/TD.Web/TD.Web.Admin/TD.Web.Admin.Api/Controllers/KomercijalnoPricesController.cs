using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class KomercijalnoPricesController : Controller
    {
        private readonly IKomercijalnoPriceManager _komercijalnoPriceManager;
        public KomercijalnoPricesController(IKomercijalnoPriceManager komercijalnoPriceManager)
        {
            _komercijalnoPriceManager = komercijalnoPriceManager;
        }

        [HttpGet]
        [Route("/komercijalno-prices")]
        public LSCoreListResponse<KomercijalnoPriceGetDto> GetMultiple() =>
            _komercijalnoPriceManager.GetMultiple();
    }
}
