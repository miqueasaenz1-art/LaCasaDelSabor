using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.DAL
{
    public class ProductoDAL
    {
        public List<CategoriaEntity> ObtenerCategorias()
        {
            var lista = new List<CategoriaEntity>();
            const string sql = @"
                SELECT id_categoria, nombre, descripcion
                FROM dbo.Categorias
                ORDER BY nombre";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new CategoriaEntity
                        {
                            IdCategoria = r.GetInt32(0),
                            Nombre = r.IsDBNull(1) ? "" : r.GetString(1),
                            Descripcion = r.IsDBNull(2) ? "" : r.GetString(2)
                        });
                    }
                }
            }
            return lista;
        }

        public bool ExisteCategoria(string nombre)
        {
            const string sql = "SELECT COUNT(*) FROM dbo.Categorias WHERE LOWER(nombre) = LOWER(@nombre)";
            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre.Trim());
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int InsertarCategoria(string nombre, string descripcion)
        {
            const string sql = @"
                INSERT INTO dbo.Categorias (nombre, descripcion)
                VALUES (@nombre, @descripcion);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion.Trim());
                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<ProductoEntity> ObtenerTodos(out bool tieneColumnaStock)
        {
            var lista = new List<ProductoEntity>();

            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                tieneColumnaStock = ConexionBD.TieneColumnaStock(cn);

                string sql = tieneColumnaStock
                    ? @"SELECT p.id_producto, p.nombre, p.descripcion, p.precio, p.disponibilidad,
                               p.id_categoria, ISNULL(c.nombre, ''), p.stock
                        FROM dbo.Productos p
                        LEFT JOIN dbo.Categorias c ON c.id_categoria = p.id_categoria
                        ORDER BY p.nombre"
                    : @"SELECT p.id_producto, p.nombre, p.descripcion, p.precio, p.disponibilidad,
                               p.id_categoria, ISNULL(c.nombre, ''), CAST(NULL AS INT) AS stock
                        FROM dbo.Productos p
                        LEFT JOIN dbo.Categorias c ON c.id_categoria = p.id_categoria
                        ORDER BY p.nombre";

                using (var cmd = new SqlCommand(sql, cn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new ProductoEntity
                        {
                            IdProducto = r.GetInt32(0),
                            Nombre = r.IsDBNull(1) ? "" : r.GetString(1),
                            Descripcion = r.IsDBNull(2) ? "" : r.GetString(2),
                            Precio = r.GetDecimal(3),
                            Disponibilidad = !r.IsDBNull(4) && r.GetBoolean(4),
                            IdCategoria = r.GetInt32(5),
                            NombreCategoria = r.IsDBNull(6) ? "" : r.GetString(6),
                            Stock = r.IsDBNull(7) ? (int?)null : r.GetInt32(7)
                        });
                    }
                }
            }

            return lista;
        }

        public bool ExisteNombre(string nombre, int? excluirId = null)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Productos
                WHERE LOWER(nombre) = LOWER(@nombre)
                  AND (@excluir IS NULL OR id_producto <> @excluir)";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@excluir", (object)excluirId ?? DBNull.Value);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int Insertar(ProductoEntity p)
        {
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                bool tieneStock = ConexionBD.TieneColumnaStock(cn);

                string sql = tieneStock
                    ? @"INSERT INTO dbo.Productos (nombre, descripcion, precio, disponibilidad, id_categoria, stock)
                        VALUES (@nombre, @descripcion, @precio, @disponibilidad, @id_categoria, @stock);
                        SELECT CAST(SCOPE_IDENTITY() AS int);"
                    : @"INSERT INTO dbo.Productos (nombre, descripcion, precio, disponibilidad, id_categoria)
                        VALUES (@nombre, @descripcion, @precio, @disponibilidad, @id_categoria);
                        SELECT CAST(SCOPE_IDENTITY() AS int);";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", p.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrWhiteSpace(p.Descripcion) ? (object)DBNull.Value : p.Descripcion.Trim());
                    cmd.Parameters.AddWithValue("@precio", p.Precio);
                    cmd.Parameters.AddWithValue("@disponibilidad", p.Disponibilidad);
                    cmd.Parameters.AddWithValue("@id_categoria", p.IdCategoria);
                    if (tieneStock)
                    {
                        cmd.Parameters.AddWithValue("@stock", p.Stock ?? 0);
                    }
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Actualizar(ProductoEntity p)
        {
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                bool tieneStock = ConexionBD.TieneColumnaStock(cn);

                string sql = tieneStock
                    ? @"UPDATE dbo.Productos
                        SET nombre = @nombre, descripcion = @descripcion, precio = @precio,
                            disponibilidad = @disponibilidad, id_categoria = @id_categoria, stock = @stock
                        WHERE id_producto = @id"
                    : @"UPDATE dbo.Productos
                        SET nombre = @nombre, descripcion = @descripcion, precio = @precio,
                            disponibilidad = @disponibilidad, id_categoria = @id_categoria
                        WHERE id_producto = @id";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", p.IdProducto);
                    cmd.Parameters.AddWithValue("@nombre", p.Nombre.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrWhiteSpace(p.Descripcion) ? (object)DBNull.Value : p.Descripcion.Trim());
                    cmd.Parameters.AddWithValue("@precio", p.Precio);
                    cmd.Parameters.AddWithValue("@disponibilidad", p.Disponibilidad);
                    cmd.Parameters.AddWithValue("@id_categoria", p.IdCategoria);
                    if (tieneStock)
                    {
                        cmd.Parameters.AddWithValue("@stock", p.Stock ?? 0);
                    }
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new ArgumentException("El producto ya no existe.");
                    }
                }
            }
        }

        public void Eliminar(int idProducto)
        {
            const string sql = "DELETE FROM dbo.Productos WHERE id_producto = @id";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", idProducto);
                cn.Open();
                try
                {
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new ArgumentException("El producto ya no existe.");
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        throw new ArgumentException(
                            "No se puede eliminar este producto porque ya aparece en ventas o encargos.\n\nDesmarca 'Disponible para venta' y guarda para ocultarlo del menú.");
                    }
                    throw;
                }
            }
        }
    }
}

