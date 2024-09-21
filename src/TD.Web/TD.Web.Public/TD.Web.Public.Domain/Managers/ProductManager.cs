using System.ComponentModel;
using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Requests.ProductGroups;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Enums;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Requests.Statistics;

namespace TD.Web.Public.Domain.Managers;

public class ProductManager(
    ILogger<ProductManager> logger,
    WebDbContext dbContext,
    IOrderManager orderManager,
    IImageManager imageManager,
    IStatisticsManager statisticsManager,
    IMemoryCache memoryCache,
    LSCoreContextUser contextUser
)
    : LSCoreManagerBase<ProductManager, ProductEntity>(logger, dbContext, contextUser),
        IProductManager
{
    public string AddToCart(AddToCartRequest request)
    {
        request.Validate();

        return orderManager.AddItem(
            new OrdersAddItemRequest()
            {
                ProductId = request.Id,
                OneTimeHash = request.OneTimeHash,
                Quantity = request.Quantity,
            }
        );
    }

    // public async Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request)
    // {
    //     var response = new LSCoreFileResponse();
    //     var query = First(x => x.IsActive && x.Src.Equals(request.Src));
    //
    //     response.Merge(query);
    //     if (response.NotOk)
    //         return response;
    //
    //     return await imageManager.GetImageAsync(new ImagesGetRequest() {
    //         Image = query.Payload!.Image,
    //         Quality = request.ImageQuality ?? Constants.DefaultImageQuality,
    //     });
    // }
    private OneTimePricesDto GetProductsOneTimePrice(GetOneTimesProductPricesRequest request)
    {
        if (request.Product == null)
            throw new LSCoreNotFoundException();

        if (request.Product.Price == null) // This happens when product has no price set through office application
            return new OneTimePricesDto() { MinPrice = 0, MaxPrice = 0 };
        var priceK = PricesHelpers.CalculatePriceK(
            request.Product.Price.Min,
            request.Product.Price.Max
        );
        return new OneTimePricesDto()
        {
            MinPrice =
                request.Product.Price.Max
                - (
                    priceK
                    / Constants.NumberOfCartValueStages
                    * PricesHelpers.CalculateCartLevel(Constants.MaximumCartValueForDiscount)
                ),
            MaxPrice = request.Product.Price.Max,
        };
    }

    private UserPricesDto GetUsersPrice(GetUsersProductPricesRequest request)
    {
        var product = Queryable<ProductEntity>()
            .Include(x => x.Price)
            .FirstOrDefault(x => x.Id == request!.ProductId);

        if (product == null)
            throw new LSCoreNotFoundException();

        var productPriceGroupLevel = Queryable<ProductPriceGroupLevelEntity>()
            .FirstOrDefault(x =>
                x.IsActive
                && x.UserId == request!.UserId
                && x.ProductPriceGroupId == product.ProductPriceGroupId
            );

        try
        {
            return new UserPricesDto
            {
                PriceWithoutVAT = PricesHelpers.CalculateProductPriceByLevel(
                    product.Price.Min,
                    product.Price.Max,
                    productPriceGroupLevel?.Level ?? 0
                ),
                VAT = product.VAT
            };
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return new UserPricesDto { PriceWithoutVAT = 0, VAT = product.VAT };
        }
    }

    public LSCoreSortedAndPagedResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.KeywordSearch))
            request.KeywordSearch = request.KeywordSearch.ToLower();

        if (!string.IsNullOrWhiteSpace(request.KeywordSearch))
            statisticsManager
                .LogAsync(
                    new ProductSearchKeywordRequest() { SearchPhrase = request.KeywordSearch }
                )
                .Wait();

        const int depth = 2;

        var query = Queryable()
            .Where(x => x.IsActive && Constants.ProductStatusesVisibleOnPublic.Contains(x.Status))
            .Where(x => request.Ids == null || request.Ids.Count == 0 || request.Ids.Contains(x.Id))
            .Where(x =>
                // Group filter needs to be done manually like this
                // Because EF Core does not support recursive queries
                // If you increase depth, you need to add more layers (depth + 1 layers)
                string.IsNullOrWhiteSpace(request.GroupName)
                ||
                // first groups layer
                x.Groups.Any(z =>
                    (z.Name == request.GroupName && z.IsActive)
                    ||
                    // second groups layer
                    (
                        z.ParentGroup != null
                        && (z.ParentGroup.Name == request.GroupName && z.ParentGroup.IsActive)
                    )
                    ||
                    // third groups layer
                    (
                        z.ParentGroup != null
                        && z.ParentGroup.ParentGroup != null
                        && (
                            z.ParentGroup.ParentGroup.Name == request.GroupName
                            && z.ParentGroup.ParentGroup.IsActive
                        )
                    )
                )
            )
            .Where(x =>
                (
                    string.IsNullOrWhiteSpace(request.KeywordSearch)
                    || x.Name.ToLower().Contains(request.KeywordSearch)
                    || string.IsNullOrWhiteSpace(x.Src)
                    || x.Src.ToLower().Contains(request.KeywordSearch)
                    || (x.SearchKeywords != null && x.SearchKeywords.Any(z => z.ToLower().Contains(request.KeywordSearch)))
                    || (
                        string.IsNullOrWhiteSpace(x.CatalogId)
                        || x.CatalogId.ToLower().Contains(request.KeywordSearch)
                    )
                    || (
                        string.IsNullOrWhiteSpace(x.ShortDescription)
                        || x.ShortDescription.ToLower().Contains(request.KeywordSearch)
                    )
                )
            )
            .OrderByDescending(x => x.PriorityIndex)
            .Include(x => x.Unit)
            .Include(x => x.Price)
            .Include(x => x.Groups)
            .ThenIncludeRecursively(depth, x => x.ParentGroup)
            .OrderBy(x => 0)
            .AsQueryable();

        if (request.SortDirection == ListSortDirection.Ascending)
            query = query.OrderBy(
                request.SortColumn == null
                    ? x => 0
                    : ProductsSortColumnCodes.ProductsSortRules[
                        (ProductsSortColumnCodes.Products)request.SortColumn
                    ]
            );
        else
            query = query.OrderByDescending(
                request.SortColumn == null
                    ? x => 0
                    : ProductsSortColumnCodes.ProductsSortRules[
                        (ProductsSortColumnCodes.Products)request.SortColumn
                    ]
            );

        var totalCount = query.Count();
        var sortedAndPagedList = query
            .OrderByDescending(x => x.PriorityIndex)
            .Skip(request.PageSize * (request.CurrentPage - 1))
            .Take(request.PageSize)
            .ToList();
        // .ToSortedAndPagedResponse(request, ProductsSortColumnCodes.ProductsSortRules);

        var dtos = sortedAndPagedList.ToDtoList<ProductEntity, ProductsGetDto>();

        Parallel.ForEach(
            dtos,
            x =>
            {
                var product = sortedAndPagedList.First(z => z.Id == x.Id);

                #region retrieve image

                if (
                    memoryCache.TryGetValue(
                        $"image_{product.Image}",
                        out ImageCacheDto imageCacheDto
                    )
                )
                {
                    x.ImageContentType = imageCacheDto.ImageContentType;
                    x.ImageData = imageCacheDto.ImageData;
                }
                else
                {
                    try
                    {
                        var imageResponse = imageManager
                            .GetImageAsync(
                                new ImagesGetRequest()
                                {
                                    Image = product.Image,
                                    Quality = Constants.DefaultThumbnailQuality,
                                }
                            )
                            .Result;

                        x.ImageContentType = imageResponse.ContentType!;
                        x.ImageData = Convert.ToBase64String(imageResponse.Data!);

                        memoryCache.Set(
                            $"image_{product.Image}",
                            new ImageCacheDto()
                            {
                                ImageContentType = imageResponse.ContentType!,
                                ImageData = x.ImageData
                            },
                            new MemoryCacheEntryOptions()
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                                Size = 1
                            }
                        );
                    }
                    catch (LSCoreNotFoundException)
                    {
                        return;
                    }
                }
                #endregion
            }
        );
        dtos.ForEach(async x =>
        {
            var product = sortedAndPagedList.First(z => z.Id == x.Id);

            if (CurrentUser?.Id == null)
            {
                var oneTimePricesResponse = GetProductsOneTimePrice(
                    new GetOneTimesProductPricesRequest() { Product = product }
                );

                x.OneTimePrice = new ProductsGetOneTimePricesDto()
                {
                    MinPrice = oneTimePricesResponse.MinPrice,
                    MaxPrice = oneTimePricesResponse.MaxPrice
                };
            }
            else
            {
                var userPriceResponse = GetUsersPrice(
                    new GetUsersProductPricesRequest()
                    {
                        ProductId = x.Id,
                        UserId = CurrentUser.Id!.Value
                    }
                );

                x.UserPrice = new ProductsGetUserPricesDto()
                {
                    PriceWithoutVAT = userPriceResponse.PriceWithoutVAT,
                    VAT = x.VAT
                };
            }
        });

        return new LSCoreSortedAndPagedResponse<ProductsGetDto>()
        {
            Payload = dtos,
            Pagination = new LSCoreSortedAndPagedResponse<ProductsGetDto>.PaginationData(
                request.CurrentPage,
                request.PageSize,
                totalCount
            )
        };
    }

    public ProductsGetSingleDto GetSingle(ProductsGetImageRequest request)
    {
        var product = Queryable()
            .Where(x =>
                x.IsActive
                && x.Src == request.Src
                && Constants.ProductStatusesVisibleOnPublic.Contains(x.Status)
            )
            .Include(x => x.Unit)
            .Include(x => x.AlternateUnit)
            .Include(x => x.Groups)
            .Include(x => x.Price)
            .FirstOrDefault();

        if (product == null)
            throw new LSCoreNotFoundException();

        statisticsManager.LogAsync(new ProductViewCountRequest() { ProductId = product.Id }).Wait();

        var dto = product.ToDto<ProductEntity, ProductsGetSingleDto>();
        if (CurrentUser?.Id == null)
        {
            var oneTimePricesResponse = GetProductsOneTimePrice(
                new GetOneTimesProductPricesRequest() { Product = product }
            );

            dto.OneTimePrice = new ProductsGetOneTimePricesDto()
            {
                MinPrice = oneTimePricesResponse.MinPrice,
                MaxPrice = oneTimePricesResponse.MaxPrice
            };
        }
        else
        {
            var userPriceResponse = GetUsersPrice(
                new GetUsersProductPricesRequest()
                {
                    ProductId = product.Id,
                    UserId = CurrentUser.Id!.Value
                }
            );

            dto.UserPrice = new ProductsGetUserPricesDto()
            {
                PriceWithoutVAT = userPriceResponse.PriceWithoutVAT,
                VAT = product.VAT
            };
        }

        dto.ImageData = imageManager
            .GetImageAsync(
                new ImagesGetRequest()
                {
                    Image = product.Image,
                    Quality = Constants.DefaultImageQuality
                }
            )
            .GetAwaiter()
            .GetResult();

        #region Category implementation
        dto.Category = GetProductGroupSequential(
            new GetParentGroupSequentialRequest() { ProductId = product.Id }
        );
        #endregion

        return dto;
    }

    private List<GetProductGroupSequentialDto> GetProductGroupSequential(
        GetParentGroupSequentialRequest request
    )
    {
        var list = new List<GetProductGroupSequentialDto>();

        var parentGroup = Queryable<ProductEntity>()
            .Include(x => x.Groups)
            .ThenInclude(x => x.ParentGroup)
            .FirstOrDefault(x => x.Id == request!.ProductId && x.IsActive);

        if (parentGroup == null)
            return list;

        list.AddRange(parentGroup.Groups.Select(BuildTree));

        return list;
    }

    private GetProductGroupSequentialDto BuildTree(ProductGroupEntity? group)
    {
        var response = new GetProductGroupSequentialDto { Name = group.Name };

        group = Queryable<ProductGroupEntity>()
            .Include(x => x.ParentGroup)
            .FirstOrDefault(x => group.ParentGroupId == x.Id && x.IsActive);

        while (group != null)
        {
            var oldResponse = response;
            response = new GetProductGroupSequentialDto();
            response.Child = oldResponse;
            response.Name = group.Name;

            group = Queryable<ProductGroupEntity>()
                .Include(x => x.ParentGroup)
                .FirstOrDefault(x => group.ParentGroupId == x.Id && x.IsActive);
        }

        return response;
    }

    public void RemoveFromCart(RemoveFromCartRequest request) =>
        orderManager.RemoveItem(
            new RemoveOrderItemRequest()
            {
                ProductId = request.Id,
                OneTimeHash = request.OneTimeHash
            }
        );

    public void SetProductQuantity(SetCartQuantityRequest request) =>
        orderManager.ChangeItemQuantity(
            new ChangeItemQuantityRequest()
            {
                ProductId = request.Id,
                OneTimeHash = request.OneTimeHash,
                Quantity = request.Quantity
            }
        );

    public LSCoreSortedAndPagedResponse<ProductsGetDto> GetFavorites()
    {
        var orders = Queryable<OrderEntity>()
            .Where(x =>
                x.IsActive
                && x.CreatedBy == CurrentUser!.Id
                && new[]
                {
                    OrderStatus.InReview,
                    OrderStatus.PendingReview,
                    OrderStatus.WaitingCollection,
                    OrderStatus.Collected
                }.Contains(x.Status)
                && x.CheckedOutAt != null
                && x.CheckedOutAt.Value >= DateTime.UtcNow.AddDays(-30)
                && x.CheckedOutAt.Value < DateTime.UtcNow
            )
            .Include(x => x.Items)
            .ThenInclude(x => x.Product);

        var distinctProductIdsInTheseOrders = orders
            .SelectMany(x => x.Items.Select(z => z.ProductId))
            .Distinct()
            .ToList();

        if (!distinctProductIdsInTheseOrders.Any())
            return new LSCoreSortedAndPagedResponse<ProductsGetDto>() { Payload = [], };

        var productOccuredXTimes = distinctProductIdsInTheseOrders.ToDictionary(
            id => id,
            id => orders.Count(x => x.Items.Any(z => z.ProductId == id))
        );

        return GetMultiple(
            new ProductsGetRequest()
            {
                Ids = productOccuredXTimes
                    .Select(x => x.Key)
                    .ToList()
                    .OrderByDescending(x => x)
                    .Take(20)
                    .ToList()
            }
        );
    }

    public LSCoreSortedAndPagedResponse<ProductsGetDto> GetSuggested(
        GetSuggestedProductsRequest request
    )
    {
        var query = Queryable().Where(x => x.IsActive).Include(x => x.Groups).Include(x => x.Unit);

        if (request.BaseProductId.HasValue)
        {
            var baseProduct = query.FirstOrDefault(x => x.Id == request.BaseProductId);
            var baseProductGroupIds = baseProduct.Groups.Select(x => x.Id).ToList();

            var suggestedProducts = query.Where(x =>
                x.Id != request.BaseProductId
                && x.Groups.Any(z => baseProductGroupIds.Contains(z.Id))
            );

            if (suggestedProducts.Count() >= 5)
                return GetMultiple(
                    new ProductsGetRequest()
                    {
                        Ids = suggestedProducts.Take(5).Select(x => x.Id).ToList()
                    }
                );
        }

        return GetMultiple(
            new ProductsGetRequest()
            {
                Ids = query
                    .Where(x => x.Id != request.BaseProductId)
                    .OrderByDescending(x => x.PriorityIndex)
                    .Take(5)
                    .Select(x => x.Id)
                    .ToList()
            }
        );
    }
}
