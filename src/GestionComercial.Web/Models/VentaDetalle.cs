using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Web.Models;

public class VentaDetalle
{
    public int Id { get; set; }

    // FK -> Venta (1 Venta : N VentaDetalle)
    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    // FK -> Producto (1 Producto : N VentaDetalle)
    [Required(ErrorMessage = "Debe seleccionar un producto")]
    [Display(Name = "Producto")]
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Precio Unitario")]
    public decimal PrecioUnitario { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal => Cantidad * PrecioUnitario;
}
