using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Cronjobs.Common.Framework;
using TD.Web.Cronjobs.Common.Helpers;
using TD.Web.Cronjobs.OrderStatusUpdater.Helpers;
using TD.Web.Cronjobs.OrderStatusUpdater.Responses;

namespace TD.Web.Cronjobs.OrderStatusUpdater
{
    public class Startup : BaseCronjobStartup
    {
        public Startup() : base() { }
        public async Task RunApplication()
        {
            var paramBuilder = new DbContextOptionsBuilder<WebDbContext>()
                .UseNpgsql(StringHelper.CreateConnectionString(base.Configuration));

            using (var dbContext = new WebDbContext(paramBuilder.Options))
            {
                var orders = dbContext.Orders
                    .Where(x => x.Status == Web.Common.Contracts.Enums.OrderStatus.WaitingCollection && x.IsActive)
                    .ToList();

                foreach (var order in orders)
                    await ProcessOrder(order);

                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task ProcessOrder(OrderEntity order)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = ApiHelper.GetDokumentiEndpoint(order.KomercijalnoVrDok, order.KomercijalnoBrDok);

                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ApiOrderResponse>(responseBody);

                    if (responseObject.Flag == 1 && responseObject.Placen == 1)
                        order.Status = Web.Common.Contracts.Enums.OrderStatus.Collected;

                }
            }
        }
    }
}
