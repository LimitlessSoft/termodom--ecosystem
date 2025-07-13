using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

namespace TD.Office.Public.Contracts.DtosMappings.SpecifikacijaNovca;

public class GetSpecifikacijaNovcaDtoMappings
	: ILSCoreMapper<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>
{
	public GetSpecifikacijaNovcaDto ToMapped(SpecifikacijaNovcaEntity sender)
	{
		var dto = new GetSpecifikacijaNovcaDto();
		dto.InjectFrom(sender);
		dto.Id = sender.Id;
		dto.MagacinId = sender.MagacinId;
		dto.DatumUTC = sender.Datum;
		dto.SpecifikacijaNovca.Eur1 = new SpecifikacijaNovcaDetailsDto.EurDto()
		{
			Komada = sender.Eur1Komada,
			Kurs = sender.Eur1Kurs,
		};
		dto.SpecifikacijaNovca.Eur2 = new SpecifikacijaNovcaDetailsDto.EurDto()
		{
			Komada = sender.Eur2Komada,
			Kurs = sender.Eur2Kurs,
		};
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 1,
			Value = sender.Novcanica1Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 2,
			Value = sender.Novcanica2Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 5,
			Value = sender.Novcanica5Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 10,
			Value = sender.Novcanica10Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 20,
			Value = sender.Novcanica20Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 50,
			Value = sender.Novcanica50Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 100,
			Value = sender.Novcanica100Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 200,
			Value = sender.Novcanica200Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 500,
			Value = sender.Novcanica500Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 1000,
			Value = sender.Novcanica1000Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 2000,
			Value = sender.Novcanica2000Komada,
		});
		dto.SpecifikacijaNovca.Novcanice.Add(new SpecifikacijaNovcaDetailsDto.NovcanicaDto()
		{
			Key = 5000,
			Value = sender.Novcanica5000Komada,
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Kartice",
			Vrednost = sender.Kartice,
			Komentar = sender.KarticeKomentar
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Cekovi",
			Vrednost = sender.Cekovi,
			Komentar = sender.CekoviKomentar
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Papiri",
			Vrednost = sender.Papiri,
			Komentar = sender.PapiriKomentar
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Troskovi",
			Vrednost = sender.Troskovi,
			Komentar = sender.TroskoviKomentar
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Vozaci",
			Vrednost = sender.Vozaci,
			Komentar = sender.VozaciKomentar
		});
		dto.SpecifikacijaNovca.Ostalo.Add(new SpecifikacijaNovcaDetailsDto.OstaloDto()
		{
			Key = "Sasa",
			Vrednost = sender.Sasa,
			Komentar = sender.SasaKomentar
		});
		return dto;
	}
}
