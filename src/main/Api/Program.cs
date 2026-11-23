using CorrelationId;
using NttBankApi.Application;
using NttBankApi.Infrastructure.Configurations;
using NttBankApi.Infrastructure.Extensions;

var builder = WebApplication
    .CreateSlimBuilder(args);

builder.Services.AddAppConfigs(builder.Configuration);

var appConfiguration = builder.Configuration
    .Get<AppConfiguration>()!;

builder.AddLogger(appConfiguration);

builder.Services
    .AddAppAuthentication(appConfiguration)
    .AddAppAuthorization(appConfiguration)
    .AddCached(appConfiguration)
    .AddJsonOptionsSerialize()
    .AddCorrelation()
    .AddCultureConfiguration()
    .AddMapper(AssemblyReference.Assembly)
    .AddDatabase(appConfiguration, builder.Environment)
    .AddApplicationServices()
    .AddOpenTelemetry(appConfiguration)
    .AddHealthCheck(appConfiguration);

var app = builder.Build();

app.UseCorrelationId();
app.UseRouting();
app.AddHealthCheckEndpoints();
app.UseOpenTelemetryPrometheusScrapingEndpoint("metrics");
app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();

public partial class Program;
