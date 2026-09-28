using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Web.Models;

public class Proveedor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    [Display(Name = "Razón Social")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El CUIT es obligatorio")]
    [StringLength(20)]
    [Display(Name = "CUIT")]
    public string Cuit { get; set; } = string.Empty;

    [StringLength(30)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Direccion { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    // Navegación: un proveedor provee muchos productos
    public List<Producto> Productos { get; set; } = new();
}
