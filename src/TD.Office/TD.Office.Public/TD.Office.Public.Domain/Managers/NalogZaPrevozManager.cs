using LSCore.Common.Contracts;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Domain.Managers
{
	public class NalogZaPrevozManager(
		INalogZaPrevozRepository nalogZaPrevozRepository,
		ITDKomercijalnoApiManager komercijalnoApiManager
	) : INalogZaPrevozManager
	{
		public void SaveNalogZaPrevoz(SaveNalogZaPrevozRequest request)
		{
			request.Validate();
			var entity = request.Id.HasValue
				? nalogZaPrevozRepository.Get(request.Id.Value)
				: new NalogZaPrevozEntity();

			entity.InjectFrom(request);
			if (request.Id.HasValue)
			{
				entity.Id = request.Id.Value;
				nalogZaPrevozRepository.Update(entity);
			}
			else
				nalogZaPrevozRepository.Insert(entity);
		}

		public async Task<GetReferentniDokumentNalogZaPrevozDto> GetReferentniDokumentAsync(
			GetReferentniDokumentNalogZaPrevozRequest request
		)
		{
			var dokument = await komercijalnoApiManager.GetDokumentAsync(
				new DokumentGetRequest { VrDok = request.VrDok, BrDok = request.BrDok }
			);

			var stavkePrevoza = dokument
				.Stavke!.Where(x => x.Naziv!.ToLower().Contains("prevoz"))
				.ToList();

			return new GetReferentniDokumentNalogZaPrevozDto
			{
				Datum = dokument.Datum,
				Zakljucan = dokument.Flag == 1,
				VrednostStavkePrevozaBezPdv =
					stavkePrevoza.Count > 0
						? (decimal)
							stavkePrevoza.Sum(x =>
								x.ProdajnaCena
								* (100 + x.Rabat)
								/ 100
								* x.Kolicina
								* (request.VrDok == 13 ? 1 : 0.8333334)
							)
						: null,
				PlacenVirmanom = stavkePrevoza.Count > 0
			};
		}

		public List<GetNalogZaPrevozDto> GetMultiple(GetMultipleNalogZaPrevozRequest request) =>
			nalogZaPrevozRepository
				.GetMultiple()
				.Where(x =>
					x.CreatedAt.Date >= request.DateFrom.Date
					&& x.CreatedAt.Date <= request.DateTo.Date
					&& x.StoreId == request.StoreId
				)
				.ToMappedList<NalogZaPrevozEntity, GetNalogZaPrevozDto>();

		public GetNalogZaPrevozDto GetSingle(LSCoreIdRequest request) =>
			nalogZaPrevozRepository
				.Get(request.Id)
				.ToMapped<NalogZaPrevozEntity, GetNalogZaPrevozDto>();
	}
}
