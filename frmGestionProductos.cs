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
    public partial class frmGestionProductos : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";
        private List<Producto> productos = new List<Producto>();
        private List<string> categorias = new List<string> { "Fruta", "Lácteos", "Panadería" };

        public frmGestionProductos()
        {
            InitializeComponent();
        }

        private void frmGestionProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
            CargarCategorias();
        }
        private void CargarProductos()
        {
            productos.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProductos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto p = new Producto
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombre"].ToString(),
                                Categoria = reader["nombre_categoria"].ToString(),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                Stock = Convert.ToInt32(reader["stock"])
                            };
                            productos.Add(p);
                        }
                    }
                }
            }

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Categoria,
                Precio = "$" + p.Precio,
                p.Stock
            }).ToList();
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Completa todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CrearProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", "");
                    cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(txtStock.Text));
                    cmd.Parameters.AddWithValue("@categoria_id", ObtenerIdCategoria(cmbCategoria.SelectedItem.ToString()));
                    cmd.ExecuteNonQuery();
                }
            }

            LimpiarCampos();
            CargarProductos();
            MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["Id"].Value);

            DialogResult result = MessageBox.Show("¿Estás seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_EliminarProducto", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                CargarProductos();
                MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                            categorias.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }

            cmbCategoria.Items.Clear();
            cmbCategoria.Items.AddRange(categorias.ToArray());
            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;
        }
        private int ObtenerIdCategoria(string nombreCategoria)
        {
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
                            if (reader["nombre"].ToString() == nombreCategoria)
                            {
                                return Convert.ToInt32(reader["id"]);
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            if (cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedIndex = 0;
        }
        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
