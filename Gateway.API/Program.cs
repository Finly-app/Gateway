var builder = WebApplication.CreateBuilder(args);

// Add controllers (this was missing)
builder.Services.AddControllers();

// Insecure HttpClient for self-signed internal certs
builder.Services
    .AddHttpClient("insecure")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

// Reverse proxy with config
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // <-- requires AddControllers() above
app.MapReverseProxy();

app.Run();
