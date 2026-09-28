using GestionComercial.Web.Data;
using GestionComercial.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Web.Services;

public class VentaService : IVentaService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public VentaService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Venta>> ObtenerTodasAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Ventas
            .AsNoTracking()
            .Include(v => v.Detalles)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }

    // Usado por la grilla interactiva de búsqueda en vivo
    public async Task<List<Venta>> BuscarAsync(string? texto)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var query = db.Ventas.AsNoTracking().Include(v => v.Detalles).AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
        {
            query = query.Where(v => v.ClienteNombre.Contains(texto));
        }

        return await query.OrderByDescending(v => v.Fecha).ToListAsync();
    }

    public async Task<Venta?> ObtenerPorIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Ventas
            .AsNoTracking()
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Venta> CrearAsync(Venta venta)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        await using var transaccion = await db.Database.BeginTransactionAsync();

        try
        {
            venta.Total = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            db.Ventas.Add(venta);
            await db.SaveChangesAsync();

            // Descontar stock de cada producto vendido
            foreach (var detalle in venta.Detalles)
            {
                var producto = await db.Productos.FindAsync(detalle.ProductoId);
                if (producto is not null)
                {
                    producto.Stock -= detalle.Cantidad;
                    if (producto.Stock < 0) producto.Stock = 0;
                }
            }

            await db.SaveChangesAsync();
            await transaccion.CommitAsync();
            return venta;
        }
        catch
        {
            await transaccion.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> AnularAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var venta = await db.Ventas.FindAsync(id);
        if (venta is null) return false;

        venta.Estado = EstadoVenta.Anulada;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var venta = await db.Ventas.Include(v => v.Detalles).FirstOrDefaultAsync(v => v.Id == id);
        if (venta is null) return false;

        db.Ventas.Remove(venta); // cascada elimina VentaDetalles
        await db.SaveChangesAsync();
        return true;
    }
}
