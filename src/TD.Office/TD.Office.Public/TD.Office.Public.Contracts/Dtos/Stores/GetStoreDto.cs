using TD.Komercijalno.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Stores;

public class GetStoreDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public MagacinVrsta Vrsta { get; set; }
}
