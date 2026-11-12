USE LaCasaDelSabor;
GO

/* ============================================================
   TABLA USUARIOS (requerida por LoginForm)
   ============================================================ */
IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        id_usuario INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        correo NVARCHAR(100) NOT NULL UNIQUE,
        contrasena NVARCHAR(100) NOT NULL,
        rol NVARCHAR(50) NOT NULL DEFAULT 'Cajero',
        activo BIT NOT NULL DEFAULT 1,
        fecha_creacion DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF COL_LENGTH('dbo.Usuarios', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios
    ADD activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1;
END
GO

/* ============================================================
   PROCEDIMIENTO DE LOGIN: sp_Usuario_Login
   Coincide con LoginForm.cs (@correo, @contrasena -> nombre, rol)
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
        IF OBJECT_ID('dbo.LogErrores', 'U') IS NOT NULL
        BEGIN
            INSERT INTO dbo.LogErrores
            (procedimiento, numero_error, severidad, estado_error, mensaje)
            VALUES
            ('sp_Usuario_Login',
             ERROR_NUMBER(),
             ERROR_SEVERITY(),
             ERROR_STATE(),
             ERROR_MESSAGE());
        END;

        THROW;
    END CATCH
END;
GO

/* ============================================================
   USUARIO DE PRUEBA
   ============================================================ */
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE correo = N'admin@lacasadelsabor.com')
BEGIN
    INSERT INTO dbo.Usuarios (nombre, correo, contrasena, rol, activo)
    VALUES (N'Administrador', N'admin@lacasadelsabor.com', N'admin123', N'Administrador', 1);
END
GO

SELECT id_usuario, nombre, correo, rol, activo
FROM dbo.Usuarios
WHERE correo = N'admin@lacasadelsabor.com';
GO

