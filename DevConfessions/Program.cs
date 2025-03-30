using DevConfessions.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<JsonConfessionService>();

// Antes de builder.Build():
var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();
