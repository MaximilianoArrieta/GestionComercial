using GestionComercial.Web.Data;
using GestionComercial.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Web.Services;

public class ProductoService : IProductoService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public ProductoService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Productos
            .AsNoTracking()
            .Include(p => p.Proveedor)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Productos.AsNoTracking()
            .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producto> CrearAsync(Producto producto)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Productos.Add(producto);
        await db.SaveChangesAsync();
        return producto;
    }

    public async Task<bool> ActualizarAsync(Producto producto)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Productos.Update(producto);
        var filas = await db.SaveChangesAsync();
        return filas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var producto = await db.Productos.FindAsync(id);
        if (producto is null) return false;

        var tieneVentas = await db.VentaDetalles.AnyAsync(vd => vd.ProductoId == id);
        if (tieneVentas) return false;

        db.Productos.Remove(producto);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Producto>> ObtenerConStockBajoAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Productos
            .AsNoTracking()
            .Include(p => p.Proveedor)
            .Where(p => p.Stock <= p.StockMinimo && p.Activo)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }
}
