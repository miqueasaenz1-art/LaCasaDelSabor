using Login_and_Register_System.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_and_Register_System.DAL
{
    public class VentaDAL
    {
        // Solo productos disponibles (y con existencia si existe la columna stock)
        public DataTable ObtenerProductosDisponibles()
        {
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                bool tieneStock = ConexionBD.TieneColumnaStock(cn);
                string sql = tieneStock
                    ? @"SELECT id_producto, nombre, precio, stock
                        FROM Productos
                        WHERE disponibilidad = 1 AND stock > 0
                        ORDER BY nombre"
                    : @"SELECT id_producto, nombre, precio, CAST(NULL AS INT) AS stock
                        FROM Productos
                        WHERE disponibilidad = 1
                        ORDER BY nombre";

                using (var da = new SqlDataAdapter(sql, cn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public List<VentaReporteEntity> ObtenerVentas()
        {
            string sql = @"SELECT pa.id_pedido, pa.fecha_pago, c.nombre, pa.metodo_pago, pa.monto
                   FROM Pagos pa
                   JOIN Pedidos p ON p.id_pedido = pa.id_pedido
                   LEFT JOIN Clientes c ON c.id_cliente = p.id_cliente
                   ORDER BY pa.fecha_pago, pa.id_pago";

            var lista = new List<VentaReporteEntity>();
            using (var cn = ConexionBD.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new VentaReporteEntity
                        {
                            IdPedido = r.GetInt32(0),
                            Fecha = r.GetDateTime(1),
                            Cliente = r.IsDBNull(2) ? "" : r.GetString(2),
                            MetodoPago = r.GetString(3),
                            Monto = r.GetDecimal(4)
                        });
                    }
                }
            }
            return lista;
        }

        public int Cobrar(VentaEntity v)
        {
            using (var cn = ConexionBD.CrearConexion())
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        bool tieneStock = ConexionBD.TieneColumnaStock(cn, tx);

                        int idPedido;
                        using (var cmd = new SqlCommand(
                            @"DECLARE @idCliente INT = (
                                  SELECT TOP 1 id_cliente
                                  FROM Clientes
                                  ORDER BY CASE WHEN id_cliente = 1 THEN 0 WHEN nombre = 'Cliente Mostrador' THEN 1 ELSE 2 END, id_cliente
                              );
                              DECLARE @ahora DATETIME = GETDATE();
                              INSERT INTO Pedidos (id_cliente, fecha_pedido, fecha_entrega, estado, total)
                              VALUES (@idCliente, @ahora, @ahora, 'Entregado', 0);
                              SELECT CAST(SCOPE_IDENTITY() AS int);", cn, tx))
                        {
                            idPedido = (int)cmd.ExecuteScalar();
                        }

                        foreach (var d in v.Detalles)
                        {
                            if (tieneStock)
                            {
                                using (var stock = new SqlCommand(
                                    @"UPDATE Productos SET stock = stock - @cant
                                      WHERE id_producto = @id AND stock >= @cant", cn, tx))
                                {
                                    stock.Parameters.AddWithValue("@cant", d.Cantidad);
                                    stock.Parameters.AddWithValue("@id", d.IdProducto);
                                    if (stock.ExecuteNonQuery() == 0)
                                        throw new ArgumentException("No hay suficiente existencia de " + d.NombreProducto + ".");
                                }
                            }
                            else
                            {
                                using (var chk = new SqlCommand(
                                    "SELECT COUNT(*) FROM Productos WHERE id_producto = @id AND disponibilidad = 1", cn, tx))
                                {
                                    chk.Parameters.AddWithValue("@id", d.IdProducto);
                                    if (Convert.ToInt32(chk.ExecuteScalar()) == 0)
                                        throw new ArgumentException("El producto " + d.NombreProducto + " ya no está disponible.");
                                }
                            }

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

                        using (var pago = new SqlCommand("sp_Pago_Insertar", cn, tx))
                        {
                            pago.CommandType = CommandType.StoredProcedure;
                            pago.Parameters.AddWithValue("@id_pedido", idPedido);
                            pago.Parameters.AddWithValue("@monto", v.Total);
                            pago.Parameters.AddWithValue("@metodo_pago", v.MetodoPago);
                            pago.ExecuteNonQuery();
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
    }
}