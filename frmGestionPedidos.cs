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
    public partial class frmGestionPedidos : Form
    {
        private string connectionString = "workstation id=SuperOBR_DB_POO.mssql.somee.com;packet size=4096;user id=The_Unkn0wn;pwd=ElW3ird0_;data source=SuperOBR_DB_POO.mssql.somee.com;persist security info=False;initial catalog=SuperOBR_DB_POO;TrustServerCertificate=True";

        public frmGestionPedidos()
        {
            InitializeComponent();
        }

        private void frmGestionPedidos_Load(object sender, EventArgs e)
        {
            cmbEstados.SelectedIndex = 0;

            txtBuscarUsuario.Text = "Buscar por usuario...";
            txtBuscarUsuario.ForeColor = Color.Gray;

            txtBuscarUsuario.Enter += txtBuscarUsuario_Enter;
            txtBuscarUsuario.Leave += txtBuscarUsuario_Leave;

            CargarTodosLosPedidos();
        }

        private void CargarTodosLosPedidos()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerTodosLosPedidos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<dynamic> pedidos = new List<dynamic>();
                        while (reader.Read())
                        {
                            pedidos.Add(new
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Fecha = Convert.ToDateTime(reader["fecha_pedido"]),
                                Total = Convert.ToDecimal(reader["total"]),
                                Estado = reader["estado"].ToString(),
                                Usuario = reader["usuario"].ToString()
                            });
                        }
                        dgvPedidos.DataSource = pedidos;
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string estado = cmbEstados.SelectedItem.ToString();
            string usuario = txtBuscarUsuario.Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd;

                if (!string.IsNullOrWhiteSpace(usuario) && usuario != "Buscar por usuario...")
                {
                    cmd = new SqlCommand("sp_BuscarPedidosPorUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", usuario);
                }
                else if (estado != "Todos")
                {
                    cmd = new SqlCommand("sp_BuscarPedidosPorEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@estado", estado.ToUpper());
                }
                else
                {
                    CargarTodosLosPedidos();
                    return;
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<dynamic> pedidos = new List<dynamic>();
                    while (reader.Read())
                    {
                        pedidos.Add(new
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Fecha = Convert.ToDateTime(reader["fecha_pedido"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            Estado = reader["estado"].ToString(),
                            Usuario = reader["usuario"].ToString()
                        });
                    }
                    dgvPedidos.DataSource = pedidos;
                }
            }
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un pedido primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pedidoId = Convert.ToInt32(dgvPedidos.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerDetallesPedido_Admin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pedido_id", pedidoId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        string detalle = "";
                        while (reader.Read())
                        {
                            string nombre = reader["nombre"].ToString();
                            int cantidad = Convert.ToInt32(reader["cantidad"]);
                            decimal precio = Convert.ToDecimal(reader["precio_unitario"]);
                            detalle += $"{nombre} - Cantidad: {cantidad}, Precio: ${precio}\n";
                        }

                        MessageBox.Show(detalle, "Detalle del Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un pedido primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pedidoId = Convert.ToInt32(dgvPedidos.SelectedRows[0].Cells["Id"].Value);
            string estadoActual = dgvPedidos.SelectedRows[0].Cells["Estado"].Value.ToString();

            string nuevoEstado = Microsoft.VisualBasic.Interaction.InputBox($"Estado actual: {estadoActual}\nNuevo estado (PENDIENTE, PROCESADO o CANCELADO):", "Cambiar Estado");

            if (string.IsNullOrWhiteSpace(nuevoEstado)) return;

            nuevoEstado = nuevoEstado.ToUpper();

            if (nuevoEstado != "PENDIENTE" && nuevoEstado != "PROCESADO" && nuevoEstado != "CANCELADO")
            {
                MessageBox.Show("Estado inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CambiarEstadoPedido", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pedido_id", pedidoId);
                    cmd.Parameters.AddWithValue("@nuevo_estado", nuevoEstado);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Estado actualizado correctamente.");
            CargarTodosLosPedidos();
        }

        private void btnEliminarPedido_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un pedido primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pedidoId = Convert.ToInt32(dgvPedidos.SelectedRows[0].Cells["Id"].Value);

            DialogResult result = MessageBox.Show("¿Estás seguro de eliminar este pedido?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_EliminarPedido", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pedido_id", pedidoId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Pedido eliminado correctamente.");
                CargarTodosLosPedidos();
            }
        }

        private void txtBuscarUsuario_Enter(object sender, EventArgs e)
        {
            if (txtBuscarUsuario.Text == "Buscar por usuario...")
            {
                txtBuscarUsuario.Text = "";
                txtBuscarUsuario.ForeColor = Color.Black;
            }
        }

        private void txtBuscarUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarUsuario.Text))
            {
                txtBuscarUsuario.Text = "Buscar por usuario...";
                txtBuscarUsuario.ForeColor = Color.Gray;
            }
        }
    }
}