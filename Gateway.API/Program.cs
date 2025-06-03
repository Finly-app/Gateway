using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

var certPath = builder.Configuration["ASPNETCORE_Kestrel__Certificates__Default__Path"];
var certPassword = builder.Configuration["ASPNETCORE_Kestrel__Certificates__Default__Password"];

if (!string.IsNullOrEmpty(certPath) && File.Exists(certPath)) {
    builder.WebHost.ConfigureKestrel(options => {
        options.ConfigureHttpsDefaults(httpsOptions => {
            httpsOptions.ServerCertificate = new X509Certificate2(certPath, certPassword);
        });
    });
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapReverseProxy();

app.Run();
