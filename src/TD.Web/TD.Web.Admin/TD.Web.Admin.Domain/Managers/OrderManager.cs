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

namespace TD.Web.Admin.Domain.Managers
{
    public class OrderManager : LSCoreBaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        
        public OrderManager(ILogger<OrderManager> logger,
            IKomercijalnoApiManager komercijalnoApiManager,
            WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request)
        {
            var response = new LSCoreSortedPagedResponse<OrdersGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var orders = qResponse.Payload!
                .Where(x => x.IsActive &&
                    (request.Status == null || request.Status.Contains(x.Status)))
                .Include(x => x.User)
                .Include(x => x.OrderOneTimeInformation)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product);

            response.Payload = orders.ToDtoList<OrdersGetDto, OrderEntity>();
            return response;
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
            try
            {
                var response = new LSCoreResponse();
            
                var orderResponse = Queryable()
                    .LSCoreFilters(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
                    .LSCoreIncludes(x => x.Items);
            
                response.Merge(orderResponse);
                if (response.NotOk || orderResponse.Payload?.FirstOrDefault() == null)
                    return LSCoreResponse.BadRequest();

                var komercijalnoWebProductLinksResponse = Queryable<KomercijalnoWebProductLinkEntity>()
                    .LSCoreFilters(x => x.IsActive);
                
                response.Merge(komercijalnoWebProductLinksResponse);
                if (response.NotOk)
                    return response;

                var order = orderResponse.Payload!.First();

                // Create document in Komercijalno
                var dokumentCreateResponse = await _komercijalnoApiManager.DokumentiPostAsync(
                    new KomercijalnoApiDokumentiCreateRequest()
                    {
                        VrDok = 32,
                        MagacinId = order.StoreId,
                        ZapId = 107,
                        RefId = 107,
                        IntBroj = "Web: " + request.OneTimeHash.Substring(0, 8),
                    });
                response.Merge(dokumentCreateResponse);
                if(response.NotOk)
                    return response;
            
                var dokument = dokumentCreateResponse.Payload!;
                order.KomercijalnoVrDok = dokument.VrDok;
                order.KomercijalnoBrDok = dokument.BrDok;
                response.Merge(Update(order));
                if(response.NotOk)
                    return response;
                
                var komercijalnoWebProductLinks = komercijalnoWebProductLinksResponse.Payload!;
                
                // Insert items into komercijalno dokument
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
                return new LSCoreResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            response.Merge(Update(order));
            
            return response;
        }
    }
}
