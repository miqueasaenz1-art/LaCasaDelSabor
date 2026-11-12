using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.DAL
{
    public class PedidoDAL
    {
        public DataTable ObtenerClientes()
        {
            using (var cn = ConexionBD.CrearConexion())
            using (var da = new SqlDataAdapter(
                "SELECT id_cliente, nombre FROM Clientes ORDER BY nombre", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ObtenerProductosDisponibles()
        {
            using (var cn = ConexionBD.CrearConexion())
            using (var da = new SqlDataAdapter(
                @"SELECT id_producto, nombre, precio
          FROM Productos
          WHERE disponibilidad = 1
          ORDER BY nombre", cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public List<PedidoEntity> ObtenerPedidos(string filtro)
        {
            string where = "";
            if (filtro == "Hoy")
                where = "WHERE CAST(p.fecha_pedido AS date) = CAST(GETDATE() AS date) ";
            else if (filtro == "Pendiente")
                where = "WHERE p.estado = 'Pendiente' ";

            string sql = @"SELECT p.id_pedido, c.nombre, p.fecha_entrega, p.estado, p.total
                           FROM Pedidos p
                           LEFT JOIN Clientes c ON c.id_cliente = p.id_cliente "
                         + where + "ORDER BY p.id_pedido DESC";

            var lista = new List<PedidoEntity>();
            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new PedidoEntity
                        {
                            IdPedido = r.GetInt32(0),
                            NombreCliente = r.IsDBNull(1) ? "" : r.GetString(1),
                            FechaEntrega = r.IsDBNull(2) ? (DateTime?)null : r.GetDateTime(2),
                            Estado = r.GetString(3),
                            Total = r.GetDecimal(4)
                        });
                    }
                }
            }
            return lista;
        }

        public PedidoEntity ObtenerPorId(int id)
        {
            PedidoEntity pedido = null;
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();

                using (var cmd = new SqlCommand(
                    "SELECT id_cliente, fecha_entrega, estado, total FROM Pedidos WHERE id_pedido = @id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return null;
                        pedido = new PedidoEntity
                        {
                            IdPedido = id,
                            IdCliente = r.IsDBNull(0) ? (int?)null : r.GetInt32(0),
                            FechaEntrega = r.IsDBNull(1) ? (DateTime?)null : r.GetDateTime(1),
                            Estado = r.GetString(2),
                            Total = r.GetDecimal(3)
                        };
                    }
                }

                using (var det = new SqlCommand(
                    @"SELECT d.id_producto, p.nombre, d.cantidad, d.precio_unitario
                      FROM Detalle_Pedido d
                      JOIN Productos p ON p.id_producto = d.id_producto
                      WHERE d.id_pedido = @id", cn))
                {
                    det.Parameters.AddWithValue("@id", id);
                    using (var r = det.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            pedido.Detalles.Add(new DetallePedidoEntity
                            {
                                IdProducto = r.GetInt32(0),
                                NombreProducto = r.GetString(1),
                                Cantidad = r.GetInt32(2),
                                PrecioUnitario = r.GetDecimal(3)
                            });
                        }
                    }
                }
            }
            return pedido;
        }

        public int InsertarCliente(string nombre, string telefono, string correo)
        {
            const string sql = @"
                INSERT INTO dbo.Clientes (nombre, telefono, correo)
                VALUES (@nombre, @telefono, @correo);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre.Trim());
                cmd.Parameters.AddWithValue("@telefono", string.IsNullOrWhiteSpace(telefono) ? (object)DBNull.Value : telefono.Trim());
                cmd.Parameters.AddWithValue("@correo", string.IsNullOrWhiteSpace(correo) ? (object)DBNull.Value : correo.Trim());
                cn.Open();
                try
                {
                    return (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                        throw new ArgumentException("Ya existe un cliente registrado con ese correo.");
                    throw;
                }
            }
        }

        public int Guardar(PedidoEntity p)
        {
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        int idPedido = p.IdPedido;

                        if (idPedido > 0)
                        {
                            using (var upd = new SqlCommand(
                                @"UPDATE Pedidos
                                  SET id_cliente = @cliente, fecha_entrega = @entrega, estado = @estado, total = 0
                                  WHERE id_pedido = @id;", cn, tx))
                            {
                                upd.Parameters.AddWithValue("@id", idPedido);
                                upd.Parameters.AddWithValue("@cliente", (object)p.IdCliente ?? DBNull.Value);
                                upd.Parameters.AddWithValue("@entrega", (object)p.FechaEntrega ?? DBNull.Value);
                                upd.Parameters.AddWithValue("@estado", p.Estado);
                                if (upd.ExecuteNonQuery() == 0)
                                    throw new ArgumentException("El encargo ya no existe.");
                            }

                            using (var del = new SqlCommand(
                                "DELETE FROM Detalle_Pedido WHERE id_pedido = @id;", cn, tx))
                            {
                                del.Parameters.AddWithValue("@id", idPedido);
                                del.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (var cmd = new SqlCommand(
                                @"INSERT INTO Pedidos (id_cliente, fecha_pedido, fecha_entrega, estado, total)
                                  VALUES (@cliente, GETDATE(), @entrega, @estado, 0);
                                  SELECT CAST(SCOPE_IDENTITY() AS int);", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@cliente", (object)p.IdCliente ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@entrega", (object)p.FechaEntrega ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@estado", p.Estado);
                                idPedido = (int)cmd.ExecuteScalar();
                            }
                        }

                        foreach (var d in p.Detalles)
                        {
                            using (var det = new SqlCommand("sp_DetallePedido_Insertar", cn, tx))
                            {
                                det.CommandType = CommandType.StoredProcedure;
                                det.Parameters.AddWithValue("@id_pedido", idPedido);
                                det.Parameters.AddWithValue("@id_producto", d.IdProducto);
                                det.Parameters.AddWithValue("@cantidad", d.Cantidad);
                                det.Parameters.AddWithValue("@precio_unitario", d.PrecioUnitario);
                                det.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                        return idPedido;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public ResumenPedidosEntity ObtenerResumen()
        {
            string sql = @"SELECT
                ISNULL(SUM(CASE WHEN estado = 'Pendiente'  THEN 1 ELSE 0 END), 0),
               ISNULL(SUM(CASE WHEN estado = 'Cancelado' THEN 1 ELSE 0 END), 0),
                ISNULL(SUM(CASE WHEN estado = 'Entregado'  THEN 1 ELSE 0 END), 0),
                ISNULL(SUM(CASE WHEN CAST(fecha_pedido AS date) = CAST(GETDATE() AS date)
                                AND estado <> 'Cancelado' THEN total ELSE 0 END), 0)
                FROM Pedidos";

            var resumen = new ResumenPedidosEntity();
            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        resumen.Pendientes = Convert.ToInt32(r[0]);
                        resumen.Cancelados = Convert.ToInt32(r[1]);
                        resumen.Entregados = Convert.ToInt32(r[2]);
                        resumen.VentasHoy = Convert.ToDecimal(r[3]);
                    }
                }
            }
            return resumen;
        }
    }
}