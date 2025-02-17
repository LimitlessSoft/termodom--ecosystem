using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Izvestaji;

namespace TD.Office.Public.Api.Controllers;

// [LSCoreAuthorize]
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

    [HttpPost]
    [Route("izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima-promeni-nacin-uplate")]
    public async Task<IActionResult> PromeniNacinUplate(
        [FromBody] PromeniNacinUplateRequest request
    )
    {
        await izvestajManager.PromeniNacinUplateAsync(request);
        return Ok();
    }

    [HttpGet]
    [Permissions(Permission.IzvestajIzlazaRobePoGodinamaRead)]
    [Route("izvestaj-izlaza-roba-po-godinama")]
    public async Task<IActionResult> IzvestajIzlazaRobePoGodinamaAsync(
        [FromQuery] GetIzvestajIzlazaRobePoGodinamaRequest request
    ) => Ok(await izvestajManager.GetIzvestajIzlazaRobePoGodinamaAsync(request));
}
