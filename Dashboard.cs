using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Login_and_Register_System.BLL;

namespace Login_and_Register_System
{
    public partial class Dashboard : Form
    {
        public static Dashboard instance;
        public Label lbl;

        private readonly string nombreUsuario;
        private readonly string rolUsuario;
        private Button botonActivo;

        public Dashboard() : this("Administrador", "Panel de Control")
        {
        }

        public Dashboard(string nombre, string rol)
        {
            nombreUsuario = string.IsNullOrWhiteSpace(nombre) ? "Administrador" : nombre;
            rolUsuario = string.IsNullOrWhiteSpace(rol) ? "Panel de Control" : rol;

            InitializeComponent();
            instance = this;
            lbl = label1;

            Tema.Aplicar(this);
            ConfigurarDiseno();
        }

        private void ConfigurarDiseno()
        {
            Tema.RedondearFormulario(this, 14);
            Tema.HabilitarArrastre(panelTopBar, this);
            Tema.HabilitarArrastre(lblSeccionActual, this);
            Tema.HabilitarArrastre(panel2, this);

            panelTopBar.DoubleClick += (s, e) => AlternarMaximizar();
            lblSeccionActual.DoubleClick += (s, e) => AlternarMaximizar();

            label1.Text = nombreUsuario;

            // Logotipo oficial de pastelería (mismo de LoginForm)
            try
            {
                var rm = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
                var logoLogin = rm.GetObject("pictureBox1.Image") as Image;
                if (logoLogin != null)
                {
                    pictureBox1.Image = logoLogin;
                }
            }
            catch { }

            // Borde festoneado tipo glaseado al pie del encabezado del menú
            panel2.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Tema.DibujarOndaGlaseado(e.Graphics, 0, panel2.Height - 2, panel2.Width + 14, 14, Tema.Rosa, true);
                using (var p = new Pen(Tema.Borde, 1f))
                    e.Graphics.DrawLine(p, 14, panel2.Height - 1, panel2.Width - 14, panel2.Height - 1);
            };

            Tema.DecorarTituloSeccion(panelTopBar, lblSeccionActual, Tema.MotivoPasteleria.Cupcake);
            panelTopBar.Paint += (s, e) =>
            {
                using (var p = new Pen(Tema.Borde, 1f))
                    e.Graphics.DrawLine(p, 0, panelTopBar.Height - 1, panelTopBar.Width, panelTopBar.Height - 1);
            };

            panel1.Paint += (s, e) =>
            {
                using (var p = new Pen(Tema.Borde, 1f))
                {
                    e.Graphics.DrawLine(p, panel1.Width - 1, 0, panel1.Width - 1, panel1.Height);
                    e.Graphics.DrawLine(p, 16, button6.Top - 1, panel1.Width - 16, button6.Top - 1);
                }

                int espacioLibre = button6.Top - button5.Bottom;
                if (espacioLibre > 90)
                {
                    int cy = button6.Top - 76;
                    var med = new Rectangle((panel1.Width - 42) / 2, cy, 42, 42);
                    Tema.DibujarMedallonPasteleria(e.Graphics, med, Tema.MotivoPasteleria.PastelPisos, Tema.Crema, Tema.Borde, Tema.Cafe);
                    using (var f = Tema.Fuente(8F, FontStyle.Italic))
                    using (var b = new SolidBrush(Tema.TextoSuave))
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center })
                    {
                        e.Graphics.DrawString("Pastelería Artesanal", f, b, panel1.Width / 2f, cy + 48, sf);
                    }
                }
            };

            EstilizarBotonMenu(button1);
            EstilizarBotonMenu(button2);
            EstilizarBotonMenu(button3);
            EstilizarBotonMenu(button4);
            EstilizarBotonMenu(button5);

            button6.FlatAppearance.MouseOverBackColor = Tema.PeligroHover;
            button6.FlatAppearance.MouseDownBackColor = Tema.Rosa;

            panelIndicador.BringToFront();
            ConfigurarPermisosSegunRol();
        }

        private void ConfigurarPermisosSegunRol()
        {
            bool esAdmin = UsuarioBLL.EsAdmin(rolUsuario);
            bool esCajero = UsuarioBLL.EsCajero(rolUsuario);
            bool esPasteleria = UsuarioBLL.EsPasteleria(rolUsuario);

            if (!esAdmin && !esCajero && !esPasteleria)
            {
                esCajero = true;
            }

            // Administrador: Todo (Inicio, Ventas, Productos, Encargos, Usuarios)
            // Cajero: Inicio, Ventas (cobro), Productos (catálogo de consulta), Encargos (registrar). Usuarios oculto.
            // Pastelería: Encargos (comanda de producción). Inicio, Ventas, Productos y Usuarios ocultos.
            button1.Visible = !esPasteleria;
            button2.Visible = esAdmin || esCajero;
            button3.Visible = esAdmin || esCajero;
            button4.Visible = true;
            button5.Visible = esAdmin;

            // Reorganizar verticalmente los botones visibles para eliminar huecos
            int y = panel2.Bottom + 12;
            Button[] botones = { button1, button2, button3, button4, button5 };
            foreach (var b in botones)
            {
                if (b.Visible)
                {
                    b.Top = y;
                    y += 54;
                }
            }
        }

        private void EstilizarBotonMenu(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Tema.Rosa;
            btn.FlatAppearance.MouseDownBackColor = Tema.Rosa;
            btn.UseVisualStyleBackColor = false;
            btn.BackColor = Tema.RosaSuave;
            btn.ForeColor = Tema.Oscuro;
        }

        private void MarcarBotonActivo(Button btn, string tituloSeccion)
        {
            Button[] botones = { button1, button2, button3, button4, button5 };
            foreach (var b in botones)
            {
                b.BackColor = Tema.RosaSuave;
                b.ForeColor = Tema.Oscuro;
            }

            botonActivo = btn;
            if (botonActivo != null)
            {
                botonActivo.BackColor = Tema.Blanco;
                botonActivo.ForeColor = Tema.Vino;
                panelIndicador.Top = botonActivo.Top + 8;
                panelIndicador.Height = botonActivo.Height - 16;
                panelIndicador.Visible = true;
                panelIndicador.BringToFront();
            }

            lblSeccionActual.Text = tituloSeccion + "  •  La Casa del Sabor";
        }

        private void NavegarDesdeInicio(string destino)
        {
            switch (destino)
            {
                case "Ventas":
                    button2_Click(button2, EventArgs.Empty);
                    break;
                case "Productos":
                    button3_Click(button3, EventArgs.Empty);
                    break;
                case "Encargos":
                    button4_Click(button4, EventArgs.Empty);
                    break;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            if (UsuarioBLL.EsPasteleria(rolUsuario))
            {
                button4_Click(button4, EventArgs.Empty);
            }
            else
            {
                button1_Click(button1, EventArgs.Empty);
            }
        }

        private void AbrirFormularioEnPanel(Form formHijo)
        {
            if (this.panelContenedor.Controls.Count > 0)
            {
                var anterior = this.panelContenedor.Controls[0] as Form;
                this.panelContenedor.Controls.Clear();
                if (anterior != null) anterior.Dispose();
            }

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            this.panelContenedor.Controls.Add(formHijo);
            this.panelContenedor.Tag = formHijo;
            formHijo.Show();
        }

        private void CerrarSesion()
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(button1, "Inicio");
            AbrirFormularioEnPanel(new InicioForm(nombreUsuario, rolUsuario, NavegarDesdeInicio));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario) && !UsuarioBLL.EsCajero(rolUsuario))
            {
                MessageBox.Show("Tu rol no tiene acceso al módulo de Ventas.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MarcarBotonActivo(button2, "Punto de Venta");
            AbrirFormularioEnPanel(new VentasForm(rolUsuario));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario) && !UsuarioBLL.EsCajero(rolUsuario))
            {
                MessageBox.Show("Tu rol no tiene acceso al catálogo de productos.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string titulo = UsuarioBLL.EsAdmin(rolUsuario)
                ? "Catálogo de Productos"
                : "Catálogo de Productos (Consulta)";
            MarcarBotonActivo(button3, titulo);
            AbrirFormularioEnPanel(new ProductosForm(rolUsuario));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string titulo = UsuarioBLL.EsPasteleria(rolUsuario)
                ? "Comanda de Pastelería"
                : "Encargos y Pedidos";
            MarcarBotonActivo(button4, titulo);
            AbrirFormularioEnPanel(new Formpedidos(rolUsuario));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!UsuarioBLL.EsAdmin(rolUsuario))
            {
                MessageBox.Show("El módulo de gestión de usuarios está reservado exclusivamente para administradores.", "Acceso Restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MarcarBotonActivo(button5, "Usuarios y Sesión");
            AbrirFormularioEnPanel(new UsuariosForm(nombreUsuario, rolUsuario, CerrarSesion));
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            AlternarMaximizar();
        }

        private void AlternarMaximizar()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                btnMax.Text = "▢";
            }
            else
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
                btnMax.Text = "❐";
            }
        }

        private void btnCerrarApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
