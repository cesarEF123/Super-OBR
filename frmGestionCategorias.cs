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
    public partial class frmGestionCategorias : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";

        private List<Categoria> categorias = new List<Categoria>();

        public frmGestionCategorias()
        {
            InitializeComponent();
        }

        private void frmGestionCategorias_Load(object sender, EventArgs e)
        {

            CargarCategorias();
        }

        private void CargarCategorias()
        {
            categorias.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerCategorias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categoria cat = new Categoria
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombre"].ToString()
                            };
                            categorias.Add(cat);
                        }
                    }
                }
            }

            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = categorias.Select(c => new { c.Id, c.Nombre }).ToList();
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingresa un nombre para la categoría.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CrearCategoria", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.ExecuteNonQuery();
                }
            }

            txtNombre.Clear();
            CargarCategorias();
            MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una categoría para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvCategorias.SelectedRows[0].Cells["Id"].Value);

            DialogResult result = MessageBox.Show("¿Estás seguro de eliminar esta categoría?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                CargarCategorias();
                MessageBox.Show("Categoría eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
