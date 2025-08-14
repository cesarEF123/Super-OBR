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
    public partial class frmPrincipalAdmin : Form
    {
        public frmPrincipalAdmin()
        {
            InitializeComponent();
        }

        private void frmPrincipalAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin login = new frmLogin();
            login.Show();
        }

        private void gestiónDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionUsuarios usuariosForm = new frmGestionUsuarios();
            usuariosForm.ShowDialog();
        }

        private void gestiónDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionProductos productosForm = new frmGestionProductos();
            productosForm.ShowDialog();
        }

        private void gestiónDeCategoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionCategorias categoriasForm = new frmGestionCategorias();
            categoriasForm.ShowDialog();
        }

        private void verPedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionPedidos pedidosForm = new frmGestionPedidos();
            pedidosForm.ShowDialog();
        }
    }
}