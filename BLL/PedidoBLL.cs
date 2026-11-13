using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login_and_Register_System.DAL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.BLL
{
    public class PedidoBLL
    {
        private readonly PedidoDAL dal = new PedidoDAL();

        public DataTable ObtenerClientes()
        {
            return dal.ObtenerClientes();
        }

        public DataTable ObtenerProductosDisponibles()
        {
            return dal.ObtenerProductosDisponibles();
        }

        public List<PedidoEntity> ObtenerPedidos(string filtro)
        {
            return dal.ObtenerPedidos(filtro);
        }

        public PedidoEntity ObtenerPorId(int id)
        {
            return dal.ObtenerPorId(id);
        }

        public ResumenPedidosEntity ObtenerResumen()
        {
            return dal.ObtenerResumen();
        }

        public int CrearCliente(string nombre, string telefono, string correo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Ingresa el nombre del cliente.");
            return dal.InsertarCliente(nombre, telefono, correo);
        }

        public int Guardar(PedidoEntity pedido)
        {
            if (pedido.IdCliente == null)
                throw new ArgumentException("Selecciona un cliente.");
            if (string.IsNullOrWhiteSpace(pedido.Estado))
                throw new ArgumentException("Selecciona un estado.");
            if (pedido.Detalles == null || pedido.Detalles.Count == 0)
                throw new ArgumentException("Agrega al menos un producto.");

            decimal total = 0;
            foreach (var d in pedido.Detalles)
            {
                if (d.Cantidad <= 0)
                    throw new ArgumentException("La cantidad de " + d.NombreProducto + " debe ser mayor que cero.");
                total += d.Subtotal;
            }
            pedido.Total = total;

            return dal.Guardar(pedido);
        }

        public void ActualizarEstado(int idPedido, string nuevoEstado)
        {
            if (idPedido <= 0)
                throw new ArgumentException("ID de encargo no válido.");
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new ArgumentException("Selecciona un estado.");

            dal.ActualizarEstado(idPedido, nuevoEstado);
        }
    }
}