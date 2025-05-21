using LSCore.Auth.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Public.Client;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Admin.Contracts;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Enums;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using Constants = TD.Common.Environments.Constants;

namespace TD.Web.Admin.Domain.Managers;

public class OrderManager(
	IKomercijalnoWebProductLinkRepository komercijalnoWebProductLinkRepository,
	IOfficeServerApiManager officeServerApiManager,
	IOrderRepository repository,
	IUserRepository userRepository,
	ISettingRepository settingRepository,
	ITDKomercijalnoClientFactory komercijalnoClientFactory,
	TDOfficeClient officeClient,
	LSCoreAuthContextEntity<string> contextEntity,
	IConfigurationRoot configurationRoot
) : IOrderManager
{
	public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(
		OrdersGetMultipleRequest request
	) =>
		repository
			.GetMultiple()
			.Where(x =>
				(request.Status == null || request.Status.Contains(x.Status))
				&& (request.UserId == null || x.CreatedBy == request.UserId.Value)
			)
			.Include(x => x.User)
			.ThenInclude(x => x.ProductPriceGroupLevels)
			.ThenInclude(x => x.ProductPriceGroup)
			.Include(x => x.OrderOneTimeInformation)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.ToSortedAndPagedResponse<OrderEntity, OrdersSortColumnCodes.Orders, OrdersGetDto>(
				request,
				OrdersSortColumnCodes.OrdersSortRules,
				x => x.ToMapped<OrderEntity, OrdersGetDto>()
			);

	public OrdersGetDto GetSingle(OrdersGetSingleRequest request)
	{
		var order = repository
			.GetMultiple()
			.Where(x => x.OneTimeHash == request.OneTimeHash)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.Include(x => x.OrderOneTimeInformation)
			.Include(x => x.Referent)
			.Include(x => x.User)
			.ThenInclude(x => x.ProductPriceGroupLevels)
			.ThenInclude(x => x.ProductPriceGroup)
			.FirstOrDefault();

		if (order == null)
			throw new LSCoreNotFoundException();

		return order.ToMapped<OrderEntity, OrdersGetDto>();
	}

	public void PutStoreId(OrdersPutStoreIdRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
		if (order == null)
			throw new LSCoreNotFoundException();

		order.StoreId = request.StoreId;
		repository.Update(order);
	}

	public void PutStatus(OrdersPutStatusRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);

		if (order == null)
			throw new LSCoreNotFoundException();

		order.Status = request.Status;
		repository.Update(order);
	}

	public void PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);

		if (order == null)
			throw new LSCoreNotFoundException();

		order.PaymentTypeId = request.PaymentTypeId;
		repository.Update(order);
	}

	public async Task PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request)
	{
		request.Validate();

		var order = repository
			.GetMultiple()
			.Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
			.Include(x => x.Store)
			.Include(x => x.Items)
			.Include(x => x.PaymentType)
			.Include(x => x.User)
			.Include(x => x.OrderOneTimeInformation)
			.FirstOrDefault();

		if (order == null)
			throw new LSCoreNotFoundException();

		var komercijalnoWebProductLinks = komercijalnoWebProductLinkRepository.GetMultiple();

		var vrDok = request.Type switch
		{
			ForwardToKomercijalnoType.Proracun => 32,
			ForwardToKomercijalnoType.Ponuda => 34,
			ForwardToKomercijalnoType.Profaktura => 4,
			ForwardToKomercijalnoType.InternaOtpremnica => 19,
			_ => throw new ArgumentOutOfRangeException(nameof(request.Type))
		};

		var magacinId = request.Type switch
		{
			ForwardToKomercijalnoType.Proracun => order.StoreId,
			ForwardToKomercijalnoType.Ponuda => order.StoreId,
			ForwardToKomercijalnoType.InternaOtpremnica => order.StoreId,
			ForwardToKomercijalnoType.Profaktura
				=> order.Store!.VPMagacinId
					?? throw new LSCoreBadRequestException(
						"Store doesn't have VPMagacin connected"
					),
			_ => throw new ArgumentOutOfRangeException(nameof(request.Type))
		};

		var komercijalnoFirmaMagacin = await officeClient.KomercijalnoMagacinFirma.Get(
			(int)magacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(
				configurationRoot[Constants.DeployVariable]!
			),
			komercijalnoFirmaMagacin.ApiFirma
		);
		#region Create document in Komercijalno
		var komercijalnoDokument = await client.Dokumenti.CreateAsync(
			new KomercijalnoApiDokumentiCreateRequest()
			{
				VrDok = vrDok,
				MagacinId = (short)magacinId,
				ZapId = 107,
				RefId = 107,
				IntBroj = "Web: " + request.OneTimeHash[..8],
				Flag = 0,
				MagId = request.Type switch
				{
					ForwardToKomercijalnoType.InternaOtpremnica => request.DestinacioniMagacinId,
					_ => null
				},
				KodDok = 0,
				Linked = "0000000000",
				PPID = null,
				Placen = 0,
				NuId = (short)order.PaymentType.KomercijalnoNUID,
				NrId = 1,
			}
		);
		#endregion

		#region Update komercijalno dokument komentari
		await client.Komentari.CreateAsync(
			new CreateKomentarRequest()
			{
				BrDok = komercijalnoDokument.BrDok,
				VrDok = komercijalnoDokument.VrDok,
				Komentar =
					$"Porudžbina kreirana uz pomoć www.termodom.rs profi kutka.\r\n\r\nPorudžbina id: {request.OneTimeHash}\r\n\r\nSkraćeni id: {request.OneTimeHash[..8]}\r\n===============\r\nJavni komentar: {order.PublicComment}",
				InterniKomentar =
					order.OrderOneTimeInformation != null
						? $"Ovo je jednokratna kupovina\r\nKupac je ostavio kontakt: {order.OrderOneTimeInformation.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061\r\n===============\r\nAdmin komentar: {order.AdminComment}"
						: $"KupacId: {order.User.Id}\r\nKupac: {order.User.Nickname}({order.User.Username})\r\nKupac ostavio kontakt: {order.User.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061\r\n===============\r\nAdmin komentar: {order.AdminComment}"
			}
		);
		#endregion

		order.KomercijalnoVrDok = komercijalnoDokument.VrDok;
		order.KomercijalnoBrDok = komercijalnoDokument.BrDok;
		order.Status = OrderStatus.WaitingCollection;
		repository.Update(order);

		#region Insert items into komercijalno dokument
		foreach (var orderItemEntity in order.Items)
		{
			var link = komercijalnoWebProductLinks.FirstOrDefault(x =>
				x.WebId == orderItemEntity.ProductId
			);
			if (link == null)
				throw new LSCoreBadRequestException(
					$"Product {orderItemEntity.Product.Name} not linked to Komercijalno."
				);

			await client.Stavke.CreateAsync(
				new StavkaCreateRequest
				{
					VrDok = komercijalnoDokument.VrDok,
					BrDok = komercijalnoDokument.BrDok,
					RobaId = link.RobaId,
					Kolicina = Convert.ToDouble(orderItemEntity.Quantity),
					ProdajnaCenaBezPdv = Convert.ToDouble(orderItemEntity.Price),
					CeneVuciIzOvogMagacina = request.Type switch
					{
						ForwardToKomercijalnoType.Profaktura => 150,
						_ => null
					}
				}
			);
		}
		#endregion

		#region Kalkulacija
		if (request.Type == ForwardToKomercijalnoType.InternaOtpremnica)
		{
			var dokumentKalkulacijeKomercijalno = await client.Dokumenti.CreateAsync(
				new KomercijalnoApiDokumentiCreateRequest
				{
					VrDok = 18,
					MagacinId = komercijalnoDokument.MagId!.Value,
					ZapId = 107,
					RefId = 107,
					IntBroj = "Web: " + request.OneTimeHash[..8],
					Flag = 0,
					KodDok = 0,
					Linked = "0000000000",
					Placen = 0,
					NrId = 1,
					VrdokIn = (short)komercijalnoDokument.VrDok,
					BrDokIn = komercijalnoDokument.BrDok
				}
			);

			foreach (var orderItemEntity in order.Items)
			{
				var link = komercijalnoWebProductLinks.FirstOrDefault(x =>
					x.WebId == orderItemEntity.ProductId
				);
				if (link == null)
					throw new LSCoreBadRequestException(
						$"Product {orderItemEntity.Product.Name} not linked to Komercijalno."
					);

				await client.Stavke.CreateAsync(
					new StavkaCreateRequest
					{
						VrDok = dokumentKalkulacijeKomercijalno.VrDok,
						BrDok = dokumentKalkulacijeKomercijalno.BrDok,
						RobaId = link.RobaId,
						Kolicina = Convert.ToDouble(orderItemEntity.Quantity)
					}
				);
			}

			// Update otpremnica out with kalkulacija values
			await client.Dokumenti.UpdateDokOut(
				new DokumentSetDokOutRequest()
				{
					VrDok = komercijalnoDokument.VrDok,
					BrDok = komercijalnoDokument.BrDok,
					VrDokOut = (short?)dokumentKalkulacijeKomercijalno.VrDok,
					BrDokOut = dokumentKalkulacijeKomercijalno.BrDok
				}
			);
		}
		#endregion

		if (settingRepository.GetValue<bool>(SettingKey.SMS_SEND_OBRADJENA_PORUDZBINA))
		{
			_ = officeServerApiManager.SmsQueueAsync(
				new SMSQueueRequest()
				{
					Numbers = new List<string>()
					{
						order.OrderOneTimeInformation == null
							? order.User.Mobile
							: order.OrderOneTimeInformation!.Mobile
					},
					Text =
						$"Vasa porudzbina {order.OneTimeHash[..5]} je obradjena. TD Broj: "
						+ komercijalnoDokument.VrDok
						+ "-"
						+ komercijalnoDokument.BrDok
						+ ". https://termodom.rs",
				}
			);
		}
	}

	public void PutOccupyReferent(OrdersPutOccupyReferentRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
		if (order == null)
			throw new LSCoreNotFoundException();

		if (order.ReferentId != null)
			throw new LSCoreBadRequestException("Porudžbina već ima referenta!");

		var currentUser = userRepository.GetCurrentUser();
		order.ReferentId = currentUser.Id;
		order.Status = OrderStatus.InReview;
		repository.Update(order);
	}

	public async Task PostUnlinkFromKomercijalnoAsync(
		OrdersPostUnlinkFromKomercijalnoRequest request
	)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);

		if (order == null)
			throw new LSCoreNotFoundException();

		var magacinId = order.KomercijalnoVrDok switch
		{
			4
				=> order.Store!.VPMagacinId
					?? throw new LSCoreBadRequestException(
						"Store doesn't have VPMagacin connected"
					),
			32 => order.StoreId,
			34 => order.StoreId,
			19 => order.StoreId,
			_ => throw new ArgumentOutOfRangeException(nameof(order.KomercijalnoVrDok))
		};
		var komercijalnoFirmaMagacin = await officeClient.KomercijalnoMagacinFirma.Get(
			(int)magacinId
		);
		var client = komercijalnoClientFactory.Create(
			DateTime.UtcNow.Year,
			TDKomercijalnoClientHelpers.ParseEnvironment(
				configurationRoot[Constants.DeployVariable]!
			),
			komercijalnoFirmaMagacin.ApiFirma
		);
		await client.Stavke.DeleteAsync(
			new StavkeDeleteRequest()
			{
				VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
				BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException()
			}
		);

		await client.Komentari.Flush(
			new FlushCommentsRequest()
			{
				VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
				BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException()
			}
		);

		// await komercijalnoApiManager.DokumentiKomentariUpdateAsync(
		// 	new UpdateKomentarRequest()
		// 	{
		// 		VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
		// 		BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException(),
		// 		InterniKomentar = Constants.DefaultOrderUnlinkFromKomercijalnoKomentar
		// 	}
		// );

		order.KomercijalnoBrDok = null;
		order.KomercijalnoVrDok = null;
		order.Status = OrderStatus.InReview;

		repository.Update(order);
	}

	public void PutAdminComment(OrdersPutAdminCommentRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
		if (order == null)
			throw new LSCoreNotFoundException();

		order.AdminComment = request.Comment;
		repository.Update(order);
	}

	public void PutPublicComment(OrdersPutPublicCommentRequest request)
	{
		var order = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
		if (order == null)
			throw new LSCoreNotFoundException();

		order.PublicComment = request.Comment;
		repository.Update(order);
	}
}
