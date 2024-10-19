using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Izvestaji;

namespace TD.Office.Public.Api.Controllers;

// [Authorize]
[ApiController]
public class IzvestajiController(IIzvestajManager izvestajManager) : ControllerBase
{
    [HttpGet]
    [Route("izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima")]
    public async Task<IActionResult> GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentima(
        [FromQuery] GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    ) =>
        Ok(
            await izvestajManager.GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
                request
            )
        );

    [HttpPost]
    [Route("izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima-izvezi")]
    public async Task<IActionResult> ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentima(
        [FromBody] ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    )
    {
        await izvestajManager.ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
            request
        );
        return Ok();
    }
}
