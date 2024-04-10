using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Web.Cronjobs.OrderStatusUpdater.Helpers;
using TD.Web.Common.Repository;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Cronjobs.OrderStatusUpdater.Responses;
using TD.Web.Cronjobs.Common.Helpers;

namespace TD.Web.Cronjobs.OrderStatusUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Startup().RunApplication();
        }
    }
}