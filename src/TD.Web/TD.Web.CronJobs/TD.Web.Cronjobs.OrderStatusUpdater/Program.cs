using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Web.Cronjobs.OrderStatusUpdater.Helpers;
using TD.Web.Common.Repository;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Http;
using TD.Web.Cronjobs.OrderStatusUpdater.Responses;

namespace TD.Web.Cronjobs.OrderStatusUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var paramBuilder = new DbContextOptionsBuilder<WebDbContext>()
                .UseNpgsql(StringHelper.CreateConnectionString(config));

            using (var dbContext = new WebDbContext(paramBuilder.Options))
            {
                var orders = dbContext.Orders
                    .Where(x => x.Status == Common.Contracts.Enums.OrderStatus.WaitingCollection && x.IsActive)
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
                var apiUrl = String.Format(Constants.DefaultKomercijalnoApi, DateTime.Now.Year.ToString(), order.KomercijalnoBrDok);

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<ApiOrderResponse>(responseBody);

                    if (responseObject.Flag == 1 && responseObject.Placen == 1)
                        order.Status = Common.Contracts.Enums.OrderStatus.Collected;

                }
            }
        }
    }
}