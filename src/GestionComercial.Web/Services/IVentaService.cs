using GestionComercial.Web.Models;

namespace GestionComercial.Web.Services;

public interface IVentaService
{
    Task<List<Venta>> ObtenerTodasAsync();
    Task<List<Venta>> BuscarAsync(string? texto);
    Task<Venta?> ObtenerPorIdAsync(int id);
    Task<Venta> CrearAsync(Venta venta);
    Task<bool> AnularAsync(int id);
    Task<bool> EliminarAsync(int id);
}
