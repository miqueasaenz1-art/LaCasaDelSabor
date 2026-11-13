using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Login_and_Register_System.BLL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System
{
    public partial class ProductosForm : Form
    {
        private readonly ProductoBLL bll = new ProductoBLL();
        private readonly string rolUsuario;

        private List<ProductoEntity> productos = new List<ProductoEntity>();
        private List<CategoriaEntity> categorias = new List<CategoriaEntity>();
        private bool tieneColumnaStock = true;
        private int idProductoActual = 0;
        private string filtroActual = "Todos";

        private TextBox textBuscar;
        private Button btnActualizar;
        private Button btnNuevo;
        private Button btnTodos;
        private Button btnDisponibles;
        private Button btnStockBajo;
        private DataGridView gridProductos;
        private Label lblTotalVal;
        private Label lblDispVal;
        private Label lblBajoVal;
        private Label lblEstadoConexion;

        private Label lblFormTitulo;
        private Label lblEstadoBadge;
        private TextBox txtNombre;
        private ComboBox cmbCategoria;
        private Button btnNuevaCategoria;
        private TextBox txtDescripcion;
        private NumericUpDown numPrecio;
        private Label lblStock;
        private NumericUpDown numStock;
        private CheckBox chkDisponible;
        private Button btnGuardar;
        private Button btnEliminar;

        public ProductosForm() : this("Administrador")
        {
        }

        public ProductosForm(string rol)
        {
            rolUsuario = string.IsNullOrWhiteSpace(rol) ? "Administrador" : rol;
            InitializeComponent();
            ConstruirInterfaz();
            Tema.Aplicar(this);
            Load += ProductosForm_Load;
        }

        private void ConstruirInterfaz()
        {
            Padding = new Padding(24, 20, 24, 24);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Tema.Crema
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 1. Header
            var panelHeader = new Panel { Dock = DockStyle.Fill, Size = new Size(982, 72), BackColor = Tema.Crema };
            var lblTitulo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(19F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, 2),
                Text = "Catálogo de Productos"
            };
            var lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9.5F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Location = new Point(2, 38),
                Text = "Menú, precios, categorías y disponibilidad de cafetería y pastelería"
            };

            var panelBuscar = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelHeader.Width - 546, 14),
                Size = new Size(248, 38)
            };
            Tema.PintarTarjeta(panelBuscar, Tema.Blanco, Tema.Borde, 10);

            textBuscar = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Tema.Blanco,
                ForeColor = Tema.Oscuro,
                Font = Tema.Fuente(10F),
                Location = new Point(12, 9),
                Size = new Size(224, 20)
            };
            Tema.HabilitarPlaceholder(textBuscar, "Buscar producto o categoría...");
            textBuscar.TextChanged += (s, e) => AplicarFiltros();
            panelBuscar.Controls.Add(textBuscar);

            btnActualizar = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelHeader.Width - 284, 14),
                Size = new Size(116, 38),
                Text = "Actualizar"
            };
            Tema.EstilizarBotonSecundario(btnActualizar, 10);
            btnActualizar.Click += (s, e) => CargarTodo();

            btnNuevo = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelHeader.Width - 156, 14),
                Size = new Size(156, 38),
                Text = "+ Nuevo producto"
            };
            Tema.EstilizarBotonPrimario(btnNuevo, Tema.Vino, Tema.VinoHover, 10);
            btnNuevo.Click += (s, e) => PrepararNuevo();

            panelHeader.Controls.Add(btnNuevo);
            panelHeader.Controls.Add(btnActualizar);
            panelHeader.Controls.Add(panelBuscar);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(lblTitulo);
            Tema.DecorarTituloSeccion(panelHeader, lblTitulo, Tema.MotivoPasteleria.Tarta);

            // 2. KPI Cards
            var tablaKpi = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 2, 0, 12),
                BackColor = Tema.Crema
            };
            for (int i = 0; i < 3; i++)
                tablaKpi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tablaKpi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblTotalVal = AgregarTarjeta(tablaKpi, 0, "Total en catálogo", "0", Tema.Vino);
            lblDispVal = AgregarTarjeta(tablaKpi, 1, "Disponibles", "0", Tema.Exito);
            lblBajoVal = AgregarTarjeta(tablaKpi, 2, "No disponibles / Stock bajo", "0", Tema.Alerta);

            // 3. Cuerpo de 2 columnas
            var cuerpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Tema.Crema
            };
            cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            cuerpo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            cuerpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Izquierda: Tabla
            var cardTabla = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(18, 16, 18, 18)
            };
            Tema.PintarTarjeta(cardTabla, Tema.Blanco, Tema.Borde, 14);

            var barraTabla = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Size = new Size(520, 44),
                BackColor = Tema.Blanco
            };

            var lblLista = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(11.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, 8),
                Text = "Listado del menú"
            };

            lblEstadoConexion = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(8.5F, FontStyle.Regular),
                ForeColor = Tema.PeligroTexto,
                Location = new Point(140, 12),
                Text = ""
            };

            btnStockBajo = CrearFiltro("Agotados", 0);
            btnDisponibles = CrearFiltro("Disponibles", 1);
            btnTodos = CrearFiltro("Todos", 2);

            btnStockBajo.Location = new Point(barraTabla.Width - 96, 4);
            btnDisponibles.Location = new Point(barraTabla.Width - 204, 4);
            btnTodos.Location = new Point(barraTabla.Width - 284, 4);

            btnTodos.Click += (s, e) => CambiarFiltro("Todos");
            btnDisponibles.Click += (s, e) => CambiarFiltro("Disponibles");
            btnStockBajo.Click += (s, e) => CambiarFiltro("StockBajo");

            barraTabla.Controls.Add(btnStockBajo);
            barraTabla.Controls.Add(btnDisponibles);
            barraTabla.Controls.Add(btnTodos);
            barraTabla.Controls.Add(lblEstadoConexion);
            barraTabla.Controls.Add(lblLista);

            gridProductos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "N.º", FillWeight = 12 });
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Producto", FillWeight = 32 });
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCategoria", HeaderText = "Categoría", FillWeight = 20 });
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrecio", HeaderText = "Precio", FillWeight = 18 });
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStock", HeaderText = "Stock", FillWeight = 14 });
            gridProductos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", FillWeight = 20 });

            gridProductos.Columns["colId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridProductos.Columns["colPrecio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridProductos.Columns["colStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridProductos.Columns["colEstado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridProductos.CellClick += GridProductos_CellClick;

            cardTabla.Controls.Add(gridProductos);
            cardTabla.Controls.Add(barraTabla);

            // Derecha: Formulario de producto
            var cardForm = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10, 0, 0, 0),
                Padding = new Padding(20, 16, 20, 18),
                Size = new Size(400, 460)
            };
            Tema.PintarTarjeta(cardForm, Tema.Blanco, Tema.Borde, 14);

            var panelFormTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Tema.Blanco
            };

            lblFormTitulo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(11.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, 4),
                Text = "Nuevo producto"
            };

            lblEstadoBadge = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelFormTop.Width - 96, 3),
                Size = new Size(96, 24),
                Font = Tema.Fuente(8.5F, FontStyle.Bold),
                Text = "Nuevo"
            };
            Tema.EstilizarBadge(lblEstadoBadge, Tema.RosaSuave, Tema.Vino, 12);

            panelFormTop.Controls.Add(lblEstadoBadge);
            panelFormTop.Controls.Add(lblFormTitulo);

            var panelCampos = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Tema.Blanco,
                AutoScroll = true
            };

            int y = 10;
            panelCampos.Controls.Add(CrearEtiqueta("Nombre del producto", 0, y));
            y += 20;
            txtNombre = CrearCampo(panelCampos, 0, y, panelFormTop.Width);
            y += 38;

            panelCampos.Controls.Add(CrearEtiqueta("Categoría", 0, y));
            y += 20;
            cmbCategoria = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = Tema.Fuente(10F),
                Location = new Point(0, y),
                Size = new Size(panelFormTop.Width - 42, 28)
            };
            btnNuevaCategoria = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelFormTop.Width - 36, y - 1),
                Size = new Size(36, 29),
                Text = "+"
            };
            Tema.EstilizarBotonSecundario(btnNuevaCategoria, 8);
            btnNuevaCategoria.Click += BtnNuevaCategoria_Click;
            panelCampos.Controls.Add(cmbCategoria);
            panelCampos.Controls.Add(btnNuevaCategoria);
            y += 38;

            panelCampos.Controls.Add(CrearEtiqueta("Descripción", 0, y));
            y += 20;
            txtDescripcion = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, y),
                Size = new Size(panelFormTop.Width, 56),
                Multiline = true,
                Font = Tema.Fuente(9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Tema.InputFondo,
                ForeColor = Tema.Oscuro
            };
            panelCampos.Controls.Add(txtDescripcion);
            y += 66;

            panelCampos.Controls.Add(CrearEtiqueta("Precio (C$)", 0, y));
            lblStock = CrearEtiqueta("Existencia (stock)", 186, y);
            panelCampos.Controls.Add(lblStock);
            y += 20;

            numPrecio = new NumericUpDown
            {
                Location = new Point(0, y),
                Size = new Size(170, 28),
                Font = Tema.Fuente(10F),
                DecimalPlaces = 2,
                Maximum = 999999,
                Minimum = 0,
                Increment = 5
            };
            numStock = new NumericUpDown
            {
                Location = new Point(186, y),
                Size = new Size(140, 28),
                Font = Tema.Fuente(10F),
                DecimalPlaces = 0,
                Maximum = 999999,
                Minimum = 0
            };
            panelCampos.Controls.Add(numPrecio);
            panelCampos.Controls.Add(numStock);
            y += 42;

            chkDisponible = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Font = Tema.Fuente(9.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, y),
                Text = "Producto disponible para la venta"
            };
            panelCampos.Controls.Add(chkDisponible);

            var panelBotones = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Tema.Blanco
            };
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            panelBotones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            btnGuardar = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 6, 0),
                Text = "Guardar producto"
            };
            Tema.EstilizarBotonPrimario(btnGuardar, Tema.Exito, Tema.ExitoHover, 10);
            btnGuardar.Click += BtnGuardar_Click;

            btnEliminar = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 0, 0, 0),
                Text = "Eliminar",
                Enabled = false
            };
            Tema.EstilizarBotonPeligro(btnEliminar, 10);
            btnEliminar.Click += BtnEliminar_Click;

            panelBotones.Controls.Add(btnGuardar, 0, 0);
            panelBotones.Controls.Add(btnEliminar, 1, 0);

            cardForm.Controls.Add(panelCampos);
            cardForm.Controls.Add(panelBotones);
            cardForm.Controls.Add(panelFormTop);

            cuerpo.Controls.Add(cardTabla, 0, 0);
            cuerpo.Controls.Add(cardForm, 1, 0);

            layout.Controls.Add(panelHeader, 0, 0);
            layout.Controls.Add(tablaKpi, 0, 1);
            layout.Controls.Add(cuerpo, 0, 2);
            Controls.Add(layout);

            ActualizarBotonesFiltro();
        }

        private Button CrearFiltro(string texto, int orden)
        {
            return new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(orden == 0 ? 96 : (orden == 1 ? 102 : 74), 28),
                Text = texto
            };
        }

        private Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Location = new Point(x, y),
                Font = Tema.Fuente(8.5F, FontStyle.Bold),
                ForeColor = Tema.Cafe,
                Text = texto
            };
        }

        private TextBox CrearCampo(Panel contenedor, int x, int y, int ancho)
        {
            var tb = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(x, y),
                Size = new Size(ancho, 26),
                Font = Tema.Fuente(10F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Tema.InputFondo,
                ForeColor = Tema.Oscuro
            };
            contenedor.Controls.Add(tb);
            return tb;
        }

        private Label AgregarTarjeta(TableLayoutPanel tabla, int col, string titulo, string valor, Color acento)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(col == 0 ? 0 : 8, 0, col == 2 ? 0 : 8, 0),
                Padding = new Padding(16, 12, 14, 12)
            };
            Tema.PintarTarjeta(card, Tema.Blanco, Tema.Borde, 12);

            var barra = new Panel
            {
                BackColor = acento,
                Location = new Point(14, 14),
                Size = new Size(5, 62)
            };
            var lblTit = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9F, FontStyle.Bold),
                ForeColor = Tema.TextoSuave,
                Location = new Point(30, 14),
                Text = titulo
            };
            var lblVal = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(19F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(28, 38),
                Text = valor
            };

            card.Controls.Add(lblVal);
            card.Controls.Add(lblTit);
            card.Controls.Add(barra);
            tabla.Controls.Add(card, col, 0);
            return lblVal;
        }

        private void ProductosForm_Load(object sender, EventArgs e)
        {
            CargarTodo();
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                btnNuevo.Visible = false;
                btnNuevaCategoria.Visible = false;
                btnGuardar.Visible = false;
                btnEliminar.Visible = false;
                txtNombre.ReadOnly = true;
                txtDescripcion.ReadOnly = true;
                numPrecio.Enabled = false;
                if (numStock != null) numStock.Enabled = false;
                cmbCategoria.Enabled = false;
                if (chkDisponible != null) chkDisponible.Enabled = false;
                lblFormTitulo.Text = "Consulta de producto";
            }
        }

        private void CargarTodo()
        {
            CargarCategorias();
            CargarProductos();
        }

        private void CargarCategorias(int? seleccionarId = null)
        {
            try
            {
                categorias = bll.ObtenerCategorias();
                cmbCategoria.DataSource = null;
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "IdCategoria";

                if (seleccionarId.HasValue)
                {
                    cmbCategoria.SelectedValue = seleccionarId.Value;
                }
            }
            catch
            {
                categorias = new List<CategoriaEntity>();
                cmbCategoria.DataSource = null;
            }
        }

        private void CargarProductos()
        {
            try
            {
                productos = bll.ObtenerTodos(out tieneColumnaStock);
                lblEstadoConexion.Text = "";
            }
            catch (Exception ex)
            {
                productos = new List<ProductoEntity>();
                lblEstadoConexion.Text = "Sin conexión (" + ex.Message + ")";
            }

            lblStock.Visible = tieneColumnaStock;
            numStock.Visible = tieneColumnaStock;
            gridProductos.Columns["colStock"].Visible = tieneColumnaStock;

            ActualizarResumen();
            AplicarFiltros();
        }

        private void CambiarFiltro(string filtro)
        {
            filtroActual = filtro;
            ActualizarBotonesFiltro();
            AplicarFiltros();
        }

        private void ActualizarBotonesFiltro()
        {
            Tema.EstilizarFiltro(btnTodos, filtroActual == "Todos");
            Tema.EstilizarFiltro(btnDisponibles, filtroActual == "Disponibles");
            Tema.EstilizarFiltro(btnStockBajo, filtroActual == "StockBajo");
        }

        private void ActualizarResumen()
        {
            int total = productos.Count;
            int disponibles = 0;
            int bajos = 0;

            foreach (var p in productos)
            {
                if (tieneColumnaStock)
                {
                    int stock = p.Stock ?? 0;
                    if (p.Disponibilidad && stock > 0) disponibles++;
                    if (!p.Disponibilidad || stock <= 5) bajos++;
                }
                else
                {
                    if (p.Disponibilidad) disponibles++;
                    else bajos++;
                }
            }

            lblTotalVal.Text = total.ToString();
            lblDispVal.Text = disponibles.ToString();
            lblBajoVal.Text = bajos.ToString();
        }

        private void AplicarFiltros()
        {
            gridProductos.Rows.Clear();
            string busqueda = (textBuscar.Text ?? "").Trim().ToLower();

            foreach (var p in productos)
            {
                int stock = p.Stock ?? 0;
                if (!string.IsNullOrEmpty(busqueda) &&
                    !p.Nombre.ToLower().Contains(busqueda) &&
                    !p.NombreCategoria.ToLower().Contains(busqueda) &&
                    !p.IdProducto.ToString().Contains(busqueda))
                    continue;

                if (tieneColumnaStock)
                {
                    if (filtroActual == "Disponibles" && !(p.Disponibilidad && stock > 0)) continue;
                    if (filtroActual == "StockBajo" && (p.Disponibilidad && stock > 5)) continue;
                }
                else
                {
                    if (filtroActual == "Disponibles" && !p.Disponibilidad) continue;
                    if (filtroActual == "StockBajo" && p.Disponibilidad) continue;
                }

                string estado;
                Color bg, fg;
                if (tieneColumnaStock)
                {
                    if (!p.Disponibilidad || stock <= 0)
                    {
                        estado = p.Disponibilidad ? "Agotado" : "No disponible";
                        bg = Tema.PeligroSuave;
                        fg = Tema.PeligroTexto;
                    }
                    else if (stock <= 5)
                    {
                        estado = "Stock bajo";
                        bg = Tema.AlertaSuave;
                        fg = Tema.AlertaTexto;
                    }
                    else
                    {
                        estado = "Disponible";
                        bg = Tema.ExitoSuave;
                        fg = Tema.ExitoTexto;
                    }
                }
                else
                {
                    estado = p.Disponibilidad ? "Disponible" : "No disponible";
                    bg = p.Disponibilidad ? Tema.ExitoSuave : Tema.PeligroSuave;
                    fg = p.Disponibilidad ? Tema.ExitoTexto : Tema.PeligroTexto;
                }

                object textoStock = tieneColumnaStock ? (object)stock : "—";
                int idx = gridProductos.Rows.Add(
                    p.IdProducto, p.Nombre, p.NombreCategoria, "C$ " + p.Precio.ToString("N2"), textoStock, estado);
                gridProductos.Rows[idx].Tag = p;

                var celdaEstado = gridProductos.Rows[idx].Cells["colEstado"];
                celdaEstado.Style.BackColor = bg;
                celdaEstado.Style.ForeColor = fg;
                celdaEstado.Style.Font = Tema.Fuente(9F, FontStyle.Bold);
            }
        }

        private void GridProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var p = gridProductos.Rows[e.RowIndex].Tag as ProductoEntity;
            if (p == null) return;

            idProductoActual = p.IdProducto;
            lblFormTitulo.Text = "Producto N.º " + p.IdProducto;
            txtNombre.Text = p.Nombre;
            cmbCategoria.SelectedValue = p.IdCategoria;
            txtDescripcion.Text = p.Descripcion ?? "";
            numPrecio.Value = Math.Min(numPrecio.Maximum, Math.Max(numPrecio.Minimum, p.Precio));
            if (tieneColumnaStock)
            {
                int stock = p.Stock ?? 0;
                numStock.Value = Math.Min(numStock.Maximum, Math.Max(numStock.Minimum, stock));
            }
            chkDisponible.Checked = p.Disponibilidad;

            lblEstadoBadge.Text = p.Disponibilidad ? "Activo" : "Inactivo";
            Tema.EstilizarBadge(
                lblEstadoBadge,
                p.Disponibilidad ? Tema.ExitoSuave : Tema.PeligroSuave,
                p.Disponibilidad ? Tema.ExitoTexto : Tema.PeligroTexto,
                12);

            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                lblFormTitulo.Text = "Producto N.º " + p.IdProducto + " (Consulta)";
                btnGuardar.Visible = false;
                btnEliminar.Visible = false;
            }
            else
            {
                btnGuardar.Text = "Actualizar producto";
                btnEliminar.Enabled = true;
            }
        }

        private void PrepararNuevo()
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                lblFormTitulo.Text = "Consulta de producto";
                btnGuardar.Visible = false;
                btnEliminar.Visible = false;
                return;
            }
            idProductoActual = 0;
            lblFormTitulo.Text = "Nuevo producto";
            txtNombre.Text = "";
            if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = 0;
            txtDescripcion.Text = "";
            numPrecio.Value = 0;
            numStock.Value = 0;
            chkDisponible.Checked = true;
            lblEstadoBadge.Text = "Nuevo";
            Tema.EstilizarBadge(lblEstadoBadge, Tema.RosaSuave, Tema.Vino, 12);
            btnGuardar.Text = "Guardar producto";
            btnEliminar.Enabled = false;
            txtNombre.Focus();
        }

        private void BtnNuevaCategoria_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                MessageBox.Show("Tu rol únicamente tiene permiso de consulta en el catálogo.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var dlg = new Form
            {
                Text = "Nueva categoría",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(340, 200),
                BackColor = Tema.Crema
            })
            {
                var lblN = new Label { Location = new Point(20, 18), AutoSize = true, Text = "Nombre de categoría", Font = Tema.Fuente(9F, FontStyle.Bold), ForeColor = Tema.Oscuro };
                var tbN = new TextBox { Location = new Point(20, 40), Size = new Size(300, 26), Font = Tema.Fuente(9.5F) };
                var lblD = new Label { Location = new Point(20, 76), AutoSize = true, Text = "Descripción (opcional)", Font = Tema.Fuente(9F, FontStyle.Bold), ForeColor = Tema.Oscuro };
                var tbD = new TextBox { Location = new Point(20, 98), Size = new Size(300, 26), Font = Tema.Fuente(9.5F) };
                var btnOk = new Button { Location = new Point(210, 144), Size = new Size(110, 34), Text = "Crear" };
                var btnCan = new Button { Location = new Point(94, 144), Size = new Size(106, 34), Text = "Cancelar", DialogResult = DialogResult.Cancel };
                Tema.EstilizarBotonPrimario(btnOk, Tema.Vino, Tema.VinoHover, 8);
                Tema.EstilizarBotonSecundario(btnCan, 8);

                btnOk.Click += (s, ev) =>
                {
                    try
                    {
                        int idCat = bll.CrearCategoria(tbN.Text, tbD.Text);
                        CargarCategorias(idCat);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Categorías", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                dlg.Controls.AddRange(new Control[] { lblN, tbN, lblD, tbD, btnCan, btnOk });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCan;
                dlg.ShowDialog(this);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                MessageBox.Show("Tu rol únicamente tiene permiso de consulta en el catálogo.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Crea o selecciona una categoría primero.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var producto = new ProductoEntity
            {
                IdProducto = idProductoActual,
                Nombre = txtNombre.Text.Trim(),
                IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = numPrecio.Value,
                Stock = tieneColumnaStock ? (int?)Convert.ToInt32(numStock.Value) : null,
                Disponibilidad = chkDisponible.Checked
            };

            try
            {
                bool esNuevo = idProductoActual <= 0;
                int id = bll.Guardar(producto);
                MessageBox.Show(
                    esNuevo ? "Producto N.º " + id + " agregado al catálogo." : "Producto actualizado correctamente.",
                    "Productos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                PrepararNuevo();
                CargarProductos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el producto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                MessageBox.Show("Tu rol no tiene permiso para eliminar productos.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (idProductoActual <= 0) return;

            var resp = MessageBox.Show(
                "¿Eliminar el producto \"" + txtNombre.Text.Trim() + "\"?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                bll.Eliminar(idProductoActual);
                MessageBox.Show("Producto eliminado correctamente.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrepararNuevo();
                CargarProductos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
