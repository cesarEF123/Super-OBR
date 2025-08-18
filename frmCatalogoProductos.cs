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

    public partial class frmCatalogoProductos : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";

        private List<Producto> productos = new List<Producto>();

        public frmCatalogoProductos()
        {
            InitializeComponent();
        }

        private void frmCatalogoProductos_Load(object sender, EventArgs e)
        {

            CargarCategorias();
            CargarProductos();
        }
        private void CargarCategorias()
        {
            cmbCategorias.Items.Clear();
            cmbCategorias.Items.Add("Todos");

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
                            string nombre = reader["nombre"].ToString();
                            cmbCategorias.Items.Add(nombre);
                        }
                    }
                }
            }

            cmbCategorias.SelectedIndex = 0;
        }
        private void CargarProductos(string categoria = "Todos", string filtro = "")
        {
            productos.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd;

                if (categoria == "Todos")
                {
                    cmd = new SqlCommand("sp_ObtenerProductos", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd = new SqlCommand("sp_ObtenerProductosPorCategoria", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@categoria", categoria);
                }

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

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                productos = productos.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower())).ToList();
            }

            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = productos.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Categoria,
                Precio = "$" + p.Precio,
                p.Stock
            }).ToList();
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string categoria = cmbCategorias.SelectedItem.ToString();
            string filtro = txtBuscar.Text.Trim();
            CargarProductos(categoria, filtro);
        }
        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            string categoria = cmbCategorias.SelectedItem.ToString();
            CargarProductos(categoria, txtBuscar.Text.Trim());
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvCatalogo.SelectedRows.Count > 0)
            {
                int productoId = Convert.ToInt32(dgvCatalogo.SelectedRows[0].Cells["Id"].Value);
                int usuarioId = Sesion.Usuario.Id;

                
                int carritoId = ObtenerOCrearCarritoActivo(usuarioId);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_AgregarProductoAlCarrito", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@carrito_id", carritoId);
                        cmd.Parameters.AddWithValue("@producto_id", productoId);
                        cmd.Parameters.AddWithValue("@cantidad", 1); 

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Producto agregado al carrito.");
            }
            else
            {
                MessageBox.Show("Selecciona un producto primero.");
            }
        }
        private int ObtenerOCrearCarritoActivo(int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerCarritosPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        return Convert.ToInt32(result);
                }

                using (SqlCommand cmd = new SqlCommand("sp_CrearCarrito", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
