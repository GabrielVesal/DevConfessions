using DevConfessions.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<JsonConfessionService>();

var dataPath = Path.Combine(app.Environment.WebRootPath, "data");
if (!Directory.Exists(dataPath)) 
{
    Directory.CreateDirectory(dataPath);
}

var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();
