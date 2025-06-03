var builder = WebApplication.CreateBuilder(args);

// Configure named HttpClient that skips SSL validation
builder.Services
    .AddHttpClient("insecure")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

// Add Reverse Proxy with config
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapReverseProxy();

app.Run();
