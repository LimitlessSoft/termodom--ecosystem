namespace NeslaganjaUInternomTransportu;

public class WhatToCheckFilter
{
	public required bool ImaIzlazniDokument { get; set; }
	public required bool BrojStavki { get; set; }
	public required bool SveStavkeIste { get; set; }
	public required bool Kolicine { get; set; }
	public required bool ProdajnaCenaNula { get; set; }
	public required bool NabavnaCenaNula { get; set; }
}
