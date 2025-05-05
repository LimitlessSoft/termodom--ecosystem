using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
				c.CustomSchemaIds(type => type.ToString());
				c.OrderActionsBy(x => x.GroupName);
			});

			File.AppendAllLines(
				Path.Combine(AppContext.BaseDirectory, "startedLog.txt"),
				new List<string>()
				{
					"Started: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")
				}
			);

			services.AddCors(options =>
			{
				options.AddPolicy(
					name: "all",
					policy =>
					{
						policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
					}
				);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors("all");
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Termodom API V1");
				c.DefaultModelsExpandDepth(-1);
			});

			app.UseRouting();

			app.Use(
				async (context, next) =>
				{
					var ep = context.GetEndpoint();

					if (ep != null)
					{
						// Checking if any attribute is APIAuthorization
						// If it is then i will check if client send token and if his account has access to endpoint
						// Otherwise i will let anyone to that endpoint
						foreach (object o in ep.Metadata)
						{
							if (o is RequireBearer)
							{
								string fullAuth = context
									.Request.Headers["Authorization"]
									.ToStringOrDefault();

								if (string.IsNullOrWhiteSpace(fullAuth))
								{
									context.Response.StatusCode = 403;
									return;
								}

								string[] els = fullAuth.Split(' ');

								if (
									els[0].ToLower() == "bearer"
									&& Program.GetSessions().ContainsValue(els[1])
								)
								{
									break;
								}
								else
								{
									context.Response.StatusCode = 403;
									return;
								}
							}
						}
					}

					await next.Invoke();
				}
			);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
