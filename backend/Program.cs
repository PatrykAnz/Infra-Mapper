using CloudBackend.Data;
using Microsoft.EntityFrameworkCore;
using CloudBackend.Models;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// --- INTEGRACJA Z MAGAZYNEM KLUCZY (KEY VAULT) ---
// W Production, gdy podano KeyVaultName, sekrety (np. DbConnectionString) są ładowane z Azure.
if (builder.Environment.IsProduction())
{
    var vaultName = builder.Configuration["KeyVaultName"];
    if (!string.IsNullOrEmpty(vaultName))
    {
        var keyVaultEndpoint = new Uri($"https://{vaultName}.vault.azure.net/");
        builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
    }
}

// --- SEKCJA USŁUG (Dependency Injection) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string: Docker (env) → Key Vault / appsettings (DbConnectionString) → DefaultConnection
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration["DbConnectionString"]
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// --- AUTOMATYCZNE TWORZENIE BAZY I DANYCH ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // context.Database.EnsureCreated();
        if (!context.Tasks.Any())
        {
            context.Tasks.AddRange(
                new CloudTask { Name = "Router", IsCompleted = true },
                new CloudTask { Name = "Komputer", IsCompleted = true },
                new CloudTask { Name = "Termometr IoT", IsCompleted = false }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Błąd podczas tworzenia bazy: {ex.Message}");
    }
}

// --- SEKCJA POTOKU HTTP (MIDDLEWARE) ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cloud API V1");
    c.RoutePrefix = string.Empty;
});
// app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.Run();
