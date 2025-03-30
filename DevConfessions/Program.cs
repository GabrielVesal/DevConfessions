using DevConfessions.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração essencial de serviços
builder.Services.AddControllersWithViews(); // Adiciona suporte a controllers e views

// 2. Configuração de persistência (Render Free Tier)
var dataDir = Environment.GetEnvironmentVariable("JSON_STORAGE_PATH") 
    ?? Path.Combine(Directory.GetCurrentDirectory(), "data");

if (!Directory.Exists(dataDir)) 
    Directory.CreateDirectory(dataDir);

var jsonPath = Path.Combine(dataDir, "confessions.json");

// 3. Registro do serviço com caminho absoluto
builder.Services.AddSingleton<JsonConfessionService>(_ => 
    new JsonConfessionService(jsonPath));

var app = builder.Build();

// 4. Pipeline de middlewares (ORDEM IMPORTANTE!)
app.UseStaticFiles();
app.UseRouting(); // Necessário antes do mappeing de rotas

// 5. Configuração de rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();
