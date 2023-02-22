using Microsoft.EntityFrameworkCore;

TDBrain_v3.Settings.Reload();
TDBrain_v3.BigBrain.Start();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "all",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.CustomSchemaIds(type => type.ToString());
    c.OrderActionsBy(x => x.GroupName);
});

var app = builder.Build();

#if DEBUG
app.UseDeveloperExceptionPage();
#endif

app.UseHttpLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TDBrain v3 API V1");
    c.DefaultModelsExpandDepth(-1);
});
app.UseRouting();

app.UseEndpoints(endpointes =>
{
    endpointes.MapControllers();
});

app.Run();