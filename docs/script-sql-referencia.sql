-- =====================================================================
-- Script de referencia — equivalente a la migración inicial de EF Core
-- Compatible con SQL Server 2014 (nivel de compatibilidad 120)
-- NO es necesario ejecutarlo a mano: "dotnet ef database update" lo
-- genera y aplica automáticamente. Se deja como documentación /
-- respaldo por si preferís crear el esquema manualmente.
-- =====================================================================

IF DB_ID('GestionComercialDb') IS NULL
BEGIN
    CREATE DATABASE GestionComercialDb;
END
GO

USE GestionComercialDb;
GO

CREATE TABLE Proveedores (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre        NVARCHAR(150) NOT NULL,
    Cuit          NVARCHAR(20)  NOT NULL,
    Telefono      NVARCHAR(30)  NULL,
    Email         NVARCHAR(150) NULL,
    Direccion     NVARCHAR(200) NULL,
    Activo        BIT NOT NULL DEFAULT(1),
    CONSTRAINT UQ_Proveedores_Cuit UNIQUE (Cuit)
);
GO

CREATE TABLE Productos (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre        NVARCHAR(150) NOT NULL,
    Descripcion   NVARCHAR(500) NULL,
    Precio        DECIMAL(18,2) NOT NULL,
    Stock         INT NOT NULL,
    StockMinimo   INT NOT NULL DEFAULT(5),
    Activo        BIT NOT NULL DEFAULT(1),
    ProveedorId   INT NOT NULL,
    CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (ProveedorId)
        REFERENCES Proveedores(Id) ON DELETE NO ACTION
);
GO

CREATE TABLE Ventas (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Fecha         DATETIME NOT NULL,
    ClienteNombre NVARCHAR(150) NOT NULL,
    Total         DECIMAL(18,2) NOT NULL DEFAULT(0),
    Estado        INT NOT NULL DEFAULT(0)
);
GO

CREATE TABLE VentaDetalles (
    Id             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    VentaId        INT NOT NULL,
    ProductoId     INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_VentaDetalles_Ventas FOREIGN KEY (VentaId)
        REFERENCES Ventas(Id) ON DELETE CASCADE,
    CONSTRAINT FK_VentaDetalles_Productos FOREIGN KEY (ProductoId)
        REFERENCES Productos(Id) ON DELETE NO ACTION
);
GO

-- Datos semilla opcionales (equivalentes a HasData en ApplicationDbContext)
INSERT INTO Proveedores (Nombre, Cuit, Telefono, Email, Direccion, Activo)
VALUES
    ('Distribuidora Central S.A.', '30-12345678-9', '011-4444-5555', 'ventas@distcentral.com', 'Av. Siempre Viva 123', 1),
    ('Insumos del Sur S.R.L.', '30-98765432-1', '0299-444-3333', 'contacto@insumossur.com', 'Ruta 22 Km 15', 1);
GO
