using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Login_and_Register_System.Entidades
{
    public class VentaEntity
    {
        public int IdPedido { get; set; }
        public string MetodoPago { get; set; }
        public decimal Recibido { get; set; }
        public decimal Total { get; set; }
        public decimal Cambio { get { return Recibido - Total; } }
        public List<DetallePedidoEntity> Detalles { get; set; } = new List<DetallePedidoEntity>();
    }
}

public class VentaReporteEntity
{
    public int IdPedido { get; set; }
    public DateTime Fecha { get; set; }
    public string Cliente { get; set; }
    public string MetodoPago { get; set; }
    public decimal Monto { get; set; }
}