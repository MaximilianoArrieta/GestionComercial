using GestionComercial.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaDetalle> VentaDetalles => Set<VentaDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Proveedor ----
        modelBuilder.Entity<Proveedor>(e =>
        {
            e.ToTable("Proveedores");
            e.HasIndex(p => p.Cuit).IsUnique();
        });

        // ---- Producto ----
        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("Productos");
            e.HasOne(p => p.Proveedor)
             .WithMany(pr => pr.Productos)
             .HasForeignKey(p => p.ProveedorId)
             .OnDelete(DeleteBehavior.Restrict); // no permite borrar proveedor con productos asociados
        });

        // ---- Venta ----
        modelBuilder.Entity<Venta>(e =>
        {
            e.ToTable("Ventas");
        });

        // ---- VentaDetalle ----
        modelBuilder.Entity<VentaDetalle>(e =>
        {
            e.ToTable("VentaDetalles");

            e.HasOne(vd => vd.Venta)
             .WithMany(v => v.Detalles)
             .HasForeignKey(vd => vd.VentaId)
             .OnDelete(DeleteBehavior.Cascade); // al borrar la venta, se borran sus líneas

            e.HasOne(vd => vd.Producto)
             .WithMany(p => p.VentaDetalles)
             .HasForeignKey(vd => vd.ProductoId)
             .OnDelete(DeleteBehavior.Restrict); // no permite borrar producto vendido

            e.Ignore(vd => vd.Subtotal); // es una propiedad calculada, no se persiste
        });

        // ---- Datos semilla opcionales (comentar si no se desea) ----
        modelBuilder.Entity<Proveedor>().HasData(
            new Proveedor { Id = 1, Nombre = "Distribuidora Central S.A.", Cuit = "30-12345678-9", Telefono = "011-4444-5555", Email = "ventas@distcentral.com", Direccion = "Av. Siempre Viva 123", Activo = true },
            new Proveedor { Id = 2, Nombre = "Insumos del Sur S.R.L.", Cuit = "30-98765432-1", Telefono = "0299-444-3333", Email = "contacto@insumossur.com", Direccion = "Ruta 22 Km 15", Activo = true }
        );
    }
}
