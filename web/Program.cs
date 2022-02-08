#define TEST

using web.Proxy;
using Dapr.Client;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
#if DAPR_DIRECT
    .AddDapr()
#elif DAPR_SERVICE
    .AddDapr()
#else
    // No code here
#endif
;

#if DAPR_DIRECT
// No code here
#elif DAPR_SERVICE
// TODO: work on simplifying
// builder.Services.AddDaprHttpClient<IWeatherForecastClient, WeatherForecastClient>("poc-api");
builder.Services.AddSingleton<IWeatherForecastClient, WeatherForecastClient>(_ => new WeatherForecastClient(DaprClient.CreateInvokeHttpClient("poc-api")));
#else
builder.Services.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(
    (client) => { client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5100/"); }
);
#endif

// open telemetry
var serviceName = "poc-web";
var serviceVersion = "1.0.0";
builder.Services.AddOpenTelemetryTracing((builder) => builder
            .AddSource(serviceName)
            .AddAspNetCoreInstrumentation()
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddHttpClientInstrumentation()
            .AddZipkinExporter(zipkinOptions =>
            {
                zipkinOptions.Endpoint = new Uri("http://host.docker.internal:9411/api/v2/spans");
            })
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
