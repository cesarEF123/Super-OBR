using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Super_OBR
{
        public partial class frmLogin : Form
        {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";

        public frmLogin()
            {
                InitializeComponent();
                this.AcceptButton = btnLogin;
            }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Por favor, completa todos los campos.";
                lblError.Visible = true;
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_VerificarCredenciales", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Correo = email,
                                Rol = reader["rol"].ToString()
                            };

                            Sesion.Usuario = usuario;

                            MessageBox.Show($"Bienvenido {usuario.Nombre} ({usuario.Rol})", "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();

                            if (usuario.Rol.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                new frmPrincipalAdmin().Show();
                            }
                            else
                            {
                                new frmPrincipal().Show();
                            }
                        }
                        else
                        {
                            lblError.Text = "Correo o contraseña incorrectos.";
                            lblError.Visible = true;
                        }
                    }
                }
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load_1(object sender, EventArgs e)
        {

        }
    }
}
