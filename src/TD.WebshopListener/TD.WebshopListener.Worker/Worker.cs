using FirebirdSql.Data.FirebirdClient;
using System.Runtime.InteropServices;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (FbConnection con = new FbConnection("data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.FDB; user=SYSDBA; password=m"))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT * FROM MAGACIN", con))
                {
                    using(FbDataReader dr = cmd.ExecuteReader())
                        while(dr.Read())
                        {
                            var a = dr[0].ToString();
                        }
                }
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}