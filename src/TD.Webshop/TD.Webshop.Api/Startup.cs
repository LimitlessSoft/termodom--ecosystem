using Lamar;

namespace TD.Webshop.Api
{
    public class Startup
    {
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json", true);

            ConfigurationRoot = builder.Build();
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddControllers();
            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();

            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();

            applicationBuilder.UseHttpsRedirection();

            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints((routes) =>
            {
                routes.MapControllers();
            });
        }
    }
}