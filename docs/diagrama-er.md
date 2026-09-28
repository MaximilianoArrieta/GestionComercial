# Diagrama Entidad-Relación — Gestión Comercial

```mermaid
erDiagram
    PROVEEDORES ||--o{ PRODUCTOS : "provee"
    PRODUCTOS ||--o{ VENTA_DETALLES : "se vende en"
    VENTAS ||--o{ VENTA_DETALLES : "contiene"

    PROVEEDORES {
        int Id PK
        nvarchar Nombre
        nvarchar Cuit UK
        nvarchar Telefono
        nvarchar Email
        nvarchar Direccion
        bit Activo
    }

    PRODUCTOS {
        int Id PK
        nvarchar Nombre
        nvarchar Descripcion
        decimal Precio
        int Stock
        int StockMinimo
        bit Activo
        int ProveedorId FK
    }

    VENTAS {
        int Id PK
        datetime Fecha
        nvarchar ClienteNombre
        decimal Total
        int Estado
    }

    VENTA_DETALLES {
        int Id PK
        int VentaId FK
        int ProductoId FK
        int Cantidad
        decimal PrecioUnitario
    }
```

## Cardinalidades

| Relación | Tipo | Regla de negocio |
|---|---|---|
| Proveedor → Producto | 1 : N | Un proveedor puede tener muchos productos. No se permite borrar un proveedor con productos asociados (`DeleteBehavior.Restrict`). |
| Venta → VentaDetalle | 1 : N | Una venta tiene una o más líneas de detalle. Al borrar la venta se borran sus líneas en cascada (`DeleteBehavior.Cascade`). |
| Producto → VentaDetalle | 1 : N | Un producto puede aparecer en muchas líneas de venta. No se permite borrar un producto con ventas asociadas (`DeleteBehavior.Restrict`). |

## Cómo visualizar / exportar la imagen

1. Pegar el bloque `mermaid` de arriba en https://mermaid.live
2. Exportar como PNG/SVG desde ese sitio, o
3. En VS Code, instalar la extensión **"Markdown Preview Mermaid Support"** y previsualizar este archivo.
