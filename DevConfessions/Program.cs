using DevConfessions.Services;

var builder = WebApplication.CreateBuilder(args);

// Pasta persistente padrão do Render Free Tier
var dataDir = Environment.GetEnvironmentVariable("JSON_STORAGE_PATH") 
    ?? Path.Combine(Directory.GetCurrentDirectory(), "data");

if (!Directory.Exists(dataDir)) 
    Directory.CreateDirectory(dataDir);

builder.Services.AddSingleton<JsonConfessionService>(_ => 
    new JsonConfessionService(Path.Combine(dataDir, "confessions.json")));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();          
