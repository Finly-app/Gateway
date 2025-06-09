var builder = WebApplication.CreateBuilder(args);

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

app.MapReverseProxy();
app.MapControllers();

app.Run();
