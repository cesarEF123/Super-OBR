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
using Microsoft.VisualBasic;

namespace Proyecto_Super_OBR
{
    public partial class frmPedidos : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";

        public frmPedidos()
        {
            InitializeComponent();
        }

        private void frmPedidos_Load(object sender, EventArgs e)
        {
            CargarPedidos();
        }
        private void CargarPedidos()
        {
            List<Pedido> pedidos = new List<Pedido>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerPedidosPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", Sesion.Usuario.Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pedido p = new Pedido
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Fecha = Convert.ToDateTime(reader["fecha_pedido"]),
                                Estado = reader["estado"].ToString(),
                                Total = Convert.ToDecimal(reader["total"])
                            };

                            pedidos.Add(p);
                        }
                    }
                }
            }

            dgvPedidos.DataSource = null;
            dgvPedidos.DataSource = pedidos.Select(p => new
            {
                p.Id,
                Fecha = p.Fecha.ToString("dd/MM/yyyy HH:mm"),
                p.Estado,
                Total = $"${p.Total}"
            }).ToList();
        }
        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pedidoId = Convert.ToInt32(dgvPedidos.SelectedRows[0].Cells["Id"].Value);
            VerDetallePedido(pedidoId);
        }
        private void VerDetallePedido(int pedidoId)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerDetallesPedido_Usuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pedido_id", pedidoId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto p = new Producto
                            {
                                Id = Convert.ToInt32(reader["producto_id"]),
                                Nombre = reader["nombre"].ToString(),
                                Categoria = reader["categoria"].ToString(),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                Stock = Convert.ToInt32(reader["cantidad"])
                            };

                            productos.Add(p);
                        }
                    }
                }
            }

            string detalle = string.Join(Environment.NewLine,
                productos.Select(p => $"- {p.Nombre} ({p.Categoria}) x{p.Stock} - ${p.Precio}"));

            MessageBox.Show($"Detalles del Pedido:\n\n{detalle}", "Detalle", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
