using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Web.Models;

public enum EstadoVenta
{
    Pendiente = 0,
    Confirmada = 1,
    Anulada = 2
}

public class Venta
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El cliente es obligatorio")]
    [StringLength(150)]
    [Display(Name = "Cliente")]
    public string ClienteNombre { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    [Display(Name = "Estado")]
    public EstadoVenta Estado { get; set; } = EstadoVenta.Pendiente;

    // Navegación: 1 Venta : N VentaDetalle
    public List<VentaDetalle> Detalles { get; set; } = new();
}
