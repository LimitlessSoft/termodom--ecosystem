using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Constants;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Helpers;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Helpers;

namespace TD.Office.Public.Domain.Managers
{
	public class WebManager(
		IUslovFormiranjaWebCeneRepository uslovFormiranjaWebCeneRepository,
		IKomercijalnoPriceRepository komercijalnoPriceRepository,
		IKomercijalnoPriceKoeficijentEntityRepository komercijalnoPriceKoeficijentEntityRepository,
		ILogger<WebManager> logger,
		OfficeDbContext dbContext,
		ICacheManager cacheManager,
		ITDWebAdminApiManager webAdminApimanager,
		ITDKomercijalnoApiManager komercijalnoApiManager
	) : IWebManager
	{
		private readonly ILogger<WebManager> _logger = logger;

		public async Task<List<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(
			WebAzuiranjeCenaRequest request
		)
		{
			var responseList = new List<WebAzuriranjeCenaDto>();

			// TODO: Implement as sortable pageable
			var webProducts = await webAdminApimanager.ProductsGetMultipleAsync(
				new ProductsGetMultipleRequest()
			);
			var komercijalnoWebLinks =
				await webAdminApimanager.KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
			var komercijalnoPrices = komercijalnoPriceRepository
				.GetMultiple()
				.ToList()
				.ToDictionary(x => x.RobaId, x => x);
			var uslovi = uslovFormiranjaWebCeneRepository
				.GetMultiple()
				.ToList()
				.ToDictionary(x => x.WebProductId, x => x);

			webProducts!
				.Where(x => request.Id == null || x.Id == request.Id)
				.ToList()
				.ForEach(x =>
				{
					var link = komercijalnoWebLinks?.FirstOrDefault(y => y.WebId == x.Id);
					var komercijalnoPrice =
						link == null ? null : komercijalnoPrices.GetValueOrDefault(link.RobaId);

					var uslov = uslovi.GetValueOrDefault(x.Id);
					if (uslov == null)
					{
						uslov = new UslovFormiranjaWebCeneEntity()
						{
							WebProductId = x.Id,
							Modifikator = 0,
							Type = UslovFormiranjaWebCeneType.ProdajnaCenaPlusProcenat,
						};
						uslovFormiranjaWebCeneRepository.Insert(uslov);
					}

					responseList.Add(
						new WebAzuriranjeCenaDto()
						{
							Id = x.Id,
							Naziv = x.Name,
							MinWebOsnova = x.MinWebBase,
							MaxWebOsnova = x.MaxWebBase,
							NabavnaCenaKomercijalno = komercijalnoPrice?.NabavnaCenaBezPDV ?? 0,
							ProdajnaCenaKomercijalno = komercijalnoPrice?.ProdajnaCenaBezPDV ?? 0,
							IronCena = PricesHelpers.CalculateProductPriceByLevel(
								x.MinWebBase,
								x.MaxWebBase,
								0
							),
							SilverCena = PricesHelpers.CalculateProductPriceByLevel(
								x.MinWebBase,
								x.MaxWebBase,
								1
							),
							GoldCena = PricesHelpers.CalculateProductPriceByLevel(
								x.MinWebBase,
								x.MaxWebBase,
								2
							),
							PlatinumCena = PricesHelpers.CalculateProductPriceByLevel(
								x.MinWebBase,
								x.MaxWebBase,
								3
							),
							LinkRobaId = link?.RobaId,
							LinkId = link?.Id,
							UslovFormiranjaWebCeneId = uslov.Id,
							UslovFormiranjaWebCeneModifikator = uslov.Modifikator,
							UslovFormiranjaWebCeneType = uslov.Type,
						}
					);
				});

			return responseList;
		}

		public async Task AzurirajCeneKomercijalnoPoslovanje()
		{
			await ProcessInProgressHelpers.ValidateProcessIsNotInProgressAsync(
				cacheManager,
				CacheKeysConstants.AzurirajCeneKomercijalnoPoslovanjeInprogressKey
			);

			try
			{
				var robaUMagacinu = await komercijalnoApiManager.GetRobaUMagacinuAsync(
					new KomercijalnoApiGetRobaUMagacinuRequest() { MagacinId = 150 }
				);

				var nabavneCeneNaDan = await komercijalnoApiManager.GetNabavnaCenaNaDanAsync(
					new ProceduraGetNabavnaCenaNaDanRequest() { Datum = DateTime.UtcNow }
				);

				var prodajneCeneNaDan =
					await komercijalnoApiManager.GetProdajnaCenaNaDanOptimizedAsync(
						new ProceduraGetProdajnaCenaNaDanOptimizedRequest()
						{
							Datum = DateTime.UtcNow,
							MagacinId = 150,
						}
					);

				foreach (var rum in robaUMagacinu)
				{
					var nabavnaCenaNaDan = nabavneCeneNaDan.FirstOrDefault(x =>
						x.RobaId == rum.RobaId
					);
					var prodajnaCenaNaDan = prodajneCeneNaDan.FirstOrDefault(x =>
						x.RobaId == rum.RobaId
					);

					if (nabavnaCenaNaDan != null)
						rum.NabavnaCena = nabavnaCenaNaDan.NabavnaCenaBezPDV;

					if (prodajnaCenaNaDan != null)
						rum.ProdajnaCena = prodajnaCenaNaDan.ProdajnaCenaBezPDV;
				}

				komercijalnoPriceRepository.HardClear();

				var list = new List<KomercijalnoPriceEntity>();
				robaUMagacinu.ForEach(roba =>
				{
					list.Add(
						new KomercijalnoPriceEntity()
						{
							RobaId = roba.RobaId,
							NabavnaCenaBezPDV = (decimal)roba.NabavnaCena,
							ProdajnaCenaBezPDV = (decimal)roba.ProdajnaCena,
						}
					);
				});
				komercijalnoPriceRepository.Insert(list);
			}
			catch
			{
				throw;
			}
			finally
			{
				await ProcessInProgressHelpers.SetProcessAsCompletedAsync(
					cacheManager,
					CacheKeysConstants.AzurirajCeneKomercijalnoPoslovanjeInprogressKey
				);
			}
		}

		public async Task<KomercijalnoWebProductLinksGetDto?> AzurirajCeneKomercijalnoPoslovanjePoveziProizvode(
			KomercijalnoWebProductLinksSaveRequest request
		) => await webAdminApimanager.KomercijalnoWebProductLinksControllerPutAsync(request);

		public void AzurirajCeneUsloviFormiranjaMinWebOsnova(
			WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request
		)
		{
			var entity = request.Id.HasValue
				? uslovFormiranjaWebCeneRepository.Get(request.Id.Value)
				: new UslovFormiranjaWebCeneEntity();
			entity.InjectFrom(request);
			if (request.Id.HasValue)
				uslovFormiranjaWebCeneRepository.Update(entity);
			else
				uslovFormiranjaWebCeneRepository.Insert(entity);
		}

		public async Task AzurirajCeneMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request)
		{
			await ProcessInProgressHelpers.ValidateProcessIsNotInProgressAsync(
				cacheManager,
				CacheKeysConstants.WebAuzurirajCeneMaxWebOsnoveInProgressKey
			);

			await webAdminApimanager.ProductsUpdateMaxWebOsnove(request);

			await ProcessInProgressHelpers.SetProcessAsCompletedAsync(
				cacheManager,
				CacheKeysConstants.WebAuzurirajCeneMaxWebOsnoveInProgressKey
			);
		}

		public async Task AzurirajCeneMinWebOsnove()
		{
			await ProcessInProgressHelpers.ValidateProcessIsNotInProgressAsync(
				cacheManager,
				CacheKeysConstants.WebAuzurirajCeneMinWebOsnoveInProgressKey
			);

			var request = new ProductsUpdateMinWebOsnoveRequest()
			{
				Items = new List<ProductsUpdateMinWebOsnoveRequest.MinItem>(),
			};

			var azuriranjeCenaAsyncResponse = await AzuriranjeCenaAsync(
				new WebAzuiranjeCenaRequest()
			);

			azuriranjeCenaAsyncResponse.ForEach(x =>
			{
				request.Items.Add(
					new ProductsUpdateMinWebOsnoveRequest.MinItem()
					{
						ProductId = x.Id,
						MinWebOsnova = CalculateMinWebOsnova(x),
					}
				);
			});

			await webAdminApimanager.UpdateMinWebOsnove(request);
			await ProcessInProgressHelpers.SetProcessAsCompletedAsync(
				cacheManager,
				CacheKeysConstants.WebAuzurirajCeneMinWebOsnoveInProgressKey
			);

			return;

			decimal CalculateMinWebOsnova(WebAzuriranjeCenaDto x)
			{
				var minWebOsnova = 0m;
				switch (x.UslovFormiranjaWebCeneType)
				{
					case UslovFormiranjaWebCeneType.NabavnaCenaPlusProcenat:
						minWebOsnova =
							x.NabavnaCenaKomercijalno
							+ (
								x.NabavnaCenaKomercijalno
								* x.UslovFormiranjaWebCeneModifikator
								/ 100
							);
						break;
					case UslovFormiranjaWebCeneType.ProdajnaCenaPlusProcenat:
						minWebOsnova =
							x.ProdajnaCenaKomercijalno
							- (
								x.ProdajnaCenaKomercijalno
								* x.UslovFormiranjaWebCeneModifikator
								/ 100
							);
						break;
					case UslovFormiranjaWebCeneType.CenaNaUpit:
						minWebOsnova = 0;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				var prodajnaCena = x.ProdajnaCenaKomercijalno;
				var maxDiscount =
					0.2 / (double)Web.Common.Contracts.LegacyConstants.DiscountPartFromDifference;
				if (prodajnaCena <= 0)
					return minWebOsnova;
				var maxAllowedDiscountValue = prodajnaCena * (decimal)maxDiscount;
				var calculatedDiscountValue = prodajnaCena - minWebOsnova;
				if (calculatedDiscountValue > maxAllowedDiscountValue)
					minWebOsnova = prodajnaCena - maxAllowedDiscountValue;
				return minWebOsnova;
			}
		}

		public async Task<
			List<KeyValuePair<long, string>>
		> AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion(
			AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionRequest request
		)
		{
			var response = new List<KeyValuePair<long, string>>(
				new List<KeyValuePair<long, string>>()
			);

			if (
				string.IsNullOrWhiteSpace(request.SearchText)
				|| request.SearchText.Length
					< LegacyConstants.AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionSearchTextMinimumLength
			)
				return response;

			var filteredWebProducts = await webAdminApimanager.ProductsGetMultipleAsync(
				new ProductsGetMultipleRequest() { SearchFilter = request.SearchText }
			);

			foreach (var productEntity in filteredWebProducts)
				response.Add(new KeyValuePair<long, string>(productEntity.Id, productEntity.Name));

			return response;
		}

		public async Task<List<ProductsGetDto>?> GetProducts(ProductsGetMultipleRequest request) =>
			await webAdminApimanager.ProductsGetMultipleAsync(request);

		public async Task<KomercijalnoPriceKoeficijentiDto> GetKomercijalnoPriceKoeficijenti() =>
			new()
			{
				Items = komercijalnoPriceKoeficijentEntityRepository
					.GetMultiple()
					.ToMappedList<
						KomercijalnoPriceKoeficijentEntity,
						KomercijalnoPriceKoeficijentiDto.Item
					>(),
			};

		public Task CreateOrUpdateKomercijalnoPriceKoeficijent(
			CreateOrUpdateKomercijalnoPriceKoeficijentRequest request
		)
		{
			request.Validate();
			var entity = request.Id.HasValue
				? komercijalnoPriceKoeficijentEntityRepository.Get(request.Id.Value)
				: new KomercijalnoPriceKoeficijentEntity();

			// if new entity, check for duplicate 'Naziv'
			if (!request.Id.HasValue)
			{
				var existingEntity = komercijalnoPriceKoeficijentEntityRepository
					.GetMultiple()
					.FirstOrDefault(e => e.Naziv == request.Naziv);
				if (existingEntity != null)
				{
					throw new LSCoreBadRequestException(
						$"Koeficijent sa nazivom '{request.Naziv}' već postoji."
					);
				}
			}
			entity.InjectFrom(request);
			komercijalnoPriceKoeficijentEntityRepository.UpdateOrInsert(entity);
			return Task.CompletedTask;
		}
	}
}
