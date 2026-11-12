using Login_and_Register_System.BLL;
using Login_and_Register_System.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;


namespace Login_and_Register_System
{
    public partial class VentasForm : Form
    {
        private readonly VentaBLL bll = new VentaBLL();
        private decimal totalActual = 0;
        private int idVentaActual = 0;
        private List<VentaReporteEntity> ventasReporte;
        private int idxReporte = 0;
        public VentasForm()
        {
            InitializeComponent();
            Load += VentasForm_Load;
            buttonAgregar.Click += buttonAgregar_Click;
            buttonQuitar.Click += buttonQuitar_Click;
            textBoxRecibido.TextChanged += (s, ev) => CalcularCambio();
            comboBoxMetodo.SelectedIndexChanged += comboBoxMetodo_SelectedIndexChanged;
            buttonCobrar.Click += buttonCobrar_Click;
            buttonNuevaVenta.Click += buttonNuevaVenta_Click;
            buttonReporte.Click += buttonReporte_Click;
            buttonVistaPrevia.Click += buttonImprimir_Click;
            btnExacto.Click += (s, ev) =>
            {
                if (comboBoxMetodo.Text == "Efectivo")
                    textBoxRecibido.Text = totalActual.ToString("0.00", CultureInfo.InvariantCulture);
            };
            Tema.Aplicar(this);
            AplicarEstilos();
        }

        private void AplicarEstilos()
        {
            Tema.PintarTarjeta(panelProductos, Tema.Blanco, Tema.Borde, 14);
            Tema.PintarTarjeta(panel, Tema.Blanco, Tema.Borde, 14);
            Tema.PintarTarjeta(panelPago, Tema.RosaSuave, Tema.Borde, 12);
            Tema.PintarTarjeta(panelTotal, Tema.Vino, Tema.VinoOscuro, 12);
            Tema.DecorarTarjetaTotalPasteleria(panelTotal, 12);
            Tema.DecorarTituloSeccion(panelHeader, labelTitulo, Tema.MotivoPasteleria.Cupcake);

            Tema.EstilizarBotonPrimario(buttonNuevaVenta, Tema.Vino, Tema.VinoHover, 10);
            Tema.EstilizarBotonPrimario(buttonAgregar, Tema.Vino, Tema.VinoHover, 8);
            Tema.EstilizarBotonPrimario(buttonCobrar, Tema.Exito, Tema.ExitoHover, 10);
            Tema.EstilizarBotonSecundario(buttonReporte, 10);
            Tema.EstilizarBotonSecundario(buttonVistaPrevia, 8);
            Tema.EstilizarBotonSecundario(btnExacto, 8);
            Tema.EstilizarBotonPeligro(buttonQuitar, 8);

            colDCantidad.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDPrecio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDPrecio.DefaultCellStyle.Format = "C$ #,##0.00";
            ColumnSubtotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ColumnSubtotal.DefaultCellStyle.Format = "C$ #,##0.00";
        }

        private void VentasForm_Load(object sender, EventArgs e)
        {
            labelFecha.Text = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy",
                              CultureInfo.GetCultureInfo("es-NI"));

            comboBoxMetodo.Items.AddRange(VentaBLL.MetodosPago);
            comboBoxMetodo.SelectedIndex = 0;

            CargarProductosDisponibles();
        }

        private void CargarProductosDisponibles()
        {
            try
            {
                comboBoxProducto.DataSource = bll.ObtenerProductosDisponibles();
                comboBoxProducto.DisplayMember = "nombre";
                comboBoxProducto.ValueMember = "id_producto";
            }
            catch
            {
                comboBoxProducto.DataSource = null;
            }
        }


        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            if (comboBoxProducto.SelectedItem == null) return;

            var fila = (DataRowView)comboBoxProducto.SelectedItem;
            int idProd = Convert.ToInt32(fila["id_producto"]);
            string nombre = fila["nombre"].ToString();
            decimal precio = Convert.ToDecimal(fila["precio"]);
            int? stock = (fila.Row.Table.Columns.Contains("stock") && fila["stock"] != DBNull.Value)
                ? Convert.ToInt32(fila["stock"])
                : (int?)null;
            int cant = (int)numericUpDownCantidad.Value;

            
            DataGridViewRow existente = null;
            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (!f.IsNewRow && f.Tag != null && (int)f.Tag == idProd) { existente = f; break; }
            }

            int yaEnTabla = existente == null ? 0 : Convert.ToInt32(existente.Cells[1].Value);
            if (stock.HasValue && yaEnTabla + cant > stock.Value)
            {
                MessageBox.Show("Solo hay " + stock.Value + " disponible(s) de " + nombre +
                                (yaEnTabla > 0 ? " (ya agregaste " + yaEnTabla + ")." : "."));
                return;
            }

            if (existente != null)
            {
                int nueva = yaEnTabla + cant;
                existente.Cells[1].Value = nueva;
                existente.Cells[3].Value = nueva * precio;
            }
            else
            {
                int i = dataGridViewDetalle.Rows.Add(nombre, cant, precio, cant * precio);
                dataGridViewDetalle.Rows[i].Tag = idProd;
            }

            numericUpDownCantidad.Value = 1;
            CalcularTotal();
        }

        private void buttonQuitar_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetalle.CurrentRow == null || dataGridViewDetalle.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Selecciona un producto de la tabla.");
                return;
            }
            dataGridViewDetalle.Rows.Remove(dataGridViewDetalle.CurrentRow);
            CalcularTotal();
        }

        
        private void CalcularTotal()
        {
            totalActual = 0;
            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (!f.IsNewRow && f.Cells[3].Value != null)
                    totalActual += Convert.ToDecimal(f.Cells[3].Value);
            }
            labelTotal.Text = "C$ " + totalActual.ToString("N2");

            
            if (comboBoxMetodo.Text != "Efectivo" && comboBoxMetodo.Text != "")
                textBoxRecibido.Text = totalActual.ToString("0.00", CultureInfo.InvariantCulture);

            CalcularCambio();
        }

        private decimal LeerMonto(string texto)
        {
            string limpio = texto.Replace("C$", "").Replace(",", "").Trim();
            decimal valor;
            return decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out valor) ? valor : 0;
        }

        private void CalcularCambio()
        {
            decimal cambio = LeerMonto(textBoxRecibido.Text) - totalActual;
            textBox2Cambio.Text = (comboBoxMetodo.Text == "Efectivo" && cambio > 0)
                                 ? cambio.ToString("N2") : "0.00";
        }

        private void comboBoxMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool efectivo = comboBoxMetodo.Text == "Efectivo";
            textBoxRecibido.Enabled = efectivo;
            btnExacto.Enabled = efectivo;
            if (!efectivo) textBoxRecibido.Text = totalActual.ToString("0.00", CultureInfo.InvariantCulture);
            CalcularCambio();
        }
        private void buttonCobrar_Click(object sender, EventArgs e)
        {
            var venta = new VentaEntity
            {
                MetodoPago = comboBoxMetodo.Text,
                Recibido = LeerMonto(textBoxRecibido.Text)
            };

            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow || f.Tag == null) continue;
                venta.Detalles.Add(new DetallePedidoEntity
                {
                    IdProducto = (int)f.Tag,
                    NombreProducto = f.Cells[0].Value.ToString(),
                    Cantidad = Convert.ToInt32(f.Cells[1].Value),
                    PrecioUnitario = Convert.ToDecimal(f.Cells[2].Value)
                });
            }

            try
            {
                int id = bll.Cobrar(venta);
                MessageBox.Show("Venta N.º " + id + " cobrada.\nCambio: C$ " + venta.Cambio.ToString("N2"));

                idVentaActual = id;
                if (MessageBox.Show("¿Deseas imprimir el recibo?", "Imprimir",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    MostrarVistaPrevia();
                idVentaActual = 0;

                NuevaVenta();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cobrar:\n" + ex.Message);
            }
        }

        private void buttonNuevaVenta_Click(object sender, EventArgs e)
        {
            NuevaVenta();
        }

        private void NuevaVenta()
        {
            dataGridViewDetalle.Rows.Clear();
            numericUpDownCantidad.Value = 1;
            if (comboBoxMetodo.Items.Count > 0) comboBoxMetodo.SelectedIndex = 0;
            textBoxRecibido.Text = "";

            CargarProductosDisponibles();

            CalcularTotal();
        }

        private void buttonReporte_Click(object sender, EventArgs e)
        {
    

            try
            {
                ventasReporte = bll.ObtenerVentas();

                if (ventasReporte.Count == 0)
                {
                    MessageBox.Show("Todavía no hay ventas registradas.");
                    return;
                }

                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "Archivo PDF (*.pdf)|*.pdf";
                    dlg.FileName = "Ventas_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    using (PrintDocument doc = new PrintDocument())
                    {
                        doc.PrinterSettings.PrinterName =
                            "Microsoft Print to PDF";

                        doc.PrintController =
                            new StandardPrintController();

                        if (!doc.PrinterSettings.IsValid)
                        {
                            MessageBox.Show(
                                "Microsoft Print to PDF no está disponible.",
                                "Impresora no disponible",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return;
                        }

                        doc.BeginPrint += (s, ev) => idxReporte = 0;
                        doc.PrintPage += ImprimirReporte;

                        doc.PrinterSettings.PrintToFile = true;
                        doc.PrinterSettings.PrintFileName = dlg.FileName;

                        
                        

                        doc.Print();

                        if (File.Exists(dlg.FileName))
                        {
                            MessageBox.Show(
                                "PDF creado correctamente en:\n" + dlg.FileName,
                                "Reporte de ventas",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        else
                        {
                            MessageBox.Show(
                                "El archivo PDF no se encontró.\n" +
                                "Ruta: " + dlg.FileName,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al generar el reporte:\n" + ex.Message
                );
            }
        }

        private void ImprimirReporte(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var titulo = new Font("Segoe UI", 16, FontStyle.Bold);
            var cab = new Font("Segoe UI", 10, FontStyle.Bold);
            var normal = new Font("Segoe UI", 10);
            var derecha = new StringFormat { Alignment = StringAlignment.Far };
            float y = 40;
            float limite = e.MarginBounds.Bottom - 20;

            if (idxReporte == 0)
            {
                g.DrawString("La Casa del Sabor - Reporte de ventas", titulo, Brushes.Black, 40, y); y += 35;
                g.DrawString("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), normal, Brushes.Black, 40, y); y += 35;
            }

            g.DrawString("N.º", cab, Brushes.Black, 40, y);
            g.DrawString("Fecha", cab, Brushes.Black, 95, y);
            g.DrawString("Cliente", cab, Brushes.Black, 270, y);
            g.DrawString("Pago", cab, Brushes.Black, 470, y);
            g.DrawString("Monto", cab, Brushes.Black, 740, y, derecha);
            y += 22;
            g.DrawLine(Pens.Black, 40, y, 740, y);
            y += 8;

            while (idxReporte < ventasReporte.Count)
            {
                if (y > limite)
                {
                    e.HasMorePages = true;
                    return;
                }
                var v = ventasReporte[idxReporte];
                g.DrawString(v.IdPedido.ToString(), normal, Brushes.Black, 40, y);
                g.DrawString(v.Fecha.ToString("dd/MM/yyyy HH:mm"), normal, Brushes.Black, 95, y);
                g.DrawString(v.Cliente, normal, Brushes.Black, 270, y);
                g.DrawString(v.MetodoPago, normal, Brushes.Black, 470, y);
                g.DrawString("C$ " + v.Monto.ToString("N2"), normal, Brushes.Black, 740, y, derecha);
                y += 22;
                idxReporte++;
            }

            // totales al final
            if (y > limite - 110)
            {
                e.HasMorePages = true;
                return;
            }

            decimal total = 0;
            var porMetodo = new Dictionary<string, decimal>();
            foreach (var v in ventasReporte)
            {
                total += v.Monto;
                if (!porMetodo.ContainsKey(v.MetodoPago)) porMetodo[v.MetodoPago] = 0;
                porMetodo[v.MetodoPago] += v.Monto;
            }

            y += 10;
            g.DrawLine(Pens.Black, 40, y, 740, y);
            y += 10;
            foreach (var par in porMetodo)
            {
                g.DrawString(par.Key + ":", normal, Brushes.Black, 470, y);
                g.DrawString("C$ " + par.Value.ToString("N2"), normal, Brushes.Black, 740, y, derecha);
                y += 22;
            }
            y += 5;
            g.DrawString("Total general:", cab, Brushes.Black, 470, y);
            g.DrawString("C$ " + total.ToString("N2"), cab, Brushes.Black, 740, y, derecha);
            g.DrawString(ventasReporte.Count + " venta(s)", normal, Brushes.Black, 40, y);

            e.HasMorePages = false;
        }

        private void buttonImprimir_Click(object sender, EventArgs e)
        {
            MostrarVistaPrevia();
        }


        private const int AnchoRecibo = 42;

        private string Centrar(string texto)
        {
            int espacios = Math.Max(0, (AnchoRecibo - texto.Length) / 2);
            return new string(' ', espacios) + texto;
        }

        private string Par(string etiqueta, string valor)
        {
            int relleno = Math.Max(1, AnchoRecibo - etiqueta.Length - valor.Length);
            return etiqueta + new string(' ', relleno) + valor;
        }

        private string FilaProducto(string producto, string cantidad, string total)
        {
            if (producto.Length > 22) producto = producto.Substring(0, 22);
            return producto.PadRight(22) + cantidad.PadLeft(6) + total.PadLeft(14);
        }




        private void MostrarVistaPrevia()
        {
            if (dataGridViewDetalle.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos para mostrar.");
                return;
            }

            string linea = new string('-', AnchoRecibo);
            var recibo = new StringBuilder();

            recibo.AppendLine(Centrar("LA CASA DEL SABOR"));
            recibo.AppendLine(Centrar("Pastelería"));
            recibo.AppendLine(linea);
            recibo.AppendLine(idVentaActual > 0 ? "Recibo de venta N.º " + idVentaActual : "Detalle de la venta");
            recibo.AppendLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            recibo.AppendLine(linea);
            recibo.AppendLine(FilaProducto("PRODUCTO", "CANT.", "TOTAL"));
            recibo.AppendLine(linea);

            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow || f.Cells[0].Value == null) continue;

                string producto = f.Cells[0].Value.ToString();
                string cantidad = f.Cells[1].Value.ToString();
                string total = Convert.ToDecimal(f.Cells[3].Value).ToString("N2");
                recibo.AppendLine(FilaProducto(producto, cantidad, "C$ " + total));
            }

            recibo.AppendLine(linea);
            recibo.AppendLine(Par("TOTAL:", labelTotal.Text));
            recibo.AppendLine(Par("Método de pago:", comboBoxMetodo.Text));
            recibo.AppendLine(Par("Recibido:", "C$ " + LeerMonto(textBoxRecibido.Text).ToString("N2")));
            recibo.AppendLine(Par("Cambio:", "C$ " + textBox2Cambio.Text));
            recibo.AppendLine(linea);
            recibo.AppendLine(Centrar("¡GRACIAS POR SU COMPRA!"));

            var contenido = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.None,
                TabStop = false
            };
            contenido.Text = recibo.ToString();

            
            contenido.Select(0, contenido.Lines[0].Length);
            contenido.SelectionFont = new Font("Consolas", 10, FontStyle.Bold);
            contenido.Select(0, 0);

           
            var margen = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 15, 20, 15), BackColor = Color.White };
            margen.Controls.Add(contenido);

           
            var panelAcciones = new Panel { Dock = DockStyle.Bottom, Height = 52, Padding = new Padding(20, 8, 20, 10), BackColor = Color.White };
            var btnImprimirReal = new Button { Dock = DockStyle.Right, Width = 120, Text = "Imprimir" };
            Tema.EstilizarBotonPrimario(btnImprimirReal, Tema.Vino, Tema.VinoHover, 8);
            btnImprimirReal.Click += (s, ev) =>
            {
                using (var doc = new PrintDocument())
                using (var dlgPrint = new PrintDialog { Document = doc })
                {
                    doc.PrintPage += ImprimirPagina;
                    if (dlgPrint.ShowDialog() == DialogResult.OK)
                    {
                        doc.Print();
                    }
                }
            };
            panelAcciones.Controls.Add(btnImprimirReal);

            int ancho = TextRenderer.MeasureText(new string('M', AnchoRecibo), contenido.Font,
                            new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width + 60;
            int alto = contenido.Font.Height * contenido.Lines.Length + 98;

            var ventana = new Form
            {
                Text = "Recibo de venta",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                ClientSize = new Size(ancho, alto)
            };
            ventana.Controls.Add(margen);
            ventana.Controls.Add(panelAcciones);
            ventana.ShowDialog(this);
        }



        private void ImprimirPagina(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var titulo = new Font("Segoe UI", 16, FontStyle.Bold);
            var normal = new Font("Segoe UI", 11);
            float y = 40;

            g.DrawString("La Casa del Sabor", titulo, Brushes.Black, 40, y); y += 40;
            string cabecera = idVentaActual > 0 ? "Recibo de venta N.º " + idVentaActual : "Detalle de la venta";
            g.DrawString(cabecera, normal, Brushes.Black, 40, y); y += 25;
            g.DrawString("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), normal, Brushes.Black, 40, y); y += 35;

            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow || f.Cells[0].Value == null) continue;
                g.DrawString(f.Cells[1].Value + " x " + f.Cells[0].Value, normal, Brushes.Black, 40, y);
                g.DrawString("C$ " + Convert.ToDecimal(f.Cells[3].Value).ToString("N2"), normal, Brushes.Black, 500, y);
                y += 25;
            }

            y += 15;
            g.DrawString("Total: " + labelTotal.Text, titulo, Brushes.Black, 40, y); y += 40;
            g.DrawString("Método de pago: " + comboBoxMetodo.Text, normal, Brushes.Black, 40, y); y += 25;
            g.DrawString("Recibido: C$ " + LeerMonto(textBoxRecibido.Text).ToString("N2"), normal, Brushes.Black, 40, y); y += 25;
            g.DrawString("Cambio: C$ " + textBox2Cambio.Text, normal, Brushes.Black, 40, y);
        }

        private void buttonAgregar_Click_1(object sender, EventArgs e)
        {

        }
    }
}