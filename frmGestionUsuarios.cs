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


namespace Proyecto_Super_OBR
{
    public partial class frmGestionUsuarios : Form
    {
        private List<Usuario> listaUsuarios = new List<Usuario>();
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";
        private int usuarioSeleccionadoId = -1;

        public frmGestionUsuarios()
        {
            InitializeComponent();
        }

        private void frmGestionUsuarios_Load(object sender, EventArgs e)
        {
            txtApellido.Text = "Apellido";
            txtApellido.ForeColor = Color.Gray;
            txtApellido.Enter += txtApellido_Enter;
            txtApellido.Leave += txtApellido_Leave;
            ConfigurarPlaceholderPassword(txtPassword, "Contraseña");
            ConfigurarPlaceholders();
            cmbRol.Items.AddRange(new[] { "Usuario", "Admin" });
            cmbRol.SelectedIndex = 0;

            CargarUsuarios();

            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
        }

        private void ConfigurarPlaceholders()
        {
            ConfigurarPlaceholder(txtNombre, "Nombre");
            ConfigurarPlaceholder(txtApellido, "Apellido");
            ConfigurarPlaceholder(txtCorreo, "Correo");
        }

        private void ConfigurarPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;
            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }

        private void CargarUsuarios()
        {
            listaUsuarios.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario u = new Usuario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Rol = reader["Rol"].ToString()
                            };
                            listaUsuarios.Add(u);
                        }
                    }
                }
            }

            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = listaUsuarios.Select(u => new
            {
                u.Id,
                u.Nombre,
                u.Apellido,
                u.Correo,
                u.Rol
            }).ToList();
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                Usuario nuevo = new Usuario
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Correo = txtCorreo.Text,
                    Rol = cmbRol.SelectedItem.ToString()
                };

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CrearUsuario", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", nuevo.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", txtApellido.Text);
                    cmd.Parameters.AddWithValue("@email", nuevo.Correo);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@role_id", nuevo.Rol == "Admin" ? 1 : 2);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario agregado exitosamente.");
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message);
            }
        }

        private bool ValidarCampos()
        {
            if (txtNombre.ForeColor == Color.Gray || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingresa un nombre válido.");
                return false;
            }
            if (txtPassword.ForeColor == Color.Gray || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingresa una contraseña válida.");
                return false;
            }
            if (txtApellido.ForeColor == Color.Gray || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Ingresa un apellido válido.");
                return false;
            }
            if (txtCorreo.ForeColor == Color.Gray || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Ingresa un correo válido.");
                return false;
            }
            if (!txtCorreo.Text.Contains("@"))
            {
                MessageBox.Show("Ingresa un correo electrónico válido.");
                return false;
            }
            return true;
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvUsuarios.Rows[e.RowIndex];

                usuarioSeleccionadoId = Convert.ToInt32(row.Cells["Id"].Value);

                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtNombre.ForeColor = Color.Black;

                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                txtApellido.ForeColor = Color.Black;

                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
                txtCorreo.ForeColor = Color.Black;

                cmbRol.SelectedItem = row.Cells["Rol"].Value.ToString();
            }
        }
        private void txtApellido_Enter(object sender, EventArgs e)
        {
            if (txtApellido.Text == "Apellido")
            {
                txtApellido.Text = "";
                txtApellido.ForeColor = Color.Black;
            }
        }

        private void txtApellido_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                txtApellido.Text = "Apellido";
                txtApellido.ForeColor = Color.Gray;
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["Id"].Value);
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            int rolId = (cmbRol.Text == "Admin") ? 1 : 2;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(correo))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@rol", rolId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarUsuarios();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["Id"].Value);

                var confirmResult = MessageBox.Show("¿Estás seguro de eliminar el usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", conn);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Usuario eliminado.");
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar usuario: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para eliminar.");
            }
        }
        private void ConfigurarPlaceholderPassword(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;
            txt.UseSystemPasswordChar = false;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                    txt.UseSystemPasswordChar = true;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.UseSystemPasswordChar = false;
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }
    }
}
