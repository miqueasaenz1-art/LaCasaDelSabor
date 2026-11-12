/* ============================================================
   BASE DE DATOS: LaCasaDelSabor
   Script completo listo para ejecutar en SQL Server (SSMS)
   ============================================================ */

IF DB_ID('LaCasaDelSabor') IS NULL
BEGIN
    CREATE DATABASE LaCasaDelSabor;
END
GO

USE LaCasaDelSabor;
GO

/* ============================================================
   1. TABLA CLIENTES
   ============================================================ */
IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes (
        id_cliente INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        telefono VARCHAR(20) NULL,
        correo VARCHAR(100) NULL
    );
END
GO

-- Índice único filtrado: permite varios clientes sin correo (NULL), pero no correos repetidos
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_Clientes_Correo' AND object_id = OBJECT_ID('dbo.Clientes')
)
BEGIN
    CREATE UNIQUE INDEX UX_Clientes_Correo
    ON dbo.Clientes(correo)
    WHERE correo IS NOT NULL;
END
GO

/* ============================================================
   2. TABLA CATEGORIAS
   ============================================================ */
IF OBJECT_ID('dbo.Categorias', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categorias (
        id_categoria INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(50) NOT NULL UNIQUE,
        descripcion VARCHAR(150) NULL
    );
END
GO

/* ============================================================
   3. TABLA PRODUCTOS
   ============================================================ */
IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        id_producto INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL UNIQUE,
        descripcion VARCHAR(200) NULL,
        precio DECIMAL(10,2) NOT NULL,
        disponibilidad BIT NOT NULL CONSTRAINT DF_Productos_Disponibilidad DEFAULT 1,
        id_categoria INT NOT NULL,

        CONSTRAINT FK_Productos_Categorias
            FOREIGN KEY (id_categoria)
            REFERENCES dbo.Categorias(id_categoria),

        CONSTRAINT CK_Productos_Precio
            CHECK (precio >= 0)
    );
END
GO

/* ============================================================
   4. TABLA PEDIDOS
   ============================================================ */
IF OBJECT_ID('dbo.Pedidos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pedidos (
        id_pedido INT IDENTITY(1,1) PRIMARY KEY,
        id_cliente INT NULL CONSTRAINT DF_Pedidos_Cliente DEFAULT 1,
        fecha_pedido DATETIME NOT NULL CONSTRAINT DF_Pedidos_FechaPedido DEFAULT GETDATE(),
        fecha_entrega DATETIME NULL,
        estado VARCHAR(30) NOT NULL CONSTRAINT DF_Pedidos_Estado DEFAULT 'Pendiente',
        total DECIMAL(10,2) NOT NULL CONSTRAINT DF_Pedidos_Total DEFAULT 0,

        CONSTRAINT FK_Pedidos_Clientes
            FOREIGN KEY (id_cliente)
            REFERENCES dbo.Clientes(id_cliente),

        CONSTRAINT CK_Pedidos_Total
            CHECK (total >= 0),

        CONSTRAINT CK_Pedidos_Estado
            CHECK (estado IN (
                'Pendiente',
                'En preparación',
                'Listo',
                'Entregado',
                'Cancelado'
            ))
    );
END
GO

/* ============================================================
   5. TABLA DETALLE PEDIDO
   ============================================================ */
IF OBJECT_ID('dbo.Detalle_Pedido', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Detalle_Pedido (
        id_detalle INT IDENTITY(1,1) PRIMARY KEY,
        id_pedido INT NOT NULL,
        id_producto INT NOT NULL,
        cantidad INT NOT NULL,
        precio_unitario DECIMAL(10,2) NOT NULL,
        subtotal DECIMAL(10,2) NOT NULL,

        CONSTRAINT FK_Detalle_Pedido
            FOREIGN KEY (id_pedido)
            REFERENCES dbo.Pedidos(id_pedido),

        CONSTRAINT FK_Detalle_Producto
            FOREIGN KEY (id_producto)
            REFERENCES dbo.Productos(id_producto),

        CONSTRAINT CK_Detalle_Cantidad
            CHECK (cantidad > 0),

        CONSTRAINT CK_Detalle_Precio
            CHECK (precio_unitario >= 0),

        CONSTRAINT CK_Detalle_Subtotal
            CHECK (subtotal >= 0)
    );
END
GO

/* ============================================================
   6. TABLA PAGOS
   ============================================================ */
IF OBJECT_ID('dbo.Pagos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pagos (
        id_pago INT IDENTITY(1,1) PRIMARY KEY,
        id_pedido INT NOT NULL,
        fecha_pago DATETIME NOT NULL CONSTRAINT DF_Pagos_FechaPago DEFAULT GETDATE(),
        monto DECIMAL(10,2) NOT NULL,
        metodo_pago VARCHAR(30) NOT NULL,

        CONSTRAINT FK_Pagos_Pedidos
            FOREIGN KEY (id_pedido)
            REFERENCES dbo.Pedidos(id_pedido),

        CONSTRAINT CK_Pagos_Monto
            CHECK (monto > 0),

        CONSTRAINT CK_Pagos_Metodo
            CHECK (metodo_pago IN (
                'Efectivo',
                'Tarjeta',
                'Transferencia'
            ))
    );
END
GO

/* ============================================================
   7. TABLA PARA LOG DE ERRORES
   ============================================================ */
IF OBJECT_ID('dbo.LogErrores', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LogErrores (
        id_error INT IDENTITY(1,1) PRIMARY KEY,
        fecha_error DATETIME NOT NULL CONSTRAINT DF_LogErrores_Fecha DEFAULT GETDATE(),
        procedimiento VARCHAR(150),
        numero_error INT,
        severidad INT,
        estado_error INT,
        mensaje VARCHAR(4000)
    );
END
GO

/* ============================================================
   8. TABLA USUARIOS (Login)
   ============================================================ */
IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        id_usuario INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        correo NVARCHAR(100) NOT NULL UNIQUE,
        contrasena NVARCHAR(100) NOT NULL,
        rol NVARCHAR(50) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT 'Cajero',
        activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1,
        fecha_creacion DATETIME NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT GETDATE()
    );
END
GO

IF COL_LENGTH('dbo.Usuarios', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios
    ADD activo BIT NOT NULL CONSTRAINT DF_Usuarios_ActivoCol DEFAULT 1;
END
GO

/* ============================================================
   9. INDICES
   ============================================================ */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Productos_Categoria' AND object_id = OBJECT_ID('dbo.Productos'))
    CREATE INDEX IX_Productos_Categoria ON dbo.Productos(id_categoria);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pedidos_Cliente' AND object_id = OBJECT_ID('dbo.Pedidos'))
    CREATE INDEX IX_Pedidos_Cliente ON dbo.Pedidos(id_cliente);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pedidos_FechaEntrega' AND object_id = OBJECT_ID('dbo.Pedidos'))
    CREATE INDEX IX_Pedidos_FechaEntrega ON dbo.Pedidos(fecha_entrega);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetallePedido_Pedido' AND object_id = OBJECT_ID('dbo.Detalle_Pedido'))
    CREATE INDEX IX_DetallePedido_Pedido ON dbo.Detalle_Pedido(id_pedido);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetallePedido_Producto' AND object_id = OBJECT_ID('dbo.Detalle_Pedido'))
    CREATE INDEX IX_DetallePedido_Producto ON dbo.Detalle_Pedido(id_producto);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pagos_Pedido' AND object_id = OBJECT_ID('dbo.Pagos'))
    CREATE INDEX IX_Pagos_Pedido ON dbo.Pagos(id_pedido);
GO

/* ============================================================
   10. PROCEDIMIENTOS - CLIENTES
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Insertar
    @nombre VARCHAR(100),
    @telefono VARCHAR(20),
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Clientes(nombre, telefono, correo)
        VALUES (@nombre, @telefono, @correo);

        SELECT SCOPE_IDENTITY() AS id_cliente;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Cliente_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Actualizar
    @id_cliente INT,
    @nombre VARCHAR(100),
    @telefono VARCHAR(20),
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Clientes
        SET nombre = @nombre,
            telefono = @telefono,
            correo = @correo
        WHERE id_cliente = @id_cliente;

        IF @@ROWCOUNT = 0
            THROW 50001, 'El cliente no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Cliente_Actualizar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Eliminar
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Clientes
        WHERE id_cliente = @id_cliente;

        IF @@ROWCOUNT = 0
            THROW 50002, 'El cliente no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Cliente_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Clientes
    ORDER BY id_cliente;
END;
GO

/* ============================================================
   11. PROCEDIMIENTOS - CATEGORIAS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Insertar
    @nombre VARCHAR(50),
    @descripcion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Categorias(nombre, descripcion)
        VALUES (@nombre, @descripcion);

        SELECT SCOPE_IDENTITY() AS id_categoria;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Categoria_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Actualizar
    @id_categoria INT,
    @nombre VARCHAR(50),
    @descripcion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Categorias
        SET nombre = @nombre,
            descripcion = @descripcion
        WHERE id_categoria = @id_categoria;

        IF @@ROWCOUNT = 0
            THROW 50003, 'La categoría no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Categoria_Actualizar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Eliminar
    @id_categoria INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Categorias
        WHERE id_categoria = @id_categoria;

        IF @@ROWCOUNT = 0
            THROW 50004, 'La categoría no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Categoria_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Categoria_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Categorias
    ORDER BY id_categoria;
END;
GO

/* ============================================================
   12. PROCEDIMIENTOS - PRODUCTOS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Producto_Insertar
    @nombre VARCHAR(100),
    @descripcion VARCHAR(200),
    @precio DECIMAL(10,2),
    @disponibilidad BIT,
    @id_categoria INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Productos
        (nombre, descripcion, precio, disponibilidad, id_categoria)
        VALUES
        (@nombre, @descripcion, @precio, @disponibilidad, @id_categoria);

        SELECT SCOPE_IDENTITY() AS id_producto;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Producto_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Producto_Actualizar
    @id_producto INT,
    @nombre VARCHAR(100),
    @descripcion VARCHAR(200),
    @precio DECIMAL(10,2),
    @disponibilidad BIT,
    @id_categoria INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Productos
        SET nombre = @nombre,
            descripcion = @descripcion,
            precio = @precio,
            disponibilidad = @disponibilidad,
            id_categoria = @id_categoria
        WHERE id_producto = @id_producto;

        IF @@ROWCOUNT = 0
            THROW 50005, 'El producto no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Producto_Actualizar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Producto_Eliminar
    @id_producto INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Productos
        WHERE id_producto = @id_producto;

        IF @@ROWCOUNT = 0
            THROW 50006, 'El producto no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Producto_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Producto_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto,
        p.nombre,
        p.descripcion,
        p.precio,
        p.disponibilidad,
        c.nombre AS categoria
    FROM dbo.Productos p
    INNER JOIN dbo.Categorias c
        ON p.id_categoria = c.id_categoria
    ORDER BY p.id_producto;
END;
GO

/* ============================================================
   13. PROCEDIMIENTOS - PEDIDOS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Insertar
    @id_cliente INT,
    @fecha_entrega DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Pedidos (id_cliente, fecha_entrega)
        VALUES (@id_cliente, @fecha_entrega);

        SELECT SCOPE_IDENTITY() AS id_pedido;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Pedido_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Actualizar
    @id_pedido INT,
    @id_cliente INT,
    @fecha_entrega DATETIME,
    @estado VARCHAR(30),
    @total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.Pedidos
        SET id_cliente = @id_cliente,
            fecha_entrega = @fecha_entrega,
            estado = @estado,
            total = @total
        WHERE id_pedido = @id_pedido;

        IF @@ROWCOUNT = 0
            THROW 50007, 'El pedido no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Pedido_Actualizar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Eliminar
    @id_pedido INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Pedidos
        WHERE id_pedido = @id_pedido;

        IF @@ROWCOUNT = 0
            THROW 50008, 'El pedido no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Pedido_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_pedido,
        c.nombre AS cliente,
        p.fecha_pedido,
        p.fecha_entrega,
        p.estado,
        p.total
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes c
        ON p.id_cliente = c.id_cliente
    ORDER BY p.id_pedido;
END;
GO

/* ============================================================
   14. PROCEDIMIENTOS - DETALLE PEDIDO
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_DetallePedido_Insertar
    @id_pedido INT,
    @id_producto INT,
    @cantidad INT,
    @precio_unitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Detalle_Pedido
        (id_pedido, id_producto, cantidad, precio_unitario, subtotal)
        VALUES
        (@id_pedido, @id_producto, @cantidad, @precio_unitario, @cantidad * @precio_unitario);

        UPDATE dbo.Pedidos
        SET total = (
            SELECT ISNULL(SUM(subtotal), 0)
            FROM dbo.Detalle_Pedido
            WHERE id_pedido = @id_pedido
        )
        WHERE id_pedido = @id_pedido;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_DetallePedido_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DetallePedido_Eliminar
    @id_detalle INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @id_pedido INT;

        SELECT @id_pedido = id_pedido
        FROM dbo.Detalle_Pedido
        WHERE id_detalle = @id_detalle;

        DELETE FROM dbo.Detalle_Pedido
        WHERE id_detalle = @id_detalle;

        IF @@ROWCOUNT = 0
            THROW 50009, 'El detalle no existe.', 1;

        UPDATE dbo.Pedidos
        SET total = (
            SELECT ISNULL(SUM(subtotal), 0)
            FROM dbo.Detalle_Pedido
            WHERE id_pedido = @id_pedido
        )
        WHERE id_pedido = @id_pedido;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_DetallePedido_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_DetallePedido_Listar
    @id_pedido INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.id_detalle,
        d.id_pedido,
        p.nombre AS producto,
        d.cantidad,
        d.precio_unitario,
        d.subtotal
    FROM dbo.Detalle_Pedido d
    INNER JOIN dbo.Productos p
        ON d.id_producto = p.id_producto
    WHERE d.id_pedido = @id_pedido;
END;
GO

/* ============================================================
   15. PROCEDIMIENTOS - PAGOS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Pago_Insertar
    @id_pedido INT,
    @monto DECIMAL(10,2),
    @metodo_pago VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Pagos (id_pedido, monto, metodo_pago)
        VALUES (@id_pedido, @monto, @metodo_pago);

        SELECT SCOPE_IDENTITY() AS id_pago;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Pago_Insertar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pago_Eliminar
    @id_pago INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM dbo.Pagos
        WHERE id_pago = @id_pago;

        IF @@ROWCOUNT = 0
            THROW 50010, 'El pago no existe.', 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Pago_Eliminar', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pago_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Pagos
    ORDER BY id_pago;
END;
GO

/* ============================================================
   16. PROCEDIMIENTO PARA VER ERRORES
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_LogErrores_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.LogErrores
    ORDER BY id_error DESC;
END;
GO

/* ============================================================
   17. PROCEDIMIENTO DE LOGIN (sp_Usuario_Login)
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Usuario_Login
    @correo NVARCHAR(100),
    @contrasena NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT TOP 1
            id_usuario,
            nombre,
            correo,
            rol
        FROM dbo.Usuarios
        WHERE correo = @correo
          AND contrasena = @contrasena
          AND activo = 1;
    END TRY
    BEGIN CATCH
        INSERT INTO dbo.LogErrores
        (procedimiento, numero_error, severidad, estado_error, mensaje)
        VALUES
        ('sp_Usuario_Login', ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

/* ============================================================
   18. DATOS INICIALES Y DE PRUEBA (idempotentes)
   ============================================================ */

-- Cliente Mostrador con ID 1 para ventas rápidas
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE id_cliente = 1)
BEGIN
    SET IDENTITY_INSERT dbo.Clientes ON;
    INSERT INTO dbo.Clientes (id_cliente, nombre, telefono, correo)
    VALUES (1, 'Cliente Mostrador', NULL, NULL);
    SET IDENTITY_INSERT dbo.Clientes OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE correo = 'maria@gmail.com')
    INSERT INTO dbo.Clientes (nombre, telefono, correo) VALUES ('María López', '8888-1111', 'maria@gmail.com');
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE correo = 'carlos@gmail.com')
    INSERT INTO dbo.Clientes (nombre, telefono, correo) VALUES ('Carlos Pérez', '8888-2222', 'carlos@gmail.com');
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE correo = 'ana@gmail.com')
    INSERT INTO dbo.Clientes (nombre, telefono, correo) VALUES ('Ana Rodríguez', '8888-3333', 'ana@gmail.com');
GO

-- Categorías iniciales
IF NOT EXISTS (SELECT 1 FROM dbo.Categorias WHERE nombre = 'Pasteles')
    INSERT INTO dbo.Categorias (nombre, descripcion) VALUES ('Pasteles', 'Pasteles para diferentes ocasiones');
IF NOT EXISTS (SELECT 1 FROM dbo.Categorias WHERE nombre = 'Cupcakes')
    INSERT INTO dbo.Categorias (nombre, descripcion) VALUES ('Cupcakes', 'Cupcakes decorados');
IF NOT EXISTS (SELECT 1 FROM dbo.Categorias WHERE nombre = 'Postres')
    INSERT INTO dbo.Categorias (nombre, descripcion) VALUES ('Postres', 'Postres variados');
GO

-- Productos iniciales
DECLARE @idPasteles INT = (SELECT TOP 1 id_categoria FROM dbo.Categorias WHERE nombre = 'Pasteles');
DECLARE @idCupcakes INT = (SELECT TOP 1 id_categoria FROM dbo.Categorias WHERE nombre = 'Cupcakes');

IF @idPasteles IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE nombre = 'Pastel de Chocolate')
    INSERT INTO dbo.Productos (nombre, descripcion, precio, disponibilidad, id_categoria)
    VALUES ('Pastel de Chocolate', 'Pastel de chocolate con cobertura', 650.00, 1, @idPasteles);

IF @idPasteles IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE nombre = 'Pastel de Vainilla')
    INSERT INTO dbo.Productos (nombre, descripcion, precio, disponibilidad, id_categoria)
    VALUES ('Pastel de Vainilla', 'Pastel de vainilla decorado', 550.00, 1, @idPasteles);

IF @idCupcakes IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Productos WHERE nombre = 'Cupcakes de Vainilla')
    INSERT INTO dbo.Productos (nombre, descripcion, precio, disponibilidad, id_categoria)
    VALUES ('Cupcakes de Vainilla', 'Docena de cupcakes de vainilla', 300.00, 1, @idCupcakes);
GO

-- Usuario administrador de prueba
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE correo = N'admin@lacasadelsabor.com')
BEGIN
    INSERT INTO dbo.Usuarios (nombre, correo, contrasena, rol, activo)
    VALUES (N'Administrador', N'admin@lacasadelsabor.com', N'admin123', N'Administrador', 1);
END
GO

SELECT name AS BaseDeDatos FROM sys.databases WHERE name = 'LaCasaDelSabor';
SELECT id_usuario, nombre, correo, rol, activo FROM dbo.Usuarios;
SELECT id_producto, nombre, precio, disponibilidad FROM dbo.Productos;
GO

