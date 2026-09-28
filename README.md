# Gestión Comercial — Blazor Server (.NET 10) + SQL Server 2014

Proyecto CRUD completo con **Proveedores**, **Productos** y **Ventas** (con
detalle de línea), usando Blazor Server con render mode `InteractiveServer`
en las pantallas que lo requieren, EF Core como capa de acceso a datos, y
Bootstrap 5 para la interfaz.
---

Estructura del proyecto

```
GestionComercial/
├── GestionComercial.sln
├── docs/
│   ├── diagrama-er.md              # Diagrama ER (Mermaid)
│   ├── diagrama-er.png             # Diagrama ER (imagen)
│   └── script-sql-referencia.sql   # Script SQL equivalente a la migración
└── src/
    └── GestionComercial.Web/
        ├── Program.cs
        ├── appsettings.json                # Cadena de conexión (producción)
        ├── appsettings.Development.json    # Cadena de conexión (local)
        ├── Models/                         # Entidades (Proveedor, Producto, Venta, VentaDetalle)
        ├── Data/ApplicationDbContext.cs     # DbContext EF Core
        ├── Services/                        # Capa de acceso a datos (DAL) por entidad
        ├── Components/
        │   ├── App.razor / Routes.razor
        │   ├── Layout/ (MainLayout, NavMenu)
        │   └── Pages/
        │       ├── Home.razor              # Dashboard interactivo (stock bajo)
        │       ├── Proveedores/Index.razor # CRUD Proveedores
        │       ├── Productos/Index.razor   # CRUD Productos
        │       └── Ventas/                 # Listado + Alta de Ventas
        └── wwwroot/css/site.css
```
Resumen de entidades de negocio

| Entidad | Descripción | Campos clave |
|---|---|---|
| **Proveedor** | Empresas que proveen productos | Nombre, CUIT (único), Teléfono, Email |
| **Producto** | Catálogo de productos, ligado a un proveedor | Nombre, Precio, Stock, StockMínimo, ProveedorId |
| **Venta** | Cabecera de una venta a un cliente | Fecha, Cliente, Total, Estado |
| **VentaDetalle** | Líneas de una venta (producto + cantidad) | VentaId, ProductoId, Cantidad, PrecioUnitario |
