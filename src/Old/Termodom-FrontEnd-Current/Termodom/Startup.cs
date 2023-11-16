using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Termodom.Models;

namespace Termodom
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Initialize();
        }

        private void Initialize()
        {

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddHsts(ops =>
            {
                ops.Preload = true;
                ops.IncludeSubDomains = true;
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Program.WebRootPath = env.WebRootPath;

            #region Culture
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberGroupSeparator = ",";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
            cultureInfo.NumberFormat.CurrencyGroupSeparator = ",";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            #endregion

            Program.ConnectionString = "Server=mysql6008.site4now.net;Database=db_a997a5_tdmain;Uid=a997a5_tdmain;Pwd=Plivanje333;Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
            if (env.IsDevelopment())
            {
                Program.APIUsername = "sasar";
                Program.APIPassword = "12321";
                Program.BaseAPIUrl = "https://api.termodom.rs";
                //Program.BaseAPIUrl = "https://localhost:44311";

                //Program.APIUsername = "termodom_webshop_dev";
                //Program.APIPassword = "j7U4LBMqEf6X";

                //Program.BaseAPIUrl = "https://api.termodom.rs";
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Program.APIUsername = "termodom_webshop_dev";
                Program.APIPassword = "j7U4LBMqEf6X";

                Program.BaseAPIUrl = "https://api.termodom.rs";
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            Program.RefreshTokenAsync().Wait();

            Program.StartAPIBearerTokenRefreshLoopAsync();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
          
            app.UseRouting();

            app.UseLSAuthorization("/IzaberiTip", "/NotAuthorized");

            // Ovim proveravam da li kontroler / akcija ima atribut [DefinisaniKorisnik]
            // AKo ima taj atribut onda dozvoljavam pristup tome samo ukoliko je klijent vec izabrao da li je profi ili jednokratni kupac
            app.Use(async (context, next) =>
            {
                var ep = context.GetEndpoint();

                if(ep != null)
                {
                    foreach (object o in ep.Metadata)
                    {
                        if (o is Attributes.DefinisaniKorisnikAttribute)
                        {
                            string tipKupcaCookie = context.Request.Cookies["tip-kupca"];
                            if (string.IsNullOrWhiteSpace(tipKupcaCookie))
                                context.Response.Cookies.Append("tip-kupca", "jednokratni");

                            // Ovo znaci da end point ima atribut DefinisaniKorisnik
                            // Proveravam da li je definisao profi / jednokratni
                            // Ako nije saljem ga tamo gde mora izabrati
                            // Ako jeste kao jednokratni pustam ga
                            // AKo jeste kao profi, proveravam da li je logovan sa izuzetkom ep-a koji sluzi za logovanje

                            if (context.GetTipKupca() == Enums.TipKupca.NULL || context.GetTipKupca() == Enums.TipKupca.Profi && Client.Get(context) == null && ep.DisplayName != "Termodom.Controllers.KorisnikController.LogovanjeValidacija (Termodom)")
                            {
                                context.Response.Redirect("/IzaberiTip");
                                return;
                            }
                        }
                    }
                }
                await next.Invoke();
            });

            // Skaldistim podatke trenutnog korisnika kao sto su client i korisnik klase da im se moze pristupiti iz bilo kog view-a uz pomoc Context.Items[item]
            app.Use(async (context, next) =>
            {
                Client client = Client.Get(context);

                if(client != null)
                {
                    Korisnik korisnik =  Korisnik.Get(client.Identifier);

                    if (korisnik != null)
                    {
                        // Azuriram last online, ali ne mogu iz buffera jer
                        // cu zeznuti ako je izmedju buffera koji je star
                        // 1 minut i ovog trenutka odradjena neka izmena
                        _ = Task.Run(() =>
                        {
                            Korisnik k = Korisnik.Get(korisnik.ID);
                            k.LogujPosetu();
                        });

                        if (context.Items.ContainsKey("korisnik"))
                            context.Items.Remove("korisnik");

                        context.Items.Add("korisnik", korisnik);
                    }
                }
                await next.Invoke();
            });

            // Povecava broj posetilaca na sajtu
            app.Use(async (context, next) =>
            {
                if(string.IsNullOrWhiteSpace(context.Request.Cookies["flag-bio-danas"]))
                {
                    context.Response.Cookies.Append("flag-bio-danas", "1", new CookieOptions() { Expires = DateTime.Now.AddHours(6) });
                    Program.PosetilacaCounter++;
                }
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
           
        }
    }
}
