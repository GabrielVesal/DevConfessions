using DevConfessions.Services;
using Firebase.Database;
using Firebase.Database.Query;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração essencial de serviços
builder.Services.AddControllersWithViews();

// 2. Configuração do Firebase
var firebaseUrl = Environment.GetEnvironmentVariable("FIREBASE_DATABASE_URL")
    ?? throw new InvalidOperationException("Firebase DatabaseUrl not configured");
var firebaseSecret = Environment.GetEnvironmentVariable("FIREBASE_SECRET")
    ?? throw new InvalidOperationException("Firebase Secret not configured");


// Configura o cliente Firebase
builder.Services.AddSingleton<FirebaseClient>(_ =>
    new FirebaseClient(
        firebaseUrl,
        new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(firebaseSecret)
        }));

// 3. Registro do serviço (mantemos o mesmo nome, mas agora usando Firebase)
builder.Services.AddSingleton<JsonConfessionService>();

var app = builder.Build();

// Restante da configuração permanece igual
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Confessions}/{action=Index}/{id?}");

app.Run();
