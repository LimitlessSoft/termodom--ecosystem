using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class StavkaManager(
	ILogger<StavkaManager> logger,
	KomercijalnoDbContext dbContext,
	IDokumentRepository dokumentRepository,
	IMagacinRepository magacinRepository,
	IRobaRepository robaRepository,
	IStavkaRepository stavkaRepository,
	IProcedureManager procedureManager
) : IStavkaManager
{
	public StavkaDto Create(StavkaCreateRequest request)
	{
		request.Validate();

		var stavka = new Stavka();

		var dokument = dokumentRepository.Get(request.VrDok, request.BrDok);
		var magacin = magacinRepository.Get(dokument.MagacinId);
		var roba = robaRepository.Get(request.RobaId, x => x.Tarifa);

		if (string.IsNullOrWhiteSpace(request.Naziv))
			request.Naziv = roba.Naziv;

		var getCenaNaDanResponse = procedureManager.GetProdajnaCenaNaDan(
			new ProceduraGetProdajnaCenaNaDanRequest
			{
				Datum = DateTime.Now,
				MagacinId = request.CeneVuciIzOvogMagacina ?? dokument.MagacinId,
				RobaId = request.RobaId,
			}
		);

		var prodajnaCenaBezPdvNaDan = getCenaNaDanResponse / ((100d + roba.Tarifa.Stopa) / 100d);

		if (request.ProdajnaCenaBezPdv == null)
			request.ProdajnaCenaBezPdv = prodajnaCenaBezPdvNaDan;

		if (request.ProdajnaCenaBezPdv.Value != prodajnaCenaBezPdvNaDan)
			request.Rabat +=
				((request.ProdajnaCenaBezPdv.Value / prodajnaCenaBezPdvNaDan) - 1) * -100;

		if (double.IsInfinity(request.Rabat) || double.IsNaN(request.Rabat))
			request.Rabat = 0;

		if (request.NabavnaCena == null)
		{
			// TODO:
			// Ovo totalno nema smisla
			// Ne treba prvu stavku da hvata, vec po onoj logici izvuce nabavnu cenu
			//var robaUMagacinu = dbContext.RobaUMagacinu.FirstOrDefault(x =>
			//	x.MagacinId == (request.CeneVuciIzOvogMagacina ?? dokument.MagacinId)
			//	&& x.RobaId == request.RobaId
			//);
			//request.NabavnaCena = robaUMagacinu?.NabavnaCena ?? 0;

			request.NabavnaCena = 0;
		}

		var magacinResponse = dbContext.Magacini.FirstOrDefault(x => x.Id == dokument.MagacinId);
		if (magacinResponse == null)
			throw new LSCoreNotFoundException();

		stavka.InjectFrom(request);

		stavka.Vrsta = roba.Vrsta;
		stavka.MagacinId = dokument.MagacinId;
		stavka.ProdCenaBp = dokument.VrDok == 4 ? 0 : request.ProdajnaCenaBezPdv ?? 0;
		stavka.Rabat = request.Rabat;
		stavka.ProdajnaCena =
			magacin.Vrsta == MagacinVrsta.Veleprodajni
				? prodajnaCenaBezPdvNaDan
				: getCenaNaDanResponse;
		stavka.DevProdCena =
			dokument.VrDok == 4
				? 0
				: (
					magacin.Vrsta == MagacinVrsta.Veleprodajni
						? prodajnaCenaBezPdvNaDan
						: getCenaNaDanResponse
				) / dokument.Kurs;
		stavka.TarifaId = roba.TarifaId;
		stavka.Porez = roba.Tarifa.Stopa;
		stavka.Taksa = 0;
		stavka.Akciza = 0;
		stavka.PorezIzlaz = roba.Tarifa.Stopa;
		stavka.PorezUlaz = dokument.VrDok == 4 ? 0 : roba.Tarifa.Stopa;
		stavka.NabCenSaPor = request.NabCenaSaPor ?? 0;
		stavka.FakturnaCena = request.FakturnaCena ?? 0;
		stavka.NabCenaBt = request.NabCenaBt ?? 0;
		stavka.Troskovi = request.Troskovi ?? 0;
		stavka.Korekcija = request.Korekcija ?? 0;
		stavka.MtId = magacinResponse.MtId;

		stavkaRepository.Insert(stavka);
		return stavka.ToStavkaDto();
	}

	public void DeleteStavke(StavkeDeleteRequest request)
	{
		if (request.RobaId is not null)
		{
			stavkaRepository.DeleteByRobaId(request.VrDok, request.BrDok, request.RobaId.Value);
			return;
		}
		stavkaRepository.Delete(request.VrDok, request.BrDok);
	}

	public List<StavkaDto> GetMultiple(StavkaGetMultipleRequest request)
	{
		var dokumentiFilter = request
			.Dokument?.Select(
				(x) =>
				{
					var split = x.Split('-');
					return new
					{
						VrDok = Convert.ToInt32(split[0]),
						BrDok = Convert.ToInt32(split[1]),
					};
				}
			)
			.ToList();

		var query = dbContext.Stavke.Where(x =>
			(request.VrDok == null || request.VrDok.Length == 0 || request.VrDok.Contains(x.VrDok))
			&& (
				request.MagacinId == null
				|| request.MagacinId.Length == 0
				|| request.MagacinId.Contains(x.MagacinId)
			)
			&& (
				dokumentiFilter == null
				|| !dokumentiFilter.Any()
				|| dokumentiFilter.Any(y => y.VrDok == x.VrDok && y.BrDok == x.BrDok)
			)
		);

		if (request.DokumentFilter != null)
		{
			query
				.Include(x => x.Dokument)
				.Where(x =>
					(
						request.DokumentFilter.PPID == null
						|| request.DokumentFilter.PPID.Length == 0
						|| (
							x.Dokument.PPID != null
							&& request.DokumentFilter.PPID.Any(z => z == x.Dokument.PPID.Value)
						)
					)
					&& (
						request.DokumentFilter.DatumOd == null
						|| x.Dokument.Datum >= request.DokumentFilter.DatumOd
					)
					&& (
						request.DokumentFilter.DatumDo == null
						|| x.Dokument.Datum >= request.DokumentFilter.DatumDo
					)
					&& (
						request.DokumentFilter.Flag == null
						|| x.Dokument.Flag == request.DokumentFilter.Flag
					)
				);
		}
		throw new NotImplementedException();
	}

	public List<StavkaDto> GetMultipleByRobaId(StavkeGetMultipleByRobaId request)
	{
		return dbContext
			.Stavke.Include(x => x.Dokument)
			.Where(x =>
				x.RobaId == request.RobaId
				&& (request.From == null || x.Dokument.Datum.Date >= request.From.Value.Date)
				&& (request.To == null || x.Dokument.Datum.Date <= request.To.Value.Date)
				&& (request.MagacinId == null || x.Dokument.MagacinId == request.MagacinId)
			)
			.ToList()
			.ToStavkaDtoList();
	}
}
