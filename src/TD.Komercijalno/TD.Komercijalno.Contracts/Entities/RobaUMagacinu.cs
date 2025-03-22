using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TD.Komercijalno.Contracts.Entities;

[Keyless]
[Table("ROBAUMAGACINU")]
public class RobaUMagacinu
{
	[Column("MAGACINID")]
	public int MagacinId { get; set; }

	[Column("ROBAID")]
	public int RobaId { get; set; }

	[Column("POZICIJAID")]
	public int? PozicijaId { get; set; }

	[Column("NABAVNACENA")]
	public double NabavnaCena { get; set; }

	[Column("PRODAJNACENA")]
	public double ProdajnaCena { get; set; }

	[Column("DEVNABCENA")]
	public double DeviznaNabavnaCena { get; set; }

	[Column("DEVIZNACENA")]
	public double DeviznaProdajnaCena { get; set; }

	[Column("STANJE")]
	public double Stanje { get; set; }

	[Column("NARUCENO")]
	public double Naruceno { get; set; }

	[Column("REZERVISANO")]
	public double Rezervisano { get; set; }

	[Column("STANJEPOOTP")]
	public double StanjePoOtpremnici { get; set; }

	[Column("EVIDSTANJE")]
	public double EvidentiranoStanje { get; set; }

	[Column("OPTZAL")]
	public double OptimalnaZaliha { get; set; }

	[Column("STANJEPOSER")]
	public double? StanjePoSerijama { get; set; }

	[Column("KRITZAL")]
	public double KriticnaZaliha { get; set; }

	[Column("MESPROSPROD")]
	public double MesecniProsekProdaje { get; set; }

	[Column("STANJEPOREKLAM")]
	public double StanjePoReklamacijama { get; set; }

	[Column("STANJEPOREVERSU")]
	public double StanjePoReversu { get; set; }

	[Column("WMS_STANJE")]
	public double? WmsStanje { get; set; }
}
