var builder = WebApplication.CreateBuilder(args);

// Add controllers (this was missing)

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Configure named HttpClient to trust all certs (for dev/self-signed)
//builder.Services.AddHttpClient("insecure")
//    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
//        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//    });


builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); 
app.MapReverseProxy();

app.Run();
