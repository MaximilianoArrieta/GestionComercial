# Gestión Comercial — Blazor Server (.NET 10) + SQL Server 2014

Proyecto CRUD completo con **Proveedores**, **Productos** y **Ventas** (con
detalle de línea), usando Blazor Server con render mode `InteractiveServer`
en las pantallas que lo requieren, EF Core como capa de acceso a datos, y
Bootstrap 5 para la interfaz.

> **Nota:** el código de este repositorio fue escrito a mano (no se compiló
> ni ejecutó en el entorno donde se generó, por pedido explícito). Seguí
> esta guía paso a paso en tu máquina para compilar, migrar y correr el
> proyecto.

---

## 1. Estructura del proyecto

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

---

## 2. Requisitos previos

| Herramienta | Versión | Notas |
|---|---|---|
| Visual Studio | 2026 (o la versión que soporte .NET 10) | Carga de trabajo **"ASP.NET y desarrollo web"**. Incluye el SDK de .NET |
| SQL Server | 2014 | Express, LocalDB o instancia completa |
| SSMS (opcional) | cualquiera | Para ver las tablas creadas |

---

## 3. Levantar el proyecto en Visual Studio (1 solo comando)

### Paso 1 — Abrir la solución
`Archivo > Abrir > Proyecto o solución` → seleccionar `GestionComercial.sln`.
Visual Studio restaura los paquetes NuGet solo al abrirla.

### Paso 2 — Configurar la conexión
Abrir `appsettings.Development.json` (en el Explorador de soluciones) y
ajustar `GestionComercialDb` a tu SQL Server 2014.
Para saber el nombre del servidor: `Ver > Explorador de objetos de SQL Server`
o el cuadro de conexión de SSMS.
Con autenticación de Windows: `Server=.\\SQLEXPRESS;Database=GestionComercialDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`

### Paso 3 — Crear la migración (único comando)
`Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes`,
seleccionar como *Proyecto predeterminado* `GestionComercial.Web` y ejecutar:

```powershell
Add-Migration InitialCreate -OutputDir Data/Migrations
```

Esto genera la carpeta `Data/Migrations` (aparece en el Explorador de soluciones).
**No hace falta ejecutar `Update-Database`**: `Program.cs` aplica las migraciones
al iniciar y crea la base `GestionComercialDb` si no existe.

### Paso 4 — Ejecutar
Presionar **F5** (perfil `https`). Se abre el navegador y en el primer arranque
se crea la base con sus tablas y los 2 proveedores semilla.
Verificalo en `Explorador de objetos de SQL Server > (localdb o tu instancia) > Bases de datos`.

### Paso 5 — Probar el flujo
1. **Proveedores**: alta/edición/baja (modal Bootstrap).
2. **Productos**: alta asociada a un proveedor.
3. **Ventas > Nueva Venta**: cliente + líneas de detalle; descuenta stock al confirmar.
4. **Dashboard**: mover el slider de umbral y ver cómo se recalcula el panel de stock bajo sin recargar (InteractiveServer).

---

## 4. Diagrama Entidad-Relación

Ver `docs/diagrama-er.png` (imagen) y `docs/diagrama-er.md` (fuente Mermaid
editable). Resumen de relaciones:

- **Proveedor 1 — N Producto** (`Restrict`: no se borra un proveedor con
  productos asociados)
- **Venta 1 — N VentaDetalle** (`Cascade`: al borrar la venta se borran sus
  líneas)
- **Producto 1 — N VentaDetalle** (`Restrict`: no se borra un producto con
  ventas asociadas)

---

## 6. Repositorio Git (desde Visual Studio, sin comandos)

El repositorio local ya está inicializado con commits. Para publicarlo:
1. `Git > Crear repositorio Git...` (o `Git > Configuración`) en Visual Studio.
2. Elegir **GitHub** e iniciar sesión, nombre `gestion-comercial`, `Insertar y publicar`.
3. Los cambios posteriores: ventana **Cambios de Git** → mensaje → `Confirmar todo y Insertar`.

---

## 7. Despliegue gratuito con SQL Server: Somee.com (desde Visual Studio)

Somee ofrece hosting ASP.NET y base MSSQL gratuitos, la opción más fiel a SQL Server 2014.

**A. Preparar en Somee (panel web)**
1. Crear cuenta y un sitio web (Windows/ASP.NET, runtime .NET; verificar en el
   panel que soporta .NET 10. Si no, ver la nota de abajo).
2. Crear una base de datos MSSQL en el panel y copiar su cadena de conexión.
3. Copiar los datos FTP/Web Deploy del sitio.

**B. Publicar con el asistente**
1. Clic derecho en el proyecto > **Publicar...** > **Nuevo perfil** > **FTP/FTPS** (o *Web Deploy* si Somee lo habilita).
2. Cargar servidor, sitio, usuario y contraseña de Somee > **Finalizar**.
3. En el perfil: **Configuración** > `Modo de implementación: Dependiente del marco`, `Configuración: Release`.
4. En *Bases de datos / Cadenas de conexión* del perfil, pegar la cadena de la base de Somee para `GestionComercialDb`
   (o editar `appsettings.json` con ella).
5. **Publicar**. En el primer acceso, `Program.cs` crea las tablas en la base de Somee automáticamente.

> **Nota:** si el plan gratuito de Somee no soporta aún .NET 10, publicá como
> *Autocontenido* (`Modo de implementación: Autocontenido`, runtime `win-x86`/`win-x64`) desde el mismo asistente.
> Alternativa: Azure App Service F1 (gratis) con el asistente *Azure* de Publicar
> + Azure SQL free; en ese caso el motor de producción es Azure SQL, no 2014.

---

## 8. Capturas de pantalla

No pude ejecutar la app en mi entorno, así que las capturas deben tomarse
en tu máquina tras el Paso 4 (tecla `Win+Shift+S`). Guardarlas en `docs/capturas/`:
Dashboard, Proveedores (con modal), Productos, Ventas y Nueva Venta con 2+ líneas.

---

## 9. Resumen de entidades de negocio

| Entidad | Descripción | Campos clave |
|---|---|---|
| **Proveedor** | Empresas que proveen productos | Nombre, CUIT (único), Teléfono, Email |
| **Producto** | Catálogo de productos, ligado a un proveedor | Nombre, Precio, Stock, StockMínimo, ProveedorId |
| **Venta** | Cabecera de una venta a un cliente | Fecha, Cliente, Total, Estado |
| **VentaDetalle** | Líneas de una venta (producto + cantidad) | VentaId, ProductoId, Cantidad, PrecioUnitario |
