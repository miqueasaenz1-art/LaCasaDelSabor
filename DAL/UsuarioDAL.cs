using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.DAL
{
    public class UsuarioDAL
    {
        public List<UsuarioEntity> ObtenerTodos()
        {
            var lista = new List<UsuarioEntity>();
            const string sql = @"
                SELECT id_usuario, nombre, correo, rol, activo, fecha_creacion
                FROM dbo.Usuarios
                ORDER BY id_usuario";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new UsuarioEntity
                        {
                            IdUsuario = r.GetInt32(0),
                            Nombre = r.IsDBNull(1) ? "" : r.GetString(1),
                            Correo = r.IsDBNull(2) ? "" : r.GetString(2),
                            Rol = r.IsDBNull(3) ? "" : r.GetString(3),
                            Activo = !r.IsDBNull(4) && r.GetBoolean(4),
                            FechaCreacion = r.IsDBNull(5) ? DateTime.MinValue : r.GetDateTime(5)
                        });
                    }
                }
            }

            return lista;
        }

        public UsuarioEntity ObtenerPorId(int idUsuario)
        {
            const string sql = @"
                SELECT id_usuario, nombre, correo, rol, activo, fecha_creacion
                FROM dbo.Usuarios
                WHERE id_usuario = @id";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new UsuarioEntity
                    {
                        IdUsuario = r.GetInt32(0),
                        Nombre = r.IsDBNull(1) ? "" : r.GetString(1),
                        Correo = r.IsDBNull(2) ? "" : r.GetString(2),
                        Rol = r.IsDBNull(3) ? "" : r.GetString(3),
                        Activo = !r.IsDBNull(4) && r.GetBoolean(4),
                        FechaCreacion = r.IsDBNull(5) ? DateTime.MinValue : r.GetDateTime(5)
                    };
                }
            }
        }

        public bool ExisteCorreo(string correo, int? excluirId = null)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Usuarios
                WHERE LOWER(correo) = LOWER(@correo)
                  AND (@excluir IS NULL OR id_usuario <> @excluir)";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@correo", correo.Trim());
                cmd.Parameters.AddWithValue("@excluir", (object)excluirId ?? DBNull.Value);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int ContarAdministradoresActivos(int? excluirId = null)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Usuarios
                WHERE activo = 1
                  AND LOWER(rol) = 'administrador'
                  AND (@excluir IS NULL OR id_usuario <> @excluir)";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@excluir", (object)excluirId ?? DBNull.Value);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int Insertar(UsuarioEntity u)
        {
            const string sql = @"
                INSERT INTO dbo.Usuarios (nombre, correo, contrasena, rol, activo)
                VALUES (@nombre, @correo, @contrasena, @rol, @activo);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", u.Nombre.Trim());
                cmd.Parameters.AddWithValue("@correo", u.Correo.Trim());
                cmd.Parameters.AddWithValue("@contrasena", u.Contrasena);
                cmd.Parameters.AddWithValue("@rol", u.Rol.Trim());
                cmd.Parameters.AddWithValue("@activo", u.Activo);
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Actualizar(UsuarioEntity u, bool actualizarContrasena)
        {
            string sql = actualizarContrasena
                ? @"UPDATE dbo.Usuarios
                    SET nombre = @nombre, correo = @correo, contrasena = @contrasena, rol = @rol, activo = @activo
                    WHERE id_usuario = @id"
                : @"UPDATE dbo.Usuarios
                    SET nombre = @nombre, correo = @correo, rol = @rol, activo = @activo
                    WHERE id_usuario = @id";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", u.IdUsuario);
                cmd.Parameters.AddWithValue("@nombre", u.Nombre.Trim());
                cmd.Parameters.AddWithValue("@correo", u.Correo.Trim());
                cmd.Parameters.AddWithValue("@rol", u.Rol.Trim());
                cmd.Parameters.AddWithValue("@activo", u.Activo);
                if (actualizarContrasena)
                {
                    cmd.Parameters.AddWithValue("@contrasena", u.Contrasena);
                }
                cn.Open();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new ArgumentException("El usuario ya no existe.");
                }
            }
        }

        public void Eliminar(int idUsuario)
        {
            const string sql = "DELETE FROM dbo.Usuarios WHERE id_usuario = @id";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cn.Open();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new ArgumentException("El usuario ya no existe.");
                }
            }
        }
    }
}

