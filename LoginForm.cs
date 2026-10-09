using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Login_and_Register_System
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Server=DESKTOP-2R1VTGR\\SQLEXPRESS; Database=LaCasaDelSabor; Integrated Security=true;");

        private void label6_Click(object sender, EventArgs e)
        {
            
            this.Hide();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void registrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Usuario_Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;

               
                cmd.Parameters.AddWithValue("@correo", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@contrasena", txtPassword.Text.Trim());

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                 
                    string nombreUsuario = dr["nombre"].ToString();
                    string rolUsuario = dr["rol"].ToString();

                    MessageBox.Show("¡Bienvenido, " + nombreUsuario + "!\nRol: " + rolUsuario, "Acceso Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    conn.Close();

                    // Aquí abres tu ventana principal del sistema o lo que tenga ahi de momento(nota)
                    new Dashboard().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrectos, intenta de nuevo.", "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
                txtPassword.PasswordChar = '*'; 
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}