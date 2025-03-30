using DevConfessions.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<JsonConfessionService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();