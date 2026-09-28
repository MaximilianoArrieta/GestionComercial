using GestionComercial.Web.Data;
using GestionComercial.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Web.Services;

public class ProveedorService : IProveedorService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public ProveedorService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Proveedor>> ObtenerTodosAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Proveedores
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Proveedor?> ObtenerPorIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Proveedores.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Proveedor> CrearAsync(Proveedor proveedor)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Proveedores.Add(proveedor);
        await db.SaveChangesAsync();
        return proveedor;
    }

    public async Task<bool> ActualizarAsync(Proveedor proveedor)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Proveedores.Update(proveedor);
        var filas = await db.SaveChangesAsync();
        return filas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var proveedor = await db.Proveedores.FindAsync(id);
        if (proveedor is null) return false;

        // Evitar borrar si tiene productos asociados (regla de negocio simple)
        var tieneProductos = await db.Productos.AnyAsync(p => p.ProveedorId == id);
        if (tieneProductos) return false;

        db.Proveedores.Remove(proveedor);
        await db.SaveChangesAsync();
        return true;
    }
}
