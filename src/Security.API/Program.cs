using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using N5.Challenge.Services.Security.API;
using N5.Challenge.Services.Security.API.Infrastructure.AutofacModules;
using N5.Challenge.Services.Security.API.Middlewares;
using N5.Challenge.Services.Security.Domain.Repositories;
using N5.Challenge.Services.Security.Infrastructure;
using N5.Challenge.Services.Security.Infrastructure.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Services.AddDbContext<SecurityContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("Database")));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddSingleton<LoggingMiddleware>();
builder.Services.AddSingleton<ProducerMiddleware>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

builder.Services.AddElasticsearch(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "Security API",
        Version = "v1",
        Description = "Specifying services for security.",
        Contact = new OpenApiContact { Name = "N5" }            
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

//
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console());

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.DocumentTitle = "N5 - SecurityAPI";
        c.RoutePrefix = string.Empty;
        c.SupportedSubmitMethods(Array.Empty<SubmitMethod>());
        c.SwaggerEndpoint(configuration["SwaggerEndPoint"], "Security V1");
        c.DefaultModelsExpandDepth(-1);
    });

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ProducerMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
