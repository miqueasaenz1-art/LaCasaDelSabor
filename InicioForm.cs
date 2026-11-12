using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Login_and_Register_System.BLL;

namespace Login_and_Register_System
{
    public class InicioForm : Form
    {
        private readonly string nombreUsuario;
        private readonly string rolUsuario;
        private readonly Action<string> navegar;

        private Label lblSaludo;
        private Label lblSubtitulo;
        private Label lblFecha;
        private Label lblRol;

        private Label lblValVentas;
        private Label lblValPendientes;
        private Label lblValEntregados;
        private Label lblValProductos;

        public InicioForm(string nombre, string rol, Action<string> onNavegar)
        {
            nombreUsuario = string.IsNullOrWhiteSpace(nombre) ? "Equipo" : nombre;
            rolUsuario = string.IsNullOrWhiteSpace(rol) ? "Panel de Control" : rol;
            navegar = onNavegar;

            ConstruirInterfaz();
            Tema.Aplicar(this);
            AplicarEstilos();
            Load += InicioForm_Load;
        }

        private void ConstruirInterfaz()
        {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Tema.Crema;
            ClientSize = new Size(1030, 690);
            Padding = new Padding(24, 20, 24, 24);

            var layoutPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Tema.Crema
            };
            layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 148F)); // Banner
            layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 128F)); // KPIs
            layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));  // Título sección
            layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Accesos rápidos

            // 1. Banner de bienvenida
            var panelHero = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Vino,
                Size = new Size(980, 140),
                Margin = new Padding(0, 0, 0, 16),
                Padding = new Padding(28, 22, 28, 22)
            };
            Tema.PintarTarjeta(panelHero, Tema.Vino, Tema.VinoOscuro, 14);
            panelHero.Paint += (s, e) =>
            {
                if (panelHero.Width < 360 || panelHero.Height < 80) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (var clip = Tema.CrearRutaRedondeada(new Rectangle(1, 1, panelHero.Width - 2, panelHero.Height - 2), 14))
                {
                    var estadoClip = e.Graphics.Save();
                    e.Graphics.SetClip(clip);

                    Color[] chispas =
                    {
                        Color.FromArgb(65, Tema.Rosa),
                        Color.FromArgb(75, Tema.Dorado),
                        Color.FromArgb(55, Tema.Vainilla)
                    };
                    var zonaDerecha = new Rectangle(Math.Max(340, panelHero.Width - 420), 6, 400, panelHero.Height - 16);
                    Tema.DibujarChispas(e.Graphics, zonaDerecha, chispas, 23, 22);

                    // Halo suave y motivo de pastelería
                    int cx = panelHero.Width - 300;
                    if (cx > 430)
                    {
                        using (var bHalo = new SolidBrush(Color.FromArgb(26, Tema.Rosa)))
                        {
                            e.Graphics.FillEllipse(bHalo, cx - 10, 14, 108, 108);
                        }
                        Tema.DibujarIconoPasteleria(e.Graphics, new Rectangle(cx, 24, 78, 78), Tema.MotivoPasteleria.PastelPisos, Tema.Oscuro);
                        Tema.DibujarIconoPasteleria(e.Graphics, new Rectangle(cx + 68, 56, 42, 42), Tema.MotivoPasteleria.Cupcake, Tema.Oscuro);
                    }

                    // Onda sutil de glaseado al pie del banner
                    Tema.DibujarOndaGlaseado(e.Graphics, 0, panelHero.Height, panelHero.Width + 20, 22, Color.FromArgb(35, Tema.Rosa), true);

                    e.Graphics.Restore(estadoClip);
                }
            };

            lblSaludo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(20F, FontStyle.Bold),
                ForeColor = Tema.Blanco,
                Location = new Point(28, 22),
                Text = "¡Hola, " + nombreUsuario + "!"
            };

            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(10.5F, FontStyle.Regular),
                ForeColor = Tema.RosaSuave,
                Location = new Point(30, 62),
                Text = "La Casa del Sabor  •  Gestión de Cafetería y Pastelería"
            };

            lblFecha = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9.5F, FontStyle.Regular),
                ForeColor = Tema.Rosa,
                Location = new Point(30, 94),
                Text = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy", CultureInfo.GetCultureInfo("es-NI"))
            };

            lblRol = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(150, 30),
                Location = new Point(panelHero.Width - 178, 26),
                Font = Tema.Fuente(9.5F, FontStyle.Bold),
                Text = rolUsuario
            };

            panelHero.Controls.Add(lblRol);
            panelHero.Controls.Add(lblFecha);
            panelHero.Controls.Add(lblSubtitulo);
            panelHero.Controls.Add(lblSaludo);

            // 2. Tarjetas KPI
            var tablaKpis = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Tema.Crema
            };
            for (int i = 0; i < 4; i++)
                tablaKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tablaKpis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblValVentas = AgregarTarjetaKpi(tablaKpis, 0, "Ventas del día", "C$ 0.00", Tema.Vino);
            lblValPendientes = AgregarTarjetaKpi(tablaKpis, 1, "Encargos pendientes", "0", Tema.Alerta);
            lblValEntregados = AgregarTarjetaKpi(tablaKpis, 2, "Encargos entregados", "0", Tema.Exito);
            lblValProductos = AgregarTarjetaKpi(tablaKpis, 3, "Productos disponibles", "0", Tema.Cafe);

            // 3. Encabezado Accesos rápidos
            var lblSeccion = new Label
            {
                Dock = DockStyle.Fill,
                Font = Tema.Fuente(12.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Text = "Accesos rápidos del mostrador",
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(2, 0, 0, 8)
            };

            // 4. Tarjetas de acción
            var tablaAcciones = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Tema.Crema
            };
            for (int i = 0; i < 3; i++)
                tablaAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tablaAcciones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            AgregarTarjetaAccion(
                tablaAcciones, 0,
                "Punto de Venta",
                "Registra ventas rápidas en mostrador, calcula cambio en efectivo o tarjeta, imprime recibos y exporta reportes en PDF.",
                "Abrir Ventas",
                Tema.Vino,
                Tema.MotivoPasteleria.Cupcake,
                () => { if (navegar != null) navegar("Ventas"); });

            AgregarTarjetaAccion(
                tablaAcciones, 1,
                "Encargos y Pedidos",
                "Administra pedidos personalizados de pastelería con cliente, fecha de entrega, estado de avance y comprobante.",
                "Abrir Encargos",
                Tema.Cafe,
                Tema.MotivoPasteleria.PastelPisos,
                () => { if (navegar != null) navegar("Encargos"); });

            AgregarTarjetaAccion(
                tablaAcciones, 2,
                "Catálogo de Productos",
                "Consulta precios, existencias disponibles y alertas de stock bajo del menú de cafetería y pastelería.",
                "Ver Productos",
                Tema.Oscuro,
                Tema.MotivoPasteleria.Tarta,
                () => { if (navegar != null) navegar("Productos"); });

            layoutPrincipal.Controls.Add(panelHero, 0, 0);
            layoutPrincipal.Controls.Add(tablaKpis, 0, 1);
            layoutPrincipal.Controls.Add(lblSeccion, 0, 2);
            layoutPrincipal.Controls.Add(tablaAcciones, 0, 3);

            Controls.Add(layoutPrincipal);
        }

        private Label AgregarTarjetaKpi(TableLayoutPanel tabla, int col, string titulo, string valorInicial, Color colorBarra)
        {
            var margen = new Padding(col == 0 ? 0 : 8, 0, col == 3 ? 0 : 8, 0);
            var tarjeta = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = margen,
                Padding = new Padding(16, 14, 14, 14)
            };
            Tema.PintarTarjeta(tarjeta, Tema.Blanco, Tema.Borde, 12);

            var barra = new Panel
            {
                BackColor = colorBarra,
                Location = new Point(14, 16),
                Size = new Size(5, 76)
            };

            var lblTit = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9.5F, FontStyle.Bold),
                ForeColor = Tema.TextoSuave,
                Location = new Point(30, 16),
                Text = titulo
            };

            var lblVal = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(20F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(28, 44),
                Text = valorInicial
            };

            tarjeta.Controls.Add(lblVal);
            tarjeta.Controls.Add(lblTit);
            tarjeta.Controls.Add(barra);
            tabla.Controls.Add(tarjeta, col, 0);
            return lblVal;
        }

        private void AgregarTarjetaAccion(
            TableLayoutPanel tabla,
            int col,
            string titulo,
            string descripcion,
            string textoBoton,
            Color colorBoton,
            Tema.MotivoPasteleria motivo,
            Action alClick)
        {
            var margen = new Padding(col == 0 ? 0 : 8, 4, col == 2 ? 0 : 8, 0);
            var tarjeta = new Panel
            {
                Dock = DockStyle.Fill,
                Size = new Size(310, 260),
                Margin = margen,
                Padding = new Padding(22)
            };
            Tema.PintarTarjeta(tarjeta, Tema.Blanco, Tema.Borde, 14);
            tarjeta.Paint += (s, e) =>
            {
                if (tarjeta.Width < 120) return;
                var med = new Rectangle(tarjeta.Width - 64, 18, 42, 42);
                Tema.DibujarMedallonPasteleria(e.Graphics, med, motivo, Tema.RosaSuave, Tema.Borde, colorBoton);
            };

            var insignia = new Panel
            {
                BackColor = colorBoton,
                Location = new Point(22, 24),
                Size = new Size(42, 6)
            };

            var lblTit = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(13F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(20, 42),
                Text = titulo
            };

            var lblDesc = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = Tema.Fuente(9.5F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Location = new Point(20, 78),
                Size = new Size(260, 110),
                Text = descripcion
            };

            var btn = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(22, tarjeta.Height - 66),
                Size = new Size(tarjeta.Width - 44, 42),
                Text = textoBoton
            };
            Tema.EstilizarBotonPrimario(btn, colorBoton, Tema.VinoHover, 10);
            btn.Click += (s, e) => alClick();

            tarjeta.Controls.Add(btn);
            tarjeta.Controls.Add(lblDesc);
            tarjeta.Controls.Add(lblTit);
            tarjeta.Controls.Add(insignia);
            tabla.Controls.Add(tarjeta, col, 0);
        }

        private void AplicarEstilos()
        {
            Tema.EstilizarBadge(lblRol, Tema.Rosa, Tema.Oscuro, 14);
        }

        private void InicioForm_Load(object sender, EventArgs e)
        {
            CargarIndicadores();
        }

        private void CargarIndicadores()
        {
            try
            {
                var pedidoBll = new PedidoBLL();
                var resumen = pedidoBll.ObtenerResumen();
                lblValVentas.Text = "C$ " + resumen.VentasHoy.ToString("N2");
                lblValPendientes.Text = resumen.Pendientes.ToString();
                lblValEntregados.Text = resumen.Entregados.ToString();
            }
            catch
            {
                lblValVentas.Text = "C$ 0.00";
                lblValPendientes.Text = "—";
                lblValEntregados.Text = "—";
            }

            try
            {
                var ventaBll = new VentaBLL();
                var productos = ventaBll.ObtenerProductosDisponibles();
                lblValProductos.Text = productos.Rows.Count.ToString();
            }
            catch
            {
                lblValProductos.Text = "—";
            }
        }
    }
}
