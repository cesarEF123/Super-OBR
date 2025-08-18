namespace Proyecto_Super_OBR
{
    partial class frmCarrito
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvCarrito;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnConfirmar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvCarrito = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            this.SuspendLayout();

            
            this.dgvCarrito.Location = new System.Drawing.Point(30, 30);
            this.dgvCarrito.Size = new System.Drawing.Size(600, 250);
            this.dgvCarrito.ReadOnly = true;
            this.dgvCarrito.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarrito.MultiSelect = false;

            
            this.lblTotal.Location = new System.Drawing.Point(30, 290);
            this.lblTotal.Size = new System.Drawing.Size(300, 25);
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            
            this.btnEliminar.Text = "Eliminar del carrito";
            this.btnEliminar.Location = new System.Drawing.Point(30, 330);
            this.btnEliminar.Size = new System.Drawing.Size(150, 35);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            
            this.btnConfirmar.Text = "Confirmar Pedido";
            this.btnConfirmar.Location = new System.Drawing.Point(200, 330);
            this.btnConfirmar.Size = new System.Drawing.Size(150, 35);
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);

            
            this.ClientSize = new System.Drawing.Size(680, 400);
            this.Controls.Add(this.dgvCarrito);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnConfirmar);
            this.Name = "frmCarrito";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carrito de Compras";
            this.Load += new System.EventHandler(this.frmCarrito_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.ResumeLayout(false);
        }
    }

    }