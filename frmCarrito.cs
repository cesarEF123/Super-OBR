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
    public partial class frmCarrito : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";
        private int carritoIdActual;
        public frmCarrito()
        {
            InitializeComponent();
        }

        private void frmCarrito_Load(object sender, EventArgs e)
        {
            CargarCarrito();
        }

        private void CargarCarrito()
        {
            if (Sesion.Usuario == null)
            {
                MessageBox.Show("No hay usuario logueado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            carritoIdActual = ObtenerOCrearCarritoActivo(Sesion.Usuario.Id);
            List<Producto> productos = ObtenerProductosDelCarrito(carritoIdActual);

            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = productos.Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Categoria,
                Precio = $"${p.Precio}",
                p.Stock
            }).ToList();

            decimal total = productos.Sum(p => p.Precio);
            lblTotal.Text = $"Total: ${total}";
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
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productoId = Convert.ToInt32(dgvCarrito.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProductoDelCarrito", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@carrito_id", carritoIdActual);
                    cmd.Parameters.AddWithValue("@producto_id", productoId);
                    cmd.ExecuteNonQuery();
                }
            }

            CargarCarrito();
            MessageBox.Show("Producto eliminado del carrito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.Rows.Count == 0)
            {
                MessageBox.Show("Tu carrito está vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_FinalizarCarrito", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@carrito_id", carritoIdActual);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Pedido confirmado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarCarrito();
        }
        private List<Producto> ObtenerProductosDelCarrito(int carritoId)
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProductosDelCarrito", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@carrito_id", carritoId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto p = new Producto
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nombre = reader["nombre"].ToString(),
                                Categoria = reader["categoria"].ToString(),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                Stock = Convert.ToInt32(reader["stock"])
                            };
                            lista.Add(p);
                        }
                    }
                }
            }

            return lista;
        }
    }
}
