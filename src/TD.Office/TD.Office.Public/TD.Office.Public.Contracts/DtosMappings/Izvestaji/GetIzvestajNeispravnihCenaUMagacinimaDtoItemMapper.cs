using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Dtos;
using TD.Office.Public.Contracts.Dtos.Izvestaji;

namespace TD.Office.Public.Contracts.DtosMappings.Izvestaji;

public class GetIzvestajNeispravnihCenaUMagacinimaDtoItemMapper
	: ILSCoreMapper<List<ReportItemDto>, List<GetIzvestajNeispravnihCenaUMagacinimaDto.Item>>
{
	public List<GetIzvestajNeispravnihCenaUMagacinimaDto.Item> ToMapped(List<ReportItemDto> source)
	{
		var dest = new List<GetIzvestajNeispravnihCenaUMagacinimaDto.Item>();
		foreach (var s in source)
		{
			var d = new GetIzvestajNeispravnihCenaUMagacinimaDto.Item();
			d.InjectFrom(s);
			d.Opis =
				s.ProblemSaCenom != null
					? $"Cena u magacinu je {s.ProblemSaCenom.TrenutnaCena.ToString("0.00")}, a treba da bude {s.ProblemSaCenom.CenaTrebaDaBude.ToString("0.00")}"
					: s.ProblemSaRobom!.Opis;
			dest.Add(d);
		}
		return dest;
	}
}
