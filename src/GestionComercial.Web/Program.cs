using GestionComercial.Web.Components;
using GestionComercial.Web.Data;
using GestionComercial.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- Blazor Server + interactividad de cliente (SignalR circuit) ----
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ---- Acceso a datos: SQL Server 2014+ vía EF Core ----
var connectionString = builder.Configuration.GetConnectionString("GestionComercialDb")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'GestionComercialDb'.");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        // SQL Server 2014 = nivel de compatibilidad 120: EF Core evita generar
        // T-SQL de versiones posteriores (p.ej. OPENJSON).
        sql.UseCompatibilityLevel(120);
        sql.EnableRetryOnFailure(maxRetryCount: 3);
    }));

// ---- Capa de servicios (DAL) ----
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();

var app = builder.Build();

// ---- Aplica migraciones pendientes al iniciar (crea la BD si no existe) ----
// Evita ejecutar "Update-Database" a mano, tanto en local como en el hosting.
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    using var db = factory.CreateDbContext();
    db.Database.Migrate();
}

// ---- Pipeline HTTP ----
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
