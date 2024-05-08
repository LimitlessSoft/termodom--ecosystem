using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Responses;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Domain.Managers
{
    public class OrderManager : LSCoreBaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        private readonly IOfficeServerApiManager _officeServerApiManager;
        public OrderManager(ILogger<OrderManager> logger,
            IKomercijalnoApiManager komercijalnoApiManager,
            IOfficeServerApiManager officeServerApiManager,
            WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
            _officeServerApiManager = officeServerApiManager;
        }

        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request)
        {
            var response = new LSCoreSortedPagedResponse<OrdersGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var ordersSortedAndPagedResponse = qResponse.Payload!
                .Where(x => x.IsActive &&
                    (request.Status == null || request.Status.Contains(x.Status)))
                .Include(x => x.User)
                .Include(x => x.OrderOneTimeInformation)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ToSortedAndPagedResponse(request, OrdersSortColumnCodes.OrdersSortRules);

            response.Merge(ordersSortedAndPagedResponse);
            if (response.NotOk)
                return response;

            return new LSCoreSortedPagedResponse<OrdersGetDto>(ordersSortedAndPagedResponse.Payload.ToDtoList<OrdersGetDto ,OrderEntity>(),
                request,
                ordersSortedAndPagedResponse.Pagination.TotalElementsCount);
        }

        public LSCoreResponse<OrdersGetDto> GetSingle(OrdersGetSingleRequest request)
        {
            var response = new LSCoreResponse<OrdersGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var order = qResponse.Payload!
                .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .Include(x => x.OrderOneTimeInformation)
                .Include(x => x.Referent)
                .Include(x => x.User)
                .ThenInclude(x => x.ProductPriceGroupLevels)
                .FirstOrDefault();

            if (order == null)
                return LSCoreResponse<OrdersGetDto>.NotFound();

            response.Payload = order.ToDto<OrdersGetDto, OrderEntity>();
            return response;
        }

        public LSCoreResponse PutStoreId(OrdersPutStoreIdRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.StoreId = request.StoreId;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }

        public LSCoreResponse PutStatus(OrdersPutStatusRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.Status = request.Status;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }

        public LSCoreResponse PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.PaymentTypeId = request.PaymentTypeId;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }

        public async Task<LSCoreResponse> PostForwardToKomercijalnoAsync(OrdersPostForwardToKomercijalnoRequest request)
        {
            var response = new LSCoreResponse();
        
            var orderResponse = Queryable()
                .LSCoreFilters(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
                .LSCoreIncludes(x => x.Items, x => x.PaymentType, x => x.User, x => x.OrderOneTimeInformation);
        
            response.Merge(orderResponse);
            if (response.NotOk || orderResponse.Payload?.FirstOrDefault() == null)
                return LSCoreResponse.BadRequest();

            var komercijalnoWebProductLinksResponse = Queryable<KomercijalnoWebProductLinkEntity>()
                .LSCoreFilters(x => x.IsActive);
            
            response.Merge(komercijalnoWebProductLinksResponse);
            if (response.NotOk)
                return response;

            var order = orderResponse.Payload!.First();

            var vrDok = request.IsPonuda != null && request.IsPonuda.Value ? 34 : 32;

            #region Create document in Komercijalno
            var dokumentCreateResponse = await _komercijalnoApiManager.DokumentiPostAsync(
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
            response.Merge(dokumentCreateResponse);
            if(response.NotOk)
                return response;
            #endregion
            
            #region Update komercijalno dokument komentari
            var dokumentKomentariCreateResponse = await _komercijalnoApiManager.DokumentiKomentariPostAsync(new CreateKomentarRequest()
            {
                VrDok = dokumentCreateResponse.Payload!.VrDok,
                BrDok = dokumentCreateResponse.Payload!.BrDok,
                Komentar = $"Porudžbina kreirana uz pomoć www.termodom.rs profi kutka.\r\n\r\nPorudžbina id: {request.OneTimeHash}\r\n\r\nSkraćeni id: {request.OneTimeHash[..8]}",
                InterniKomentar = order.OrderOneTimeInformation != null ?
                    $"Ovo je jednokratna kupovina\r\nKupac je ostavio kontakt: {order.OrderOneTimeInformation.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061" :
                    $"KupacId: {order.User.Id}\r\nKupac: {order.User.Nickname}({order.User.Username})\r\nKupac ostavio kontakt: {order.User.Mobile}\r\nDatum porucivanja: {order.CreatedAt:dd.MM.yyyy}\r\nDatum obrade: {DateTime.Now:dd.MM.yyyy HH:mm}\r\n\r\nhttps://admin.termodom.rs/porudzbine/4B186ED06D8C2C762F0FF78339700061"
            });
            #endregion
            
            var dokument = dokumentCreateResponse.Payload!;
            
            order.KomercijalnoVrDok = dokument.VrDok;
            order.KomercijalnoBrDok = dokument.BrDok;
            order.Status = OrderStatus.WaitingCollection;
            response.Merge(Update(order));
            if(response.NotOk)
                return response;
            
            var komercijalnoWebProductLinks = komercijalnoWebProductLinksResponse.Payload!;
            
            #region Insert items into komercijalno dokument
            foreach (var orderItemEntity in order.Items)
            {
                var link = komercijalnoWebProductLinks.FirstOrDefault(x => x.WebId == orderItemEntity.ProductId);
                if(link == null)
                    return LSCoreResponse.BadRequest($"Product {orderItemEntity.Product.Name} not linked to Komercijalno.");
                
                response.Merge(await _komercijalnoApiManager.StavkePostAsync(new StavkaCreateRequest()
                {
                    VrDok = dokument.VrDok,
                    BrDok = dokument.BrDok,
                    RobaId = link.RobaId,
                    Kolicina = Convert.ToDouble(orderItemEntity.Quantity),
                    ProdajnaCenaBezPdv = Convert.ToDouble(orderItemEntity.Price)
                }));
                if(response.NotOk)
                    return response;
            }
            #endregion

            _officeServerApiManager.SMSQueueAsync(new SMSQueueRequest()
            {
                Numbers = new List<string>() { order.OrderOneTimeInformation == null ? order.User.Mobile : order.OrderOneTimeInformation!.Mobile }, 
                Text = $"Vasa porudzbina {order.OneTimeHash[..5]} je obradjena. TD Broj: " + dokument.BrDok,
            });
            
            return new LSCoreResponse();
        }

        public LSCoreResponse PutOccupyReferent(OrdersPutOccupyReferentRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;

            var order = orderResponse.Payload!;
            if(order.ReferentId != null)
                return LSCoreResponse.BadRequest("Porudžbina već ima referenta!");
            
            order.ReferentId = CurrentUser!.Id;
            order.Status = OrderStatus.InReview;
            response.Merge(Update(order));
            
            return response;
        }

        public LSCoreResponse PostUnlinkFromKomercijalno(OrdersPostUnlinkFromKomercijalnoRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.IsActive && x.OneTimeHash == request.OneTimeHash);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;

            var order = orderResponse.Payload!;
            order.KomercijalnoBrDok = null;
            order.KomercijalnoVrDok = null;
            order.Status = OrderStatus.InReview;
            
            response.Merge(Update(order));
            
            return response;
        }
    }
}
