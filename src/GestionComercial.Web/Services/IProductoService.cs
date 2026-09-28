using GestionComercial.Web.Models;

namespace GestionComercial.Web.Services;

public interface IProductoService
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<Producto> CrearAsync(Producto producto);
    Task<bool> ActualizarAsync(Producto producto);
    Task<bool> EliminarAsync(int id);
    Task<List<Producto>> ObtenerConStockBajoAsync();
}
