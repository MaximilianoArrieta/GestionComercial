using GestionComercial.Web.Models;

namespace GestionComercial.Web.Services;

public interface IProveedorService
{
    Task<List<Proveedor>> ObtenerTodosAsync();
    Task<Proveedor?> ObtenerPorIdAsync(int id);
    Task<Proveedor> CrearAsync(Proveedor proveedor);
    Task<bool> ActualizarAsync(Proveedor proveedor);
    Task<bool> EliminarAsync(int id);
}
