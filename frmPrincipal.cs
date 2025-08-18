using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Super_OBR
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void catálogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCatalogoProductos catalogo = new frmCatalogoProductos();
            catalogo.ShowDialog();
        }

        private void carritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCarrito carrito = new frmCarrito();
            carrito.ShowDialog();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPedidos pedidos = new frmPedidos();
            pedidos.ShowDialog();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin login = new frmLogin();
            login.Show();
        }

        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
