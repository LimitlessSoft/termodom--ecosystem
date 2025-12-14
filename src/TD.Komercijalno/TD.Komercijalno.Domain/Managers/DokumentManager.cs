using LSCore.Exceptions;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class DokumentManager(
	KomercijalnoDbContext dbContext,
	IDokumentRepository dokumentRepository
) : IDokumentManager
{
	public DokumentDto Create(DokumentCreateRequest request)
	{
		request.Validate();

		var poslednjiBrDok = 0;

		var posledjiBrDokZaVrstuZaMagacin = dbContext.VrstaDokMag.FirstOrDefault(x =>
			x.VrDok == request.VrDok && x.MagacinId == request.MagacinId
		);
		if (posledjiBrDokZaVrstuZaMagacin == null)
		{
			var vrstaDokResponse = dbContext.VrstaDok.FirstOrDefault(x => x.Id == request.VrDok);
			if (vrstaDokResponse == null)
				throw new LSCoreNotFoundException();

			poslednjiBrDok = vrstaDokResponse.Poslednji ?? 0;
		}

		var dokument = new Dokument();
		dokument.InjectFrom(request);
		dokument.BrDok = poslednjiBrDok + 1;
		dokument.Kurs = 1;

		if (dokument.Linked == null)
			dokument.Linked = NextLinked(
				new DokumentNextLinkedRequest()
				{
					MagacinId = dokument.MagacinId,
					Datum = DateTime.Now,
				}
			);

		if (dokument.MtId == null)
		{
			var magacin = dbContext.Magacini.FirstOrDefault(x => x.Id == request.MagacinId);
			if (magacin == null)
				throw new LSCoreNotFoundException();

			dokument.MtId = magacin.MtId;
		}

		dokumentRepository.Create(dokument);
		return dokument.ToDokumentDto();
	}

	public DokumentDto Get(DokumentGetRequest request)
	{
		var dokument = dbContext
			.Dokumenti.Include(x => x.Stavke)
			.FirstOrDefault(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);
		if (dokument == null)
			throw new LSCoreNotFoundException();

		return dokument.ToDokumentDto();
	}

	public List<DokumentDto> GetMultiple(DokumentGetMultipleRequest request)
	{
		return dbContext
			.Dokumenti.Where(x =>
				(
					request.VrDok == null
					|| request.VrDok.Length == 0
					|| request.VrDok.Contains(x.VrDok)
				)
				&& (string.IsNullOrWhiteSpace(request.IntBroj) || x.IntBroj == request.IntBroj)
				&& (!request.KodDok.HasValue || x.KodDok == request.KodDok.Value)
				&& (!request.Flag.HasValue || x.Flag == request.Flag.Value)
				&& (!request.DatumOd.HasValue || x.Datum >= request.DatumOd.Value)
				&& (!request.DatumDo.HasValue || x.Datum <= request.DatumDo.Value)
				&& (string.IsNullOrWhiteSpace(request.Linked) || x.Linked == request.Linked)
				&& (!request.MagacinId.HasValue || x.MagacinId == request.MagacinId.Value)
				&& (
					request.NUID == null
					|| request.NUID.Length == 0
					|| x.NuId != null && request.NUID.Contains(x.NuId.Value)
				)
				&& (
					request.PPID == null
					|| request.PPID.Length == 0
					|| (x.PPID != null && request.PPID.Any(z => z == x.PPID.Value))
				)
			)
			.Include(x => x.Stavke)
			.ToList()
			.ToDokumentListDto();
	}

	public string NextLinked(DokumentNextLinkedRequest request)
	{
		var maxLinkedDokument = dbContext
			.Dokumenti.Where(x =>
				x.MagacinId == request.MagacinId
				&& (string.IsNullOrWhiteSpace(x.Linked) || x.Linked != "9999999999")
			)
			.OrderBy(x => Convert.ToDouble(x.Linked))
			.FirstOrDefault();

		return maxLinkedDokument == null
			? "0000000000"
			: Convert.ToDouble(maxLinkedDokument.Linked).ToString("0000000000");
	}

	public void SetNacinPlacanja(DokumentSetNacinPlacanjaRequest request)
	{
		var dokument = dbContext.Dokumenti.FirstOrDefault(x =>
			x.VrDok == request.VrDok && x.BrDok == request.BrDok
		);

		if (dokument == null)
			throw new LSCoreNotFoundException();

		dokument.NuId = request.NUID;

		dokumentRepository.Update(dokument);
	}

	public void SetDokOut(DokumentSetDokOutRequest request)
	{
		var dokument = dbContext.Dokumenti.FirstOrDefault(x =>
			x.VrDok == request.VrDok && x.BrDok == request.BrDok
		);

		if (dokument == null)
			throw new LSCoreNotFoundException();

		dokument.VrdokOut = request.VrDokOut;
		dokument.BrdokOut = request.BrDokOut;

		dokumentRepository.Update(dokument);
	}

	public void SetFlag(DokumentSetFlagRequest request)
	{
		var dokument = dbContext.Dokumenti.FirstOrDefault(x =>
			x.VrDok == request.VrDok && x.BrDok == request.BrDok
		);

		if (dokument == null)
			throw new LSCoreNotFoundException();

		dokument.Flag = request.Flag;

		dokumentRepository.Update(dokument);
	}
}
