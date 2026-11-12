using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Login_and_Register_System.DAL;

namespace Login_and_Register_System
{
    /// <summary>
    /// Permite configurar en tiempo de ejecución la IP/instancia de SQL Server
    /// (local o en la PC de un compañero) y probar la conexión antes de guardar.
    /// </summary>
    public class ConfigurarServidorForm : Form
    {
        private ComboBox cmbServidor;
        private TextBox txtBaseDatos;
        private RadioButton rbAuthSql;
        private RadioButton rbAuthWindows;
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private Label lblUsuario;
        private Label lblContrasena;
        private Label lblEstado;
        private Button btnDetectar;
        private Button btnProbar;
        private Button btnRestablecer;
        private Button btnGuardar;

        public ConfigurarServidorForm()
        {
            ConstruirInterfaz();
            Tema.Aplicar(this);
            AplicarEstilos();
            CargarConfiguracionActual();
        }

        private void ConstruirInterfaz()
        {
            Text = "Configurar servidor SQL Server";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(470, 495);
            BackColor = Tema.Crema;
            ShowInTaskbar = false;

            var panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Tema.Crema
            };
            Tema.HabilitarArrastre(panelHeader, this);

            var btnCerrar = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(428, 4),
                Size = new Size(36, 30),
                Text = "✕",
                Font = Tema.Fuente(10F, FontStyle.Bold),
                ForeColor = Tema.Vino,
                BackColor = Tema.Crema,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Tema.PeligroSuave;
            btnCerrar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            panelHeader.Controls.Add(btnCerrar);

            var lblTitulo = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(15F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Location = new Point(26, 20),
                Text = "Conexión a Base de Datos"
            };

            var lblSub = new Label
            {
                AutoSize = true,
                Font = Tema.Fuente(9F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Location = new Point(28, 50),
                Text = "Configura el servidor SQL Server y las credenciales de conexión"
            };

            var card = new Panel
            {
                Location = new Point(26, 80),
                Size = new Size(418, 390),
                BackColor = Tema.Blanco
            };
            Tema.PintarTarjeta(card, Tema.Blanco, Tema.Borde, 14);

            int y = 18;
            card.Controls.Add(CrearEtiqueta("Servidor o dirección IP", 20, y));
            y += 20;
            cmbServidor = new ComboBox
            {
                Location = new Point(20, y),
                Size = new Size(262, 26),
                Font = Tema.Fuente(9.5F),
                DropDownStyle = ComboBoxStyle.DropDown,
                BackColor = Tema.InputFondo,
                ForeColor = Tema.Oscuro
            };
            foreach (string inst in ConexionBD.ObtenerInstanciasLocalesCandidatas())
            {
                cmbServidor.Items.Add(inst);
            }
            card.Controls.Add(cmbServidor);

            btnDetectar = new Button
            {
                Location = new Point(290, y - 2),
                Size = new Size(108, 28),
                Text = "Detectar local"
            };
            btnDetectar.Click += BtnDetectar_Click;
            card.Controls.Add(btnDetectar);
            y += 40;

            card.Controls.Add(CrearEtiqueta("Base de datos", 20, y));
            y += 20;
            txtBaseDatos = CrearCampoTexto(card, 20, y, 378);
            Tema.HabilitarPlaceholder(txtBaseDatos, "LaCasaDelSabor");
            y += 42;

            rbAuthSql = new RadioButton
            {
                AutoSize = true,
                Location = new Point(20, y),
                Font = Tema.Fuente(9F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Text = "Autenticación SQL Server (recomendada en red)",
                Checked = true
            };
            rbAuthSql.CheckedChanged += (s, e) => ActualizarModoAuth();
            card.Controls.Add(rbAuthSql);
            y += 24;

            rbAuthWindows = new RadioButton
            {
                AutoSize = true,
                Location = new Point(20, y),
                Font = Tema.Fuente(9F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Text = "Autenticación de Windows (Integrated Security)"
            };
            rbAuthWindows.CheckedChanged += (s, e) => ActualizarModoAuth();
            card.Controls.Add(rbAuthWindows);
            y += 30;

            lblUsuario = CrearEtiqueta("Usuario SQL", 20, y);
            lblContrasena = CrearEtiqueta("Contraseña", 216, y);
            card.Controls.Add(lblUsuario);
            card.Controls.Add(lblContrasena);
            y += 20;

            txtUsuario = CrearCampoTexto(card, 20, y, 182);
            Tema.HabilitarPlaceholder(txtUsuario, "Ej: sa");
            txtContrasena = CrearCampoTexto(card, 216, y, 182);
            txtContrasena.PasswordChar = '●';
            y += 42;

            var lblTip = new Label
            {
                Location = new Point(20, y),
                Size = new Size(378, 34),
                Font = Tema.Fuente(8.2F, FontStyle.Regular),
                ForeColor = Tema.TextoSuave,
                Text = "Para conexiones en red, habilita TCP/IP en SQL Server y el puerto 1433 en el Firewall."
            };
            card.Controls.Add(lblTip);
            y += 38;

            lblEstado = new Label
            {
                Location = new Point(20, y),
                Size = new Size(378, 30),
                Font = Tema.Fuente(8.5F, FontStyle.Bold),
                ForeColor = Tema.TextoSuave,
                Text = ""
            };
            card.Controls.Add(lblEstado);
            y += 34;

            btnProbar = new Button
            {
                Location = new Point(20, y),
                Size = new Size(134, 34),
                Text = "Probar conexión"
            };
            btnProbar.Click += BtnProbar_Click;

            btnRestablecer = new Button
            {
                Location = new Point(160, y),
                Size = new Size(116, 34),
                Text = "Usar App.config"
            };
            btnRestablecer.Click += BtnRestablecer_Click;

            btnGuardar = new Button
            {
                Location = new Point(282, y),
                Size = new Size(116, 34),
                Text = "Guardar"
            };
            btnGuardar.Click += BtnGuardar_Click;

            card.Controls.Add(btnProbar);
            card.Controls.Add(btnRestablecer);
            card.Controls.Add(btnGuardar);

            Controls.Add(card);
            Controls.Add(lblSub);
            Controls.Add(lblTitulo);
            Controls.Add(panelHeader);

            AcceptButton = btnGuardar;
            CancelButton = btnCerrar;

            Paint += (s, e) =>
            {
                using (var pluma = new Pen(Tema.BordeFuerte, 1.5f))
                {
                    e.Graphics.DrawRectangle(pluma, 0, 0, Width - 1, Height - 1);
                }
            };
        }

        private void AplicarEstilos()
        {
            Tema.RedondearFormulario(this, 16);
            Tema.EstilizarBotonSecundario(btnDetectar, 8);
            btnDetectar.Font = Tema.Fuente(8.5F, FontStyle.Bold);
            Tema.EstilizarBotonSecundario(btnProbar, 8);
            Tema.EstilizarBotonSecundario(btnRestablecer, 8);
            Tema.EstilizarBotonPrimario(btnGuardar, Tema.Vino, Tema.VinoHover, 8);
        }

        private Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Location = new Point(x, y),
                Font = Tema.Fuente(8.5F, FontStyle.Bold),
                ForeColor = Tema.Oscuro,
                Text = texto
            };
        }

        private TextBox CrearCampoTexto(Panel contenedor, int x, int y, int ancho)
        {
            var tb = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(ancho, 26),
                Font = Tema.Fuente(9.5F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Tema.InputFondo,
                ForeColor = Tema.Oscuro
            };
            contenedor.Controls.Add(tb);
            return tb;
        }

        private void CargarConfiguracionActual()
        {
            var cfg = ConexionBD.ObtenerConfiguracionActual();
            cmbServidor.Text = string.IsNullOrWhiteSpace(cfg.DataSource) ? "localhost" : cfg.DataSource;
            txtBaseDatos.Text = string.IsNullOrWhiteSpace(cfg.InitialCatalog) ? "LaCasaDelSabor" : cfg.InitialCatalog;

            if (cfg.IntegratedSecurity)
            {
                rbAuthWindows.Checked = true;
                txtUsuario.Text = "";
                txtContrasena.Text = "";
            }
            else
            {
                rbAuthSql.Checked = true;
                txtUsuario.Text = cfg.UserID ?? "";
                txtContrasena.Text = cfg.Password ?? "";
            }

            ActualizarModoAuth();
            if (ConexionBD.TieneOverrideLocal())
            {
                lblEstado.ForeColor = Tema.TextoSuave;
                lblEstado.Text = "Usando configuración guardada en este equipo.";
            }
        }

        private void ActualizarModoAuth()
        {
            bool usarSql = rbAuthSql.Checked;
            lblUsuario.Enabled = usarSql;
            lblContrasena.Enabled = usarSql;
            txtUsuario.Enabled = usarSql;
            txtContrasena.Enabled = usarSql;
        }

        private void BtnDetectar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            lblEstado.ForeColor = Tema.TextoSuave;
            lblEstado.Text = "Buscando SQL Server local...";
            Application.DoEvents();

            try
            {
                bool encontroBd;
                string servidor = ConexionBD.DetectarInstanciaLocal(txtBaseDatos.Text, out encontroBd);
                if (!string.IsNullOrWhiteSpace(servidor))
                {
                    cmbServidor.Text = servidor;
                    rbAuthWindows.Checked = true;
                    ActualizarModoAuth();
                    lblEstado.ForeColor = encontroBd ? Tema.ExitoTexto : Tema.AlertaTexto;
                    lblEstado.Text = encontroBd
                        ? "Instancia detectada con la BD lista: " + servidor
                        : "Instancia detectada (" + servidor + "), pero ahí no existe la BD.";
                }
                else
                {
                    lblEstado.ForeColor = Tema.PeligroTexto;
                    lblEstado.Text = "No se detectó SQL Server local activo. Revisa el servicio en Windows.";
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnProbar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            lblEstado.ForeColor = Tema.TextoSuave;
            lblEstado.Text = "Probando conexión...";
            Application.DoEvents();

            try
            {
                ConexionBD.ProbarConexion(
                    cmbServidor.Text,
                    txtBaseDatos.Text,
                    rbAuthWindows.Checked,
                    txtUsuario.Text,
                    txtContrasena.Text);

                lblEstado.ForeColor = Tema.ExitoTexto;
                lblEstado.Text = "Conexión exitosa con el servidor SQL.";
            }
            catch (ArgumentException ex)
            {
                lblEstado.ForeColor = Tema.AlertaTexto;
                lblEstado.Text = ex.Message;
            }
            catch (SqlException ex)
            {
                lblEstado.ForeColor = Tema.PeligroTexto;
                lblEstado.Text = "Error SQL (" + ex.Number + "): " + ex.Message;
            }
            catch (Exception ex)
            {
                lblEstado.ForeColor = Tema.PeligroTexto;
                lblEstado.Text = ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnRestablecer_Click(object sender, EventArgs e)
        {
            ConexionBD.RestablecerConfiguracion();
            CargarConfiguracionActual();
            lblEstado.ForeColor = Tema.ExitoTexto;
            lblEstado.Text = "Configuración restablecida desde App.config.";
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexionBD.GuardarConfiguracion(
                    cmbServidor.Text,
                    txtBaseDatos.Text,
                    rbAuthWindows.Checked,
                    txtUsuario.Text,
                    txtContrasena.Text);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                lblEstado.ForeColor = Tema.AlertaTexto;
                lblEstado.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblEstado.ForeColor = Tema.PeligroTexto;
                lblEstado.Text = "No se pudo guardar: " + ex.Message;
            }
        }
    }
}
