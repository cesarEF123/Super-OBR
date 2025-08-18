using System.Windows.Forms;

namespace Proyecto_Super_OBR
{
    partial class frmCatalogoProductos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvCatalogo;
        private System.Windows.Forms.ComboBox cmbCategorias;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnAgregar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvCatalogo = new System.Windows.Forms.DataGridView();
            this.cmbCategorias = new System.Windows.Forms.ComboBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalogo)).BeginInit();
            this.SuspendLayout();

            
            this.dgvCatalogo.Location = new System.Drawing.Point(30, 60);
            this.dgvCatalogo.Size = new System.Drawing.Size(620, 250);
            this.dgvCatalogo.ReadOnly = true;
            this.dgvCatalogo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCatalogo.MultiSelect = false;

            
            this.cmbCategorias.Location = new System.Drawing.Point(30, 20);
            this.cmbCategorias.Size = new System.Drawing.Size(150, 23);
            this.cmbCategorias.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategorias.SelectedIndexChanged += new System.EventHandler(this.cmbCategorias_SelectedIndexChanged);

            
            this.txtBuscar.Location = new System.Drawing.Point(200, 20);
            this.txtBuscar.Size = new System.Drawing.Size(200, 23);

            
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Location = new System.Drawing.Point(410, 20);
            this.btnBuscar.Size = new System.Drawing.Size(80, 23);
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            
            this.btnAgregar.Text = "Agregar al Carrito";
            this.btnAgregar.Location = new System.Drawing.Point(30, 330);
            this.btnAgregar.Size = new System.Drawing.Size(150, 35);
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            
            this.ClientSize = new System.Drawing.Size(700, 400);
            this.Controls.Add(this.dgvCatalogo);
            this.Controls.Add(this.cmbCategorias);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnAgregar);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Catálogo de Productos";
            this.Load += new System.EventHandler(this.frmCatalogoProductos_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}