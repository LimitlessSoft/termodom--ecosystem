using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Responses;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts;

namespace TD.Web.Admin.Domain.Managers;

public class OrderManager (
    ILogger<OrderManager> logger,
    IKomercijalnoApiManager komercijalnoApiManager,
    IOfficeServerApiManager officeServerApiManager,
    WebDbContext dbContext,
    LSCoreContextUser currentUser)
    : LSCoreManagerBase<OrderManager, OrderEntity>(logger, dbContext, currentUser), IOrderManager
{
    public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request) =>
        Queryable()
            .Where(x => x.IsActive &&
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
        var order = Queryable()
            .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
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
        var order = Queryable()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        
        if (order == null)
            throw new LSCoreNotFoundException();
            
        order.StoreId = request.StoreId;
        Update(order);
    }

    public void PutStatus(OrdersPutStatusRequest request)
    {
        var order = Queryable()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        
        if (order == null)
            throw new LSCoreNotFoundException();

        order.Status = request.Status;
        Update(order);
    }

    public void PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request)
    {
        var order = Queryable()
            .FirstOrDefault(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
        
        if (order == null)
            throw new LSCoreNotFoundException();

        order.PaymentTypeId = request.PaymentTypeId;
        Update(order);
    }

    public async Task PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request)
    {
        var order = Queryable()
            .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
            .Include(x => x.Items)
            .Include(x => x.PaymentType)
            .Include(x => x.User)
            .Include(x => x.OrderOneTimeInformation)
            .FirstOrDefault();
        
        if(order == null)
            throw new LSCoreNotFoundException();

        var komercijalnoWebProductLinks = Queryable<KomercijalnoWebProductLinkEntity>()
            .Where(x => x.IsActive);

        var vrDok = request.IsPonuda != null && request.IsPonuda.Value ? 34 : 32;

        #region Create document in Komercijalno
        var komercijalnoDokument = await komercijalnoApiManager.DokumentiPostAsync(
            new KomercijalnoApiDokumentiCreateRequest()
            {
                VrDok = vrDok,
                MagacinId = order.StoreId,
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
            Komentar = $"Porudžbina kreirana uz pomoć www.termodom.rs profi kutka.\r\n\r\nPorudžbina id: {request.OneTimeHash}\r\n\r\nSkraćeni id: {request.OneTimeHash[..8]}",
            InterniKomentar = order.OrderOneTimeInformation != null ?
                $"Ovo je jednokratna kupovina\r\nKupac je ostavio kontakt: {order.OrderOneTimeInformation.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061" :
                $"KupacId: {order.User.Id}\r\nKupac: {order.User.Nickname}({order.User.Username})\r\nKupac ostavio kontakt: {order.User.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061"
        });
        #endregion
            
        order.KomercijalnoVrDok = komercijalnoDokument.VrDok;
        order.KomercijalnoBrDok = komercijalnoDokument.BrDok;
        order.Status = OrderStatus.WaitingCollection;
        Update(order);
            
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
        var order = Queryable()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);

        if(order == null)
            throw new LSCoreNotFoundException();
        
        if(order.ReferentId != null)
            throw new LSCoreBadRequestException("Porudžbina već ima referenta!");
            
        order.ReferentId = CurrentUser!.Id;
        order.Status = OrderStatus.InReview;
        Update(order);
    }

    public void PostUnlinkFromKomercijalno(OrdersPostUnlinkFromKomercijalnoRequest request)
    {
        var order = Queryable()
            .FirstOrDefault(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
        
        if(order == null)
            throw new LSCoreNotFoundException();
        
        order.KomercijalnoBrDok = null;
        order.KomercijalnoVrDok = null;
        order.Status = OrderStatus.InReview;
            
        Update(order);
    }
}