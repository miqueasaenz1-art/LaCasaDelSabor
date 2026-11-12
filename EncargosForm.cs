using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using Login_and_Register_System.BLL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System
{
    public partial class Formpedidos : Form
    {
        private readonly PedidoBLL bll = new PedidoBLL();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private int idPedidoActual = 0;
        private string filtroActual = "Todos";
        public Formpedidos()
        {
            InitializeComponent();

            Tema.Aplicar(this);
            AplicarEstilos();

            dataGridViewPedidos.CellClick += dataGridViewPedidos_CellClick;
            comboBoxEstado.SelectedIndexChanged += (s, ev) => ActualizarBadgeEstado(comboBoxEstado.Text);
        }

        private void AplicarEstilos()
        {
            Tema.PintarTarjeta(panelBuscar, Tema.Blanco, Tema.Borde, 10);
            Tema.PintarTarjeta(panelLista, Tema.Blanco, Tema.Borde, 14);
            Tema.PintarTarjeta(panelDetalle, Tema.Blanco, Tema.Borde, 14);
            Tema.PintarTarjeta(panelDatos, Tema.RosaSuave, Tema.Borde, 12);
            Tema.PintarTarjeta(panelTotal, Tema.Vino, Tema.VinoOscuro, 12);
            Tema.DecorarTarjetaTotalPasteleria(panelTotal, 12);
            Tema.DecorarTituloSeccion(panelHeader, labelTitulo, Tema.MotivoPasteleria.PastelPisos);

            Tema.EstilizarBotonPrimario(buttonNuevo, Tema.Vino, Tema.VinoHover, 10);
            Tema.EstilizarBotonPrimario(buttonAgregar, Tema.Vino, Tema.VinoHover, 8);
            Tema.EstilizarBotonPrimario(buttonGuardar, Tema.Exito, Tema.ExitoHover, 10);
            Tema.EstilizarBotonSecundario(buttonImprimir, 10);
            Tema.EstilizarBotonPeligro(buttonEliminarPedido, 10);

            colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEntrega.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEstado.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            colDCantidad.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDPrecio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDPrecio.DefaultCellStyle.Format = "C$ #,##0.00";
            ColumnSubtotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ColumnSubtotal.DefaultCellStyle.Format = "C$ #,##0.00";

            comboBox1Cliente.Width = Math.Max(140, comboBox1Cliente.Width - 40);
            var btnNuevoCliente = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(comboBox1Cliente.Right + 6, comboBox1Cliente.Top - 1),
                Size = new Size(34, 27),
                Text = "+"
            };
            Tema.EstilizarBotonSecundario(btnNuevoCliente, 6);
            btnNuevoCliente.Click += BtnNuevoCliente_Click;
            panelDatos.Controls.Add(btnNuevoCliente);

            ActualizarBotonesFiltro();
            ActualizarBadgeEstado("Pendiente");
        }

        private void BtnNuevoCliente_Click(object sender, EventArgs e)
        {
            using (var dlg = new Form
            {
                Text = "Nuevo cliente",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(340, 250),
                BackColor = Tema.Crema
            })
            {
                var lblN = new Label { Location = new Point(20, 16), AutoSize = true, Text = "Nombre completo", Font = Tema.Fuente(9F, FontStyle.Bold), ForeColor = Tema.Oscuro };
                var tbN = new TextBox { Location = new Point(20, 38), Size = new Size(300, 26), Font = Tema.Fuente(9.5F) };
                var lblT = new Label { Location = new Point(20, 74), AutoSize = true, Text = "Teléfono (opcional)", Font = Tema.Fuente(9F, FontStyle.Bold), ForeColor = Tema.Oscuro };
                var tbT = new TextBox { Location = new Point(20, 96), Size = new Size(300, 26), Font = Tema.Fuente(9.5F) };
                var lblC = new Label { Location = new Point(20, 132), AutoSize = true, Text = "Correo (opcional)", Font = Tema.Fuente(9F, FontStyle.Bold), ForeColor = Tema.Oscuro };
                var tbC = new TextBox { Location = new Point(20, 154), Size = new Size(300, 26), Font = Tema.Fuente(9.5F) };
                var btnOk = new Button { Location = new Point(210, 196), Size = new Size(110, 34), Text = "Guardar" };
                var btnCan = new Button { Location = new Point(94, 196), Size = new Size(106, 34), Text = "Cancelar", DialogResult = DialogResult.Cancel };
                Tema.EstilizarBotonPrimario(btnOk, Tema.Vino, Tema.VinoHover, 8);
                Tema.EstilizarBotonSecundario(btnCan, 8);

                btnOk.Click += (s, ev) =>
                {
                    try
                    {
                        int idCli = bll.CrearCliente(tbN.Text, tbT.Text, tbC.Text);
                        CargarClientes(idCli);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                dlg.Controls.AddRange(new Control[] { lblN, tbN, lblT, tbT, lblC, tbC, btnCan, btnOk });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCan;
                dlg.ShowDialog(this);
            }
        }

        private void ActualizarBotonesFiltro()
        {
            Tema.EstilizarFiltro(buttonTodos, filtroActual == "Todos" || filtroActual == "");
            Tema.EstilizarFiltro(buttonHoy, filtroActual == "Hoy");
            Tema.EstilizarFiltro(buttonPendiente, filtroActual == "Pendiente");
        }

        private void ActualizarBadgeEstado(string estado)
        {
            string texto = string.IsNullOrWhiteSpace(estado) ? "Pendiente" : estado;
            labelEstadoBadge.Text = texto;

            switch (texto)
            {
                case "Preparando":
                case "En preparación":
                    Tema.EstilizarBadge(labelEstadoBadge, Tema.RosaSuave, Tema.Vino, 12);
                    break;
                case "Listo":
                case "Entregado":
                    Tema.EstilizarBadge(labelEstadoBadge, Tema.ExitoSuave, Tema.ExitoTexto, 12);
                    break;
                case "Cancelado":
                    Tema.EstilizarBadge(labelEstadoBadge, Tema.PeligroSuave, Tema.PeligroTexto, 12);
                    break;
                default:
                    Tema.EstilizarBadge(labelEstadoBadge, Tema.AlertaSuave, Tema.AlertaTexto, 12);
                    break;
            }
        }

        private void CargarClientes(int? seleccionarId = null)
        {
            try
            {
                comboBox1Cliente.DataSource = bll.ObtenerClientes();
                comboBox1Cliente.DisplayMember = "nombre";
                comboBox1Cliente.ValueMember = "id_cliente";
                if (seleccionarId.HasValue)
                {
                    comboBox1Cliente.SelectedValue = seleccionarId.Value;
                }
            }
            catch
            {
                comboBox1Cliente.DataSource = null;
            }
        }

        private void CargarProductos()
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

        private void CargarPedidos()
        {
            dataGridViewPedidos.Rows.Clear();
            try
            {
                foreach (var p in bll.ObtenerPedidos(filtroActual))
                {
                    string entrega = p.FechaEntrega.HasValue ? p.FechaEntrega.Value.ToString("dd/MM") : "—";
                    int i = dataGridViewPedidos.Rows.Add(
                        p.IdPedido, p.NombreCliente, entrega, p.Estado, "C$ " + p.Total.ToString("N2"));
                    ColorearEstado(dataGridViewPedidos.Rows[i].Cells[3]);
                }
            }
            catch
            {
                // Sin conexión activa a la base de datos
            }
        }

        private Label lblPendientes, lblCancelados, lblEntregados, lblVentasDia;

        private void CrearTarjetas()
        {
            tableTarjeta.Controls.Clear();
            lblPendientes = AgregarTarjeta(0, "Pendientes", Tema.Alerta, Tema.TextoSuave);
            lblCancelados = AgregarTarjeta(1, "Cancelados", Tema.Peligro, Tema.TextoSuave);
            lblEntregados = AgregarTarjeta(2, "Entregados", Tema.Exito, Tema.TextoSuave);
            lblVentasDia = AgregarTarjeta(3, "Ventas del día", Tema.Vino, Tema.TextoSuave);
        }

        private Label AgregarTarjeta(int columna, string titulo, Color colorBarra, Color colorTitulo)
        {
            var margen = new Padding(columna == 0 ? 0 : 6, 0, columna == 3 ? 0 : 6, 0);
            var tarjeta = new Panel { Dock = DockStyle.Fill, Margin = margen, Padding = new Padding(14, 10, 12, 10) };
            Tema.PintarTarjeta(tarjeta, Tema.Blanco, Tema.Borde, 12);

            var barra = new Panel { BackColor = colorBarra, Location = new Point(12, 14), Size = new Size(5, 62) };
            var lblTitulo = new Label
            {
                Text = titulo,
                ForeColor = colorTitulo,
                AutoSize = true,
                Font = Tema.Fuente(9F, FontStyle.Bold),
                Location = new Point(26, 14)
            };
            var lblNumero = new Label
            {
                Text = "0",
                ForeColor = Tema.Oscuro,
                AutoSize = true,
                Font = Tema.Fuente(18F, FontStyle.Bold),
                Location = new Point(24, 38)
            };

            tarjeta.Controls.Add(lblNumero);
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(barra);
            tableTarjeta.Controls.Add(tarjeta, columna, 0);
            return lblNumero;
        }

        private void CargarResumen()
        {
            try
            {
                var r = bll.ObtenerResumen();
                lblPendientes.Text = r.Pendientes.ToString();
                lblCancelados.Text = r.Cancelados.ToString();
                lblEntregados.Text = r.Entregados.ToString();
                lblVentasDia.Text = "C$ " + r.VentasHoy.ToString("N0");
            }
            catch
            {
                lblPendientes.Text = "—";
                lblCancelados.Text = "—";
                lblEntregados.Text = "—";
                lblVentasDia.Text = "C$ 0";
            }
        }

        private void ColorearEstado(DataGridViewCell c)
        {
            c.Style.Font = Tema.Fuente(9F, FontStyle.Bold);
            switch (c.Value.ToString())
            {
                case "Pendiente":
                    c.Style.BackColor = Tema.AlertaSuave;
                    c.Style.ForeColor = Tema.AlertaTexto;
                    break;
                case "Preparando":
                case "En preparación":
                    c.Style.BackColor = Tema.RosaSuave;
                    c.Style.ForeColor = Tema.Vino;
                    break;
                case "Listo":
                case "Entregado":
                    c.Style.BackColor = Tema.ExitoSuave;
                    c.Style.ForeColor = Tema.ExitoTexto;
                    break;
                case "Cancelado":
                    c.Style.BackColor = Tema.PeligroSuave;
                    c.Style.ForeColor = Tema.PeligroTexto;
                    break;
            }
        }
        private void labelEstado_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetalle.CurrentRow == null || dataGridViewDetalle.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Selecciona un producto de la tabla.");
                return;
            }
            dataGridViewDetalle.Rows.Remove(dataGridViewDetalle.CurrentRow);
            CalcularTotal();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            bool esActualizacion = idPedidoActual > 0;
            var pedido = new PedidoEntity
            {
                IdPedido = idPedidoActual,
                IdCliente = comboBox1Cliente.SelectedValue == null
                            ? (int?)null : Convert.ToInt32(comboBox1Cliente.SelectedValue),
                FechaEntrega = dateEntrega.Checked ? (DateTime?)dateEntrega.Value.Date : null,
                Estado = comboBoxEstado.Text
            };

            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow || f.Tag == null) continue;
                pedido.Detalles.Add(new DetallePedidoEntity
                {
                    IdProducto = (int)f.Tag,
                    NombreProducto = f.Cells[0].Value.ToString(),
                    Cantidad = Convert.ToInt32(f.Cells[1].Value),
                    PrecioUnitario = Convert.ToDecimal(f.Cells[2].Value)
                });
            }

            try
            {
                int id = bll.Guardar(pedido);
                MessageBox.Show(esActualizacion
                    ? "Encargo N.º " + id + " actualizado."
                    : "Encargo N.º " + id + " guardado.");

                idPedidoActual = 0;
                labelDetalleTitulo.Text = "Nuevo encargo";
                buttonGuardar.Text = "Guardar";
                dataGridViewDetalle.Rows.Clear();
                labelTotal.Text = "C$ 0.00";
                comboBoxEstado.SelectedIndex = 0;
                dateEntrega.Checked = false;
                ActualizarBadgeEstado(comboBoxEstado.Text);
                CargarPedidos();
                CargarResumen();
                CargarProductos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar:\n" + ex.Message);
            }
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            idPedidoActual = 0;
            labelDetalleTitulo.Text = "Nuevo encargo";
            buttonGuardar.Text = "Guardar";
            dataGridViewDetalle.Rows.Clear();
            labelTotal.Text = "C$ 0.00";
            if (comboBoxEstado.Items.Count > 0) comboBoxEstado.SelectedIndex = 0;
            dateEntrega.Checked = false;
            numericUpDownCantidad.Value = 1;
            if (comboBox1Cliente.Items.Count > 0) comboBox1Cliente.SelectedIndex = 0;
            ActualizarBadgeEstado(comboBoxEstado.Text);
        }

        private void textBuscar_TextChanged(object sender, EventArgs e)
        {
            string t = ((TextBox)sender).Text.Trim().ToLower();
            dataGridViewPedidos.CurrentCell = null;

            foreach (DataGridViewRow f in dataGridViewPedidos.Rows)
            {
                if (f.IsNewRow) continue;
                string texto = (f.Cells[0].Value + " " + f.Cells[1].Value).ToLower();
                f.Visible = texto.Contains(t);
            }
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

        private void MostrarRecibo()
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
            recibo.AppendLine(idPedidoActual > 0 ? "Encargo N.º " + idPedidoActual : "Detalle del encargo");
            recibo.AppendLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            recibo.AppendLine("Cliente: " + comboBox1Cliente.Text);
            recibo.AppendLine("Entrega: " + (dateEntrega.Checked ? dateEntrega.Value.ToString("dd/MM/yyyy") : "Sin fecha"));
            recibo.AppendLine("Estado: " + comboBoxEstado.Text);
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
            recibo.AppendLine(linea);
            recibo.AppendLine(Centrar("¡GRACIAS POR SU ENCARGO!"));

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
                Text = "Recibo de encargo",
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
            g.DrawString("Cliente: " + comboBox1Cliente.Text, normal, Brushes.Black, 40, y); y += 25;
            g.DrawString("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), normal, Brushes.Black, 40, y); y += 35;

            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow || f.Cells[0].Value == null) continue;
                g.DrawString(f.Cells[1].Value + " x " + f.Cells[0].Value, normal, Brushes.Black, 40, y);
                g.DrawString("C$ " + Convert.ToDecimal(f.Cells[3].Value).ToString("N2"), normal, Brushes.Black, 500, y);
                y += 25;
            }

            y += 15;
            g.DrawString("Total: " + labelTotal.Text, titulo, Brushes.Black, 40, y);
        }

        private void buttonImprimir_Click(object sender, EventArgs e)
        {
            MostrarRecibo();
        }




        private void dataGridViewPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridViewPedidos.Rows[e.RowIndex].Cells[0].Value);
            CargarDetallePedido(id);
        }

        private void CargarDetallePedido(int id)
        {
            var p = bll.ObtenerPorId(id);
            if (p == null) return;

            if (p.IdCliente.HasValue) comboBox1Cliente.SelectedValue = p.IdCliente.Value;
            if (p.FechaEntrega.HasValue)
            {
                dateEntrega.Checked = true;
                dateEntrega.Value = p.FechaEntrega.Value;
            }
            else dateEntrega.Checked = false;
            comboBoxEstado.Text = p.Estado;

            dataGridViewDetalle.Rows.Clear();
            foreach (var d in p.Detalles)
            {
                int i = dataGridViewDetalle.Rows.Add(
                    d.NombreProducto, d.Cantidad, d.PrecioUnitario, d.Subtotal);
                dataGridViewDetalle.Rows[i].Tag = d.IdProducto;
            }

            idPedidoActual = id;
            labelDetalleTitulo.Text = "Encargo N.º " + id;
            buttonGuardar.Text = "Actualizar";
            ActualizarBadgeEstado(p.Estado);
            CalcularTotal();
        }

        private void buttonTodos_Click(object sender, EventArgs e)
        {
            filtroActual = "Todos";
            ActualizarBotonesFiltro();
            CargarPedidos();
        }

        private void buttonHoy_Click(object sender, EventArgs e)
        {
            filtroActual = "Hoy";
            ActualizarBotonesFiltro();
            CargarPedidos();
        }
        private void buttonPendiente_Click(object sender, EventArgs e)
        {
            filtroActual = "Pendiente";
            ActualizarBotonesFiltro();
            CargarPedidos();
        }

        private void tableTarjeta_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            if (comboBoxProducto.SelectedItem == null) return;

            var fila = (DataRowView)comboBoxProducto.SelectedItem;
            int idProd = Convert.ToInt32(fila["id_producto"]);
            string nombre = fila["nombre"].ToString();
            decimal precio = Convert.ToDecimal(fila["precio"]);
            int cant = (int)numericUpDownCantidad.Value;

            // si el producto ya está en la tabla, solo suma la cantidad
            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.IsNewRow) continue;
                if (f.Tag != null && (int)f.Tag == idProd)
                {
                    int nueva = Convert.ToInt32(f.Cells[1].Value) + cant;
                    f.Cells[1].Value = nueva;
                    f.Cells[3].Value = nueva * precio;
                    CalcularTotal();
                    return;
                }
            }

            int i = dataGridViewDetalle.Rows.Add(nombre, cant, precio, cant * precio);
            dataGridViewDetalle.Rows[i].Tag = idProd;
            numericUpDownCantidad.Value = 1;
            CalcularTotal();
        }
        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow f in dataGridViewDetalle.Rows)
            {
                if (f.Cells[3].Value != null)
                    total += Convert.ToDecimal(f.Cells[3].Value);
            }
            labelTotal.Text = "C$ " + total.ToString("N2");
        }
        private void Formpedidos_Load_1(object sender, EventArgs e)
        {
            CrearTarjetas();  

            labelfecha.Text = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy",
                            CultureInfo.GetCultureInfo("es-NI"));

            comboBoxEstado.Items.AddRange(new object[] { "Pendiente", "En preparación", "Listo", "Entregado", "Cancelado" });
            comboBoxEstado.SelectedIndex = 0;
            CargarClientes();
            CargarProductos();
            CargarPedidos();
            CargarResumen();
            SendMessage(textBuscar.Handle, EM_SETCUEBANNER, 1, "Buscar cliente o N.º de encargo");

        }
    }
}
