using Login_and_Register_System.DAL;
using Login_and_Register_System.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_and_Register_System.BLL
{
    public class VentaBLL
    {
        private readonly VentaDAL dal = new VentaDAL();

        
        public static readonly string[] MetodosPago = { "Efectivo", "Tarjeta", "Transferencia" };

        public DataTable ObtenerProductosDisponibles()
        {
            return dal.ObtenerProductosDisponibles();
        }

        public List<VentaReporteEntity> ObtenerVentas()
        {
            return dal.ObtenerVentas();
        }

        public int Cobrar(VentaEntity venta)
        {
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new ArgumentException("Agrega al menos un producto.");

            if (Array.IndexOf(MetodosPago, venta.MetodoPago) < 0)
                throw new ArgumentException("Selecciona un método de pago.");

            decimal total = 0;
            foreach (var d in venta.Detalles)
            {
                if (d.Cantidad <= 0)
                    throw new ArgumentException("La cantidad de " + d.NombreProducto + " debe ser mayor que cero.");
                total += d.Subtotal;
            }

            if (total <= 0)
                throw new ArgumentException("El total de la venta debe ser mayor que cero.");
            venta.Total = total;

            if (venta.MetodoPago == "Efectivo")
            {
                if (venta.Recibido < venta.Total)
                    throw new ArgumentException("El monto recibido no alcanza para cubrir el total.");
            }
            else
            {
                // tarjeta y transferencia 
                venta.Recibido = venta.Total;
            }

            return dal.Cobrar(venta);
        }
    }
}

