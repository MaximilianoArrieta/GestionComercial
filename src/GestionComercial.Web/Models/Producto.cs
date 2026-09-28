using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Web.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0, 9999999, ErrorMessage = "El precio debe ser mayor o igual a 0")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Precio Unitario")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [Display(Name = "Stock Mínimo")]
    [Range(0, int.MaxValue)]
    public int StockMinimo { get; set; } = 5;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    // FK -> Proveedor (1 Proveedor : N Productos)
    [Required(ErrorMessage = "Debe seleccionar un proveedor")]
    [Display(Name = "Proveedor")]
    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }

    // Navegación: un producto puede estar en muchos detalles de venta
    public List<VentaDetalle> VentaDetalles { get; set; } = new();
}
