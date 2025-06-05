var builder = WebApplication.CreateBuilder(args);

// Add controllers (this was missing)

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.WebHost.ConfigureKestrel(options => {
//    options.ListenAnyIP(8080);
//});

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapReverseProxy();

app.Run();
