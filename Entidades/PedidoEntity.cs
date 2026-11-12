using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_and_Register_System.Entidades
{
    public class PedidoEntity
    {
        public int IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public List<DetallePedidoEntity> Detalles { get; set; } = new List<DetallePedidoEntity>();
    }

    public class DetallePedidoEntity
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get { return Cantidad * PrecioUnitario; } }
    }

    public class ResumenPedidosEntity
    {
        public int Pendientes { get; set; }
        public int Cancelados { get; set; }
        public int Entregados { get; set; }
        public decimal VentasHoy { get; set; }
    }
}