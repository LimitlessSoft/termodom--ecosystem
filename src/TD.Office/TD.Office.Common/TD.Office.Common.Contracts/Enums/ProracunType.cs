using System.ComponentModel;

namespace TD.Office.Common.Contracts.Enums;

public enum ProracunType
{
	[Description("MP")]
	Maloprodajni,

	[Description("VP")]
	Veleprodajni,

	[Description("Nalog za utovar")]
	NalogZaUtovar
}
