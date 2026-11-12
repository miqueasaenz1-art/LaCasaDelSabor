using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Login_and_Register_System.BLL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System
{
    public class UsuariosForm : Form
    {
        private readonly string nombreUsuario;
        private readonly string rolUsuario;
        private readonly Action onCerrarSesion;
        private readonly UsuarioBLL bll = new UsuarioBLL();

        private List<UsuarioEntity> usuarios = new List<UsuarioEntity>();
        private int idUsuarioActual = 0;
        private string filtroActual = "Todos";

        private Label lblTotalVal;
        private Label lblActivosVal;
        private Label lblAdminVal;
        private Label lblEstadoConexion;

        private TextBox txtBuscar;
        private Button btnTodos;
        private Button btnActivos;
        private Button btnInactivos;
        private DataGridView gridUsuarios;

        private Label lblFormTitulo;
        private Label lblEstadoBadge;
        private TextBox txtNombre;
        private TextBox txtCorreo;
        private ComboBox cmbRol;
        private TextBox txtContrasena;
        private CheckBox chkMostrarPass;
        private Label lblNotaPass;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnEliminar;

        public UsuariosForm(string nombre, string rol, Action cerrarSesion)
        {
            nombreUsuario = string.IsNullOrWhiteSpace(nombre) ? "Administrador" : nombre;
            rolUsuario = string.IsNullOrWhiteSpace(rol) ? "Administrador" : rol;
            onCerrarSesion = cerrarSesion;

            ConstruirInterfaz();
            Tema.Aplicar(this);
            Load += (s, e) => CargarUsuarios();
        }

        private void ConstruirInterfaz()
        {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Tema.Crema;
            ClientSize = new Size(1030, 690);
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

            // 1. Header + sesión activa
            var panelHeader = new Panel { Dock = DockStyle.Fill, Size = new Size(982, 72), BackColor = Tema.Crema };
            var lblTit = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(19F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, 2),
                Text = "Usuarios y Cuentas"
            };
            var lblSub = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9.5F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Location = new Point(2, 38),
                Text = "Sesión actual: " + nombreUsuario + " (" + rolUsuario + ")"
            };

            var btnCambiarCuenta = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelHeader.Width - 324, 14),
                Size = new Size(154, 38),
                Text = "Cambiar de cuenta"
            };
            Tema.EstilizarBotonSecundario(btnCambiarCuenta, 10);
            btnCambiarCuenta.Click += (s, e) => { if (onCerrarSesion != null) onCerrarSesion(); };

            var btnNuevo = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelHeader.Width - 158, 14),
                Size = new Size(158, 38),
                Text = "+ Nuevo usuario"
            };
            Tema.EstilizarBotonPrimario(btnNuevo, Tema.Vino, Tema.VinoHover, 10);
            btnNuevo.Click += (s, e) => PrepararNuevo();

            panelHeader.Controls.Add(btnNuevo);
            panelHeader.Controls.Add(btnCambiarCuenta);
            panelHeader.Controls.Add(lblSub);
            panelHeader.Controls.Add(lblTit);
            Tema.DecorarTituloSeccion(panelHeader, lblTit, Tema.MotivoPasteleria.GorroRepostero);

            // 2. Tarjetas KPI
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

            lblTotalVal = AgregarTarjeta(tablaKpi, 0, "Total de usuarios", "0", Tema.Vino);
            lblActivosVal = AgregarTarjeta(tablaKpi, 1, "Cuentas activas", "0", Tema.Exito);
            lblAdminVal = AgregarTarjeta(tablaKpi, 2, "Administradores", "0", Tema.Cafe);

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

            // Izquierda: Lista de usuarios
            var cardLista = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(18, 16, 18, 18)
            };
            Tema.PintarTarjeta(cardLista, Tema.Blanco, Tema.Borde, 14);

            var barraLista = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                Size = new Size(520, 82),
                BackColor = Tema.Blanco
            };

            var lblLista = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(11.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, 4),
                Text = "Cuentas registradas"
            };

            lblEstadoConexion = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(8.5F),
                ForeColor = Tema.PeligroTexto,
                Location = new Point(165, 7),
                Text = ""
            };

            btnInactivos = CrearFiltro("Inactivos", 90);
            btnActivos = CrearFiltro("Activos", 82);
            btnTodos = CrearFiltro("Todos", 74);

            btnInactivos.Location = new Point(barraLista.Width - 90, 0);
            btnActivos.Location = new Point(barraLista.Width - 178, 0);
            btnTodos.Location = new Point(barraLista.Width - 258, 0);

            btnTodos.Click += (s, e) => CambiarFiltro("Todos");
            btnActivos.Click += (s, e) => CambiarFiltro("Activos");
            btnInactivos.Click += (s, e) => CambiarFiltro("Inactivos");

            var panelBuscar = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, 38),
                Size = new Size(barraLista.Width, 34)
            };
            Tema.PintarTarjeta(panelBuscar, Tema.InputFondo, Tema.Borde, 8);

            txtBuscar = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.None,
                BackColor = Tema.InputFondo,
                ForeColor = Tema.Oscuro,
                Font = Tema.Fuente(9.5F),
                Location = new Point(12, 8),
                Size = new Size(panelBuscar.Width - 24, 18)
            };
            Tema.HabilitarPlaceholder(txtBuscar, "Buscar por nombre, correo o rol...");
            txtBuscar.TextChanged += (s, e) => AplicarFiltros();
            panelBuscar.Controls.Add(txtBuscar);

            barraLista.Controls.Add(panelBuscar);
            barraLista.Controls.Add(btnInactivos);
            barraLista.Controls.Add(btnActivos);
            barraLista.Controls.Add(btnTodos);
            barraLista.Controls.Add(lblEstadoConexion);
            barraLista.Controls.Add(lblLista);

            gridUsuarios = new DataGridView
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
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", FillWeight = 12 });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", FillWeight = 28 });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCorreo", HeaderText = "Correo", FillWeight = 30 });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRol", HeaderText = "Rol", FillWeight = 18 });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", FillWeight = 16 });
            gridUsuarios.Columns["colId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridUsuarios.Columns["colEstado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridUsuarios.CellClick += GridUsuarios_CellClick;

            cardLista.Controls.Add(gridUsuarios);
            cardLista.Controls.Add(barraLista);

            // Derecha: Editor de usuario
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
                Text = "Nuevo usuario"
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

            int y = 12;
            panelCampos.Controls.Add(CrearEtiqueta("Nombre completo", 0, y));
            y += 20;
            txtNombre = CrearCampo(panelCampos, 0, y, panelFormTop.Width);
            y += 40;

            panelCampos.Controls.Add(CrearEtiqueta("Correo electrónico", 0, y));
            y += 20;
            txtCorreo = CrearCampo(panelCampos, 0, y, panelFormTop.Width);
            Tema.HabilitarPlaceholder(txtCorreo, "usuario@lacasadelsabor.com");
            y += 40;

            panelCampos.Controls.Add(CrearEtiqueta("Rol", 0, y));
            y += 20;
            cmbRol = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = Tema.Fuente(10F),
                Location = new Point(0, y),
                Size = new Size(panelFormTop.Width, 28)
            };
            cmbRol.Items.AddRange(UsuarioBLL.RolesDisponibles);
            cmbRol.SelectedIndex = 1; // Cajero por defecto
            panelCampos.Controls.Add(cmbRol);
            y += 40;

            panelCampos.Controls.Add(CrearEtiqueta("Contraseña", 0, y));
            y += 20;
            txtContrasena = CrearCampo(panelCampos, 0, y, panelFormTop.Width);
            txtContrasena.PasswordChar = '●';
            y += 30;

            chkMostrarPass = new CheckBox
            {
                AutoSize = true,
                Font = Tema.Fuente(8.5F),
                ForeColor = Tema.TextoSuave,
                Location = new Point(0, y),
                Text = "Mostrar contraseña"
            };
            chkMostrarPass.CheckedChanged += (s, e) =>
            {
                txtContrasena.PasswordChar = chkMostrarPass.Checked ? '\0' : '●';
            };
            panelCampos.Controls.Add(chkMostrarPass);
            y += 24;

            lblNotaPass = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, y),
                Size = new Size(panelFormTop.Width, 20),
                Font = Tema.Fuente(8.2F),
                ForeColor = Tema.TextoSuave,
                Text = "Mínimo 4 caracteres."
            };
            panelCampos.Controls.Add(lblNotaPass);
            y += 28;

            chkActivo = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Font = Tema.Fuente(9.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(0, y),
                Text = "Cuenta activa (puede iniciar sesión)"
            };
            panelCampos.Controls.Add(chkActivo);

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
                Text = "Guardar usuario"
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

            cuerpo.Controls.Add(cardLista, 0, 0);
            cuerpo.Controls.Add(cardForm, 1, 0);

            layout.Controls.Add(panelHeader, 0, 0);
            layout.Controls.Add(tablaKpi, 0, 1);
            layout.Controls.Add(cuerpo, 0, 2);
            Controls.Add(layout);

            ActualizarBotonesFiltro();
        }

        private Button CrearFiltro(string texto, int ancho)
        {
            return new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(ancho, 28),
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

        private void CambiarFiltro(string filtro)
        {
            filtroActual = filtro;
            ActualizarBotonesFiltro();
            AplicarFiltros();
        }

        private void ActualizarBotonesFiltro()
        {
            Tema.EstilizarFiltro(btnTodos, filtroActual == "Todos");
            Tema.EstilizarFiltro(btnActivos, filtroActual == "Activos");
            Tema.EstilizarFiltro(btnInactivos, filtroActual == "Inactivos");
        }

        private void CargarUsuarios()
        {
            try
            {
                usuarios = bll.ObtenerTodos();
                lblEstadoConexion.Text = "";
            }
            catch (Exception ex)
            {
                usuarios = new List<UsuarioEntity>();
                lblEstadoConexion.Text = "No se pudieron cargar usuarios: " + ex.Message;
            }

            ActualizarResumen();
            AplicarFiltros();
        }

        private void ActualizarResumen()
        {
            int total = usuarios.Count;
            int activos = 0;
            int admins = 0;

            foreach (var u in usuarios)
            {
                if (u.Activo) activos++;
                if (u.Activo && string.Equals(u.Rol, "Administrador", StringComparison.OrdinalIgnoreCase))
                    admins++;
            }

            lblTotalVal.Text = total.ToString();
            lblActivosVal.Text = activos.ToString();
            lblAdminVal.Text = admins.ToString();
        }

        private void AplicarFiltros()
        {
            gridUsuarios.Rows.Clear();
            string q = (txtBuscar.Text ?? "").Trim().ToLower();

            foreach (var u in usuarios)
            {
                if (filtroActual == "Activos" && !u.Activo) continue;
                if (filtroActual == "Inactivos" && u.Activo) continue;

                if (!string.IsNullOrEmpty(q) &&
                    !u.Nombre.ToLower().Contains(q) &&
                    !u.Correo.ToLower().Contains(q) &&
                    !u.Rol.ToLower().Contains(q) &&
                    !u.IdUsuario.ToString().Contains(q))
                    continue;

                string estado = u.Activo ? "Activo" : "Inactivo";
                int idx = gridUsuarios.Rows.Add(u.IdUsuario, u.Nombre, u.Correo, u.Rol, estado);
                gridUsuarios.Rows[idx].Tag = u;

                var celda = gridUsuarios.Rows[idx].Cells[4];
                celda.Style.Font = Tema.Fuente(9F, FontStyle.Bold);
                celda.Style.BackColor = u.Activo ? Tema.ExitoSuave : Tema.PeligroSuave;
                celda.Style.ForeColor = u.Activo ? Tema.ExitoTexto : Tema.PeligroTexto;
            }
        }

        private void GridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var u = gridUsuarios.Rows[e.RowIndex].Tag as UsuarioEntity;
            if (u == null) return;

            idUsuarioActual = u.IdUsuario;
            lblFormTitulo.Text = "Usuario N.º " + u.IdUsuario;
            txtNombre.Text = u.Nombre;
            txtCorreo.Text = u.Correo;

            if (!cmbRol.Items.Contains(u.Rol) && !string.IsNullOrWhiteSpace(u.Rol))
            {
                cmbRol.Items.Add(u.Rol);
            }
            cmbRol.SelectedItem = u.Rol;

            txtContrasena.Text = "";
            chkActivo.Checked = u.Activo;
            lblNotaPass.Text = "Déjala vacía para conservar la contraseña actual.";

            lblEstadoBadge.Text = u.Activo ? "Activo" : "Inactivo";
            Tema.EstilizarBadge(
                lblEstadoBadge,
                u.Activo ? Tema.ExitoSuave : Tema.PeligroSuave,
                u.Activo ? Tema.ExitoTexto : Tema.PeligroTexto,
                12);

            btnGuardar.Text = "Actualizar usuario";
            btnEliminar.Enabled = true;
        }

        private void PrepararNuevo()
        {
            idUsuarioActual = 0;
            lblFormTitulo.Text = "Nuevo usuario";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            if (cmbRol.Items.Count > 1) cmbRol.SelectedIndex = 1;
            txtContrasena.Text = "";
            chkMostrarPass.Checked = false;
            chkActivo.Checked = true;
            lblNotaPass.Text = "Mínimo 4 caracteres.";
            lblEstadoBadge.Text = "Nuevo";
            Tema.EstilizarBadge(lblEstadoBadge, Tema.RosaSuave, Tema.Vino, 12);
            btnGuardar.Text = "Guardar usuario";
            btnEliminar.Enabled = false;
            txtNombre.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            var usuario = new UsuarioEntity
            {
                IdUsuario = idUsuarioActual,
                Nombre = txtNombre.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Contrasena = txtContrasena.Text.Trim(),
                Rol = cmbRol.Text,
                Activo = chkActivo.Checked
            };

            try
            {
                bool esNuevo = idUsuarioActual <= 0;
                int id = bll.Guardar(usuario);
                MessageBox.Show(
                    esNuevo ? "Usuario creado correctamente (N.º " + id + ")." : "Usuario actualizado correctamente.",
                    "Usuarios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                PrepararNuevo();
                CargarUsuarios();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el usuario:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioActual <= 0) return;

            var resp = MessageBox.Show(
                "¿Eliminar permanentemente al usuario \"" + txtNombre.Text.Trim() + "\"?\n\nSi solo deseas bloquear su acceso, desmarca \"Cuenta activa\" y guarda.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                bll.Eliminar(idUsuarioActual, nombreUsuario);
                MessageBox.Show("Usuario eliminado correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrepararNuevo();
                CargarUsuarios();
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
