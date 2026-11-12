using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Login_and_Register_System.DAL;

namespace Login_and_Register_System
{
    public partial class LoginForm : Form
    {
        private Button btnConfigServidor;

        public LoginForm()
        {
            InitializeComponent();
            InicializarPieServidor();
            Tema.Aplicar(this);
            AplicarEstilos();
        }

        private void InicializarPieServidor()
        {
            btnConfigServidor = new Button
            {
                Location = new Point(125, 563),
                Size = new Size(170, 30),
                Text = "Configurar conexión"
            };
            Tema.EstilizarBotonSecundario(btnConfigServidor, 8);
            btnConfigServidor.Font = Tema.Fuente(8.5F, FontStyle.Bold);
            btnConfigServidor.Click += BtnConfigServidor_Click;

            Controls.Add(btnConfigServidor);
        }

        private void BtnConfigServidor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ConfigurarServidorForm())
            {
                dlg.ShowDialog(this);
            }
        }

        private void AplicarEstilos()
        {
            Tema.RedondearFormulario(this, 16);
            Tema.HabilitarArrastre(panelHeader, this);
            Tema.HabilitarArrastre(pictureBox1, this);

            Tema.PintarTarjeta(panelCard, Tema.Blanco, Tema.Borde, 14);
            EstilizarCajaInput(panelCorreo, txtUsername);
            EstilizarCajaInput(panelPass, txtPassword);

            Tema.EstilizarBotonPrimario(registrationButton, Tema.Vino, Tema.VinoHover, 10);
            Tema.EstilizarBotonSecundario(clearButton, 10);

            Tema.HabilitarPlaceholder(txtUsername, "usuario@lacasadelsabor.com");
            Tema.HabilitarPlaceholder(txtPassword, "Ingresa tu contraseña");

            panelHeader.Paint += (s, e) =>
            {
                Tema.DibujarOndaGlaseado(e.Graphics, 0, panelHeader.Height + 1, panelHeader.Width + 12, 10, Tema.RosaSuave, true);
            };

            this.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color[] chispas =
                {
                    Color.FromArgb(145, Tema.Rosa),
                    Color.FromArgb(120, Tema.Caramelo),
                    Color.FromArgb(100, Tema.Vino)
                };
                Tema.DibujarChispas(e.Graphics, new Rectangle(6, 42, 42, 110), chispas, 3, 5);
                Tema.DibujarChispas(e.Graphics, new Rectangle(Width - 48, 42, 42, 110), chispas, 17, 5);

                Tema.DibujarMedallonPasteleria(e.Graphics, new Rectangle(12, 74, 32, 32), Tema.MotivoPasteleria.Cupcake, Tema.RosaSuave, Tema.Borde, Tema.Vino);
                Tema.DibujarMedallonPasteleria(e.Graphics, new Rectangle(Width - 44, 74, 32, 32), Tema.MotivoPasteleria.PastelPisos, Tema.RosaSuave, Tema.Borde, Tema.Cafe);

                Tema.DibujarOndaGlaseado(e.Graphics, 0, Height - 4, Width + 18, 18, Tema.RosaSuave, true);

                using (var pluma = new Pen(Tema.BordeFuerte, 1.5f))
                {
                    e.Graphics.DrawRectangle(pluma, 0, 0, Width - 1, Height - 1);
                }
            };
        }

        private void EstilizarCajaInput(Panel contenedor, TextBox campo)
        {
            Tema.PintarTarjeta(contenedor, Tema.InputFondo, Tema.Borde, 8);
            campo.Enter += (s, e) => Tema.PintarTarjeta(contenedor, Tema.Blanco, Tema.Vino, 8);
            campo.Leave += (s, e) => Tema.PintarTarjeta(contenedor, Tema.InputFondo, Tema.Borde, 8);
            campo.Enter += (s, e) => campo.BackColor = Tema.Blanco;
            campo.Leave += (s, e) => campo.BackColor = Tema.InputFondo;
            contenedor.Click += (s, e) => campo.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void registrationButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingresa tu correo y contraseña para continuar.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (string.IsNullOrWhiteSpace(txtUsername.Text)) txtUsername.Focus();
                else txtPassword.Focus();
                return;
            }

            string nombreUsuario = null;
            string rolUsuario = null;
            bool autenticado = false;

            try
            {
                using (var conn = ConexionBD.CrearConexion())
                {
                    conn.Open();

                    using (var cmd = new SqlCommand("sp_Usuario_Login", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@correo", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@contrasena", txtPassword.Text.Trim());

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                nombreUsuario = dr["nombre"].ToString();
                                rolUsuario = dr["rol"].ToString();
                                autenticado = true;
                            }
                        }
                    }
                }

                if (autenticado)
                {
                    MessageBox.Show("¡Bienvenido, " + nombreUsuario + "!\nRol: " + rolUsuario, "Acceso Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var dash = new Dashboard(nombreUsuario, rolUsuario);
                    dash.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrectos, intenta de nuevo.", "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2812)
                {
                    MessageBox.Show(
                        "Se conectó al servidor (" + ConexionBD.ObtenerResumenServidor() + "), pero falta el procedimiento almacenado 'sp_Usuario_Login' y la tabla 'Usuarios' en esa base de datos.",
                        "Procedimiento faltante en BD",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    var resp = MessageBox.Show(
                        "No se pudo conectar con la base de datos.\n\nDetalle (" + ex.Number + "): " + ex.Message +
                        "\n\n¿Deseas abrir la configuración de conexión?",
                        "Error de conexión",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);
                    if (resp == DialogResult.Yes)
                    {
                        BtnConfigServidor_Click(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxShowPass.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '●';
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            Task.Run(() => ConexionBD.AsegurarServidorLocal());
        }
    }
}
