using System.ComponentModel;
using TD.Office.Common.Contracts.Attributes;

namespace TD.Office.Common.Contracts.Enums;

public enum Permission
{
	[Description("Pristup aplikaciji")]
	Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - pregled")]
	NalogZaPrevozRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - rad sa svim magacinima")]
	NalogZaPrevozRadSaSvimMagacinima,

	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - novi")]
	NalogZaPrevozNovi,

	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - prethodni datumi")]
	NalogZaPrevozPrethodniDatumi,

	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - stampa izvestaja")]
	NalogZaPrevozStampaIzvestaja,

	[PermissionGroup(LegacyConstants.PermissionGroup.NalogZaPrevoz)]
	[Description("Nalog za prevoz - stampa pojedinacnog naloga")]
	NalogZaPrevozStampaPojedinacnogNaloga,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Korisnici)]
	[Description("Korisnici - pregled")]
	KorisniciRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Web)]
	[Description("Web - pregled")]
	WebRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Partneri)]
	[Description("Partneri - pregled")]
	PartneriRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.Partneri)]
	[Description("Partneri - vidi mobilni")]
	PartneriVidiMobilni,

	[PermissionGroup(LegacyConstants.PermissionGroup.Partneri)]
	[Description("Partneri - skoro kreirani")]
	PartneriSkoroKreirani,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - pregled")]
	SpecifikacijaNovcaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - pregled svih magacina")]
	SpecifikacijaNovcaSviMagacini,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - pregled prethodnih 7 dana")]
	SpecifikacijaNovcaPrethodnih7Dana,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - pregled svih datuma")]
	SpecifikacijaNovcaSviDatumi,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - pretraga po broju")]
	SpecifikacijaNovcaPretragaPoBroju,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - sacuvaj")]
	SpecifikacijaNovcaSave,

	[PermissionGroup(LegacyConstants.PermissionGroup.SpecifikacijaNovca)]
	[Description("Specifikacija Novca - stampa izvestaja")]
	SpecifikacijaNovcaPrint,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.PartnerIzvestajFinansijskoKomercijalno)]
	[Description(
		"Partneri - izvestaj stanja po godinama finansijsko i komercijalno - pristup modulu"
	)]
	PartneriKomercijalnoFinansijskoPoGodinamaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(
		LegacyConstants.PermissionGroup.IzvestajUkupneKolicinePoRobiUFiltriranimDokumentima
	)]
	[Description("Izvestaji - Izveštaj ukupne količine u dokumentima po robi")]
	IzvestajUkupneKolicinePoRobiUFiltriranimDokumentimaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - pristup modulu")]
	ProracuniRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - pregled svih magacina")]
	ProracuniReadSviMagacini,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - pregled starijih od 7 dana")]
	ProracuniReadStarijiOd7Dana,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - kreira novi MP")]
	ProracuniNewMp,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - kreira novi VP")]
	ProracuniNewVp,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - zakljucavanje dokumenta")]
	ProracuniLock,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - otkljucavanje dokumenta")]
	ProracuniUnlock,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - MP pretvori u komercijalno")]
	ProracuniMPForwardToKomercijalno,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - moze da daje rabat")]
	ProracuniRabat,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - kreira novi nalog za utovar")]
	ProracuniNewNalogZaUtovar,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.IzvestajIzlazaRobePoGodinama)]
	[Description("Prihod po centrima - Izvestaj izlaza robe po godinama - pristup modulu")]
	IzvestajIzlazaRobePoGodinamaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.PartnerAnaliza)]
	[Description("Partner analiza - pristup modulu")]
	PartnerAnalizaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - VP pretvori u komercijalno")]
	ProracuniVPForwardToKomercijalno,

	[PermissionGroup(LegacyConstants.PermissionGroup.Proracuni)]
	[Description("Proracuni - Nalog za utovar pretvori u komercijalno")]
	ProracuniNalogZaUtovarForwardToKomercijalno,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Otpremnice)]
	[Description("Otpremnice - pristup modulu")]
	OtpremniceRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.Otpremnice)]
	[Description("Otpremnice - zakljucavanje dokumenta")]
	OtpremniceLock,

	[PermissionGroup(LegacyConstants.PermissionGroup.Otpremnice)]
	[Description("Otpremnice - otkljucavanje dokumenta")]
	OtpremniceUnlock,

	[PermissionGroup(LegacyConstants.PermissionGroup.Otpremnice)]
	[Description("Otpremnice - rad sa svim magacinima")]
	OtpremniceRadSaSvimMagacinima,

	[PermissionGroup(LegacyConstants.PermissionGroup.IzvestajNeispravnihCenaUMagacinima)]
	[Description("Neispravne cene - izvestaj")]
	IzvestajNeispravnihCenaUMagacinimaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - pristup modulu")]
	RobaPopisRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - filter 7 dana unazad")]
	RobaPopisFilter7DanaUnazad,

	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - filter svi datumi")]
	RobaPopisFilterSviDatumi,

	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - zakljucaj dokument")]
	RobaPopisFilterLock,

	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - otkljucaj dokument")]
	RobaPopisFilterUnlock,

	[PermissionGroup(LegacyConstants.PermissionGroup.RobaPopis)]
	[Description("Roba popis - storniraj dokument")]
	RobaPopisFilterStorniraj,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.MasovniSMS)]
	[Description("Masovni SMS - pristup modulu")]
	MasovniSMSRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.KalendarAktivnosti)]
	[Description("Kalendar aktivnosti - pregled")]
	KalendarAktivnostiRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.KalendarAktivnosti)]
	[Description("Kalendar aktivnosti - unos sopstvenih odsustva")]
	KalendarAktivnostiWrite,

	[PermissionGroup(LegacyConstants.PermissionGroup.KalendarAktivnosti)]
	[Description("Kalendar aktivnosti - izmena tuđih odsustva")]
	KalendarAktivnostiEditAll,

	[PermissionGroup(LegacyConstants.PermissionGroup.KalendarAktivnosti)]
	[Description("Kalendar aktivnosti - odobravanje odsustva")]
	KalendarAktivnostiApprove,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.TipOdsustva)]
	[Description("Tipovi odsustva - pregled")]
	TipOdsustvaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.TipOdsustva)]
	[Description("Tipovi odsustva - upravljanje")]
	TipOdsustvaWrite,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Korisnici)]
	[PermissionGroup(LegacyConstants.PermissionGroup.KorisniciList)]
	[Description("Korisnici lista - pregled")]
	KorisniciListRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Korisnici)]
	[PermissionGroup(LegacyConstants.PermissionGroup.TipKorisnika)]
	[Description("Tipovi korisnika - pregled")]
	TipKorisnikaRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.TipKorisnika)]
	[Description("Tipovi korisnika - upravljanje")]
	TipKorisnikaWrite,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Dashboard)]
	[Description("Kontrolna tabla - pregled")]
	DashboardRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.ModuleHelp)]
	[Description("Pomoć modula - izmena FAQ")]
	ModuleHelpSystemWrite,

	[PermissionGroup(LegacyConstants.PermissionGroup.KalendarAktivnosti)]
	[Description("Kalendar aktivnosti - brisanje odsustva")]
	KalendarAktivnostiDelete,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - pregled")]
	TicketsRead,

	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - kreiranje bug prijave")]
	TicketsCreateBug,

	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - kreiranje zahteva za funkcionalnost")]
	TicketsCreateFeature,

	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - upravljanje prioritetom")]
	TicketsManagePriority,

	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - upravljanje statusom")]
	TicketsManageStatus,

	[PermissionGroup(LegacyConstants.PermissionGroup.Tickets)]
	[Description("Tiketi - developer napomene")]
	TicketsDeveloperNotes,
}
