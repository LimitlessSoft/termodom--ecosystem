using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Requests.Stavke;
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

namespace TD.Web.Admin.Domain.Managers;

public class OrderManager (
    IKomercijalnoWebProductLinkRepository komercijalnoWebProductLinkRepository,
    IKomercijalnoApiManager komercijalnoApiManager,
    IOfficeServerApiManager officeServerApiManager,
    IOrderRepository repository,
    LSCoreContextUser contextUser)
    : IOrderManager
{
    public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request) =>
        repository.GetMultiple()
            .Where(x =>
                (request.Status == null || request.Status.Contains(x.Status)) &&
                (request.UserId == null || x.CreatedBy == request.UserId.Value))
            .Include(x => x.User)
            .ThenInclude(x => x.ProductPriceGroupLevels)
            .ThenInclude(x => x.ProductPriceGroup)
            .Include(x => x.OrderOneTimeInformation)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ToSortedAndPagedResponse<OrderEntity, OrdersSortColumnCodes.Orders, OrdersGetDto>(request, OrdersSortColumnCodes.OrdersSortRules);

    public OrdersGetDto GetSingle(OrdersGetSingleRequest request)
    {
        var order = repository.GetMultiple()
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

        return order.ToDto<OrderEntity, OrdersGetDto>();
    }

    public void PutStoreId(OrdersPutStoreIdRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        if (order == null)
            throw new LSCoreNotFoundException();
            
        order.StoreId = request.StoreId;
        repository.Update(order);
    }

    public void PutStatus(OrdersPutStatusRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        
        if (order == null)
            throw new LSCoreNotFoundException();

        order.Status = request.Status;
        repository.Update(order);

    }
    public void PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        
        if (order == null)
            throw new LSCoreNotFoundException();

        order.PaymentTypeId = request.PaymentTypeId;
        repository.Update(order);
    }

    public async Task PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request)
    {
        var order = repository.GetMultiple()
            .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
            .Include(x => x.Store)
            .Include(x => x.Items)
            .Include(x => x.PaymentType)
            .Include(x => x.User)
            .Include(x => x.OrderOneTimeInformation)
            .FirstOrDefault();
        
        if(order == null)
            throw new LSCoreNotFoundException();

        var komercijalnoWebProductLinks = komercijalnoWebProductLinkRepository.GetMultiple();

        var vrDok = request.Type switch
        {
            ForwardToKomercijalnoType.Proracun => 32,
            ForwardToKomercijalnoType.Ponuda => 34,
            ForwardToKomercijalnoType.Profaktura => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(request.Type))
        };

        var magacinId = request.Type switch
        {
            ForwardToKomercijalnoType.Proracun => order.StoreId,
            ForwardToKomercijalnoType.Ponuda => order.StoreId,
            ForwardToKomercijalnoType.Profaktura => order.Store!.VPMagacinId ?? throw new LSCoreBadRequestException("Store doesn't have VPMagacin connected"),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Type))
        };

        #region Create document in Komercijalno
        var komercijalnoDokument = await komercijalnoApiManager.DokumentiPostAsync(
            new KomercijalnoApiDokumentiCreateRequest()
            {
                VrDok = vrDok,
                MagacinId = (short)magacinId,
                ZapId = 107,
                RefId = 107,
                IntBroj = "Web: " + request.OneTimeHash[..8],
                Flag = 0,
                KodDok = 0,
                Linked = "0000000000",
                PPID = null,
                Placen = 0,
                NuId = (short)order.PaymentType.KomercijalnoNUID,
                NrId = 1,
            });
        #endregion
            
        #region Update komercijalno dokument komentari
        var dokumentKomentariCreateResponse = await komercijalnoApiManager.DokumentiKomentariPostAsync(new CreateKomentarRequest()
        {
            BrDok = komercijalnoDokument.BrDok,
            VrDok = komercijalnoDokument.VrDok,
            Komentar = $"Porudžbina kreirana uz pomoć www.termodom.rs profi kutka.\r\n\r\nPorudžbina id: {request.OneTimeHash}\r\n\r\nSkraćeni id: {request.OneTimeHash[..8]}\r\n===============\r\nJavni komentar: {order.PublicComment}",
            InterniKomentar = order.OrderOneTimeInformation != null ?
                     $"Ovo je jednokratna kupovina\r\nKupac je ostavio kontakt: {order.OrderOneTimeInformation.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061\r\n===============\r\nAdmin komentar: {order.AdminComment}" :
                     $"KupacId: {order.User.Id}\r\nKupac: {order.User.Nickname}({order.User.Username})\r\nKupac ostavio kontakt: {order.User.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061\r\n===============\r\nAdmin komentar: {order.AdminComment}"
        });
        #endregion
            
        order.KomercijalnoVrDok = komercijalnoDokument.VrDok;
        order.KomercijalnoBrDok = komercijalnoDokument.BrDok;
        order.Status = OrderStatus.WaitingCollection;
        repository.Update(order);
            
        #region Insert items into komercijalno dokument
        foreach (var orderItemEntity in order.Items)
        {
            var link = komercijalnoWebProductLinks.FirstOrDefault(x => x.WebId == orderItemEntity.ProductId);
            if(link == null)
                throw new LSCoreBadRequestException($"Product {orderItemEntity.Product.Name} not linked to Komercijalno.");
                
            await komercijalnoApiManager.StavkePostAsync(new StavkaCreateRequest
            {
                VrDok = komercijalnoDokument.VrDok,
                BrDok = komercijalnoDokument.BrDok,
                RobaId = link.RobaId,
                Kolicina = Convert.ToDouble(orderItemEntity.Quantity),
                ProdajnaCenaBezPdv = Convert.ToDouble(orderItemEntity.Price)
            });
        }
        #endregion

        _ =officeServerApiManager.SmsQueueAsync(new SMSQueueRequest()
        {
            Numbers = new List<string>() { order.OrderOneTimeInformation == null ? order.User.Mobile : order.OrderOneTimeInformation!.Mobile }, 
            Text = $"Vasa porudzbina {order.OneTimeHash[..5]} je obradjena. TD Broj: " + komercijalnoDokument.BrDok + ". https://termodom.rs",
        });
    }

    public void PutOccupyReferent(OrdersPutOccupyReferentRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
        if(order == null)
            throw new LSCoreNotFoundException();
        
        if(order.ReferentId != null)
            throw new LSCoreBadRequestException("Porudžbina već ima referenta!");
            
        order.ReferentId = contextUser.Id!.Value;
        order.Status = OrderStatus.InReview;
        repository.Update(order);
    }

    public async Task PostUnlinkFromKomercijalnoAsync(OrdersPostUnlinkFromKomercijalnoRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
        
        if(order == null)
            throw new LSCoreNotFoundException();

        await komercijalnoApiManager.StavkeDeleteAsync(new StavkeDeleteRequest()
        {
            VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
            BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException()
        });

        await komercijalnoApiManager.FlushCommentsAsync(new FlushCommentsRequest()
        {
            VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
            BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException()
        });

        await komercijalnoApiManager.DokumentiKomentariUpdateAsync(new UpdateKomentarRequest()
        {
            VrDok = order.KomercijalnoVrDok ?? throw new LSCoreBadRequestException(),
            BrDok = order.KomercijalnoBrDok ?? throw new LSCoreBadRequestException(),
            InterniKomentar = Constants.DefaultOrderUnlinkFromKomercijalnoKomentar
        });

        order.KomercijalnoBrDok = null;
        order.KomercijalnoVrDok = null;
        order.Status = OrderStatus.InReview;
            
        repository.Update(order);
    }

    public void PutAdminComment(OrdersPutAdminCommentRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
        if (order == null)
            throw new LSCoreNotFoundException();

        order.AdminComment = request.Comment;
        repository.Update(order);
    }

    public void PutPublicComment(OrdersPutPublicCommentRequest request)
    {
        var order = repository.GetMultiple()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
        if (order == null)
            throw new LSCoreNotFoundException();

        order.PublicComment = request.Comment;
        repository.Update(order);
    }
}