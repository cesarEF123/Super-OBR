using System.Windows.Forms;
using System;
using System.Drawing;

namespace Proyecto_Super_OBR
{
    partial class frmGestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvUsuarios;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtCorreo;
        private ComboBox cmbRol;
        private Button btnAgregar;
        private Button btnEliminar;
        private Button btnActualizar;
        private System.Windows.Forms.TextBox txtPassword;


        private void InitializeComponent()
        {
            this.dgvUsuarios = new DataGridView();
            this.txtNombre = new TextBox();
            this.txtApellido = new TextBox();
            this.txtCorreo = new TextBox();
            this.cmbRol = new ComboBox();
            this.btnAgregar = new Button();
            this.btnEliminar = new Button();
            this.btnActualizar = new Button();


            this.dgvUsuarios.Location = new System.Drawing.Point(30, 30);
            this.dgvUsuarios.Size = new System.Drawing.Size(580, 200);
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;





            this.txtNombre.Location = new System.Drawing.Point(30, 250);
            this.txtNombre.Size = new System.Drawing.Size(140, 20);
            this.txtNombre.Text = "Nombre";
            this.txtNombre.ForeColor = Color.Gray;


            this.txtApellido.Location = new System.Drawing.Point(180, 250);
            this.txtApellido.Size = new System.Drawing.Size(140, 20);
            this.txtApellido.Text = "Apellido";
            this.txtApellido.ForeColor = Color.Gray;


            this.txtCorreo.Location = new System.Drawing.Point(330, 250);
            this.txtCorreo.Size = new System.Drawing.Size(140, 20);
            this.txtCorreo.Text = "Correo";
            this.txtCorreo.ForeColor = Color.Gray;


            this.cmbRol.Location = new System.Drawing.Point(480, 250);
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Size = new System.Drawing.Size(130, 21);


            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new System.Drawing.Point(30, 290);
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);


            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Location = new System.Drawing.Point(120, 290);
            this.btnActualizar.Click += new EventHandler(this.btnActualizar_Click);


            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new System.Drawing.Point(220, 290);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);


            this.txtPassword = new System.Windows.Forms.TextBox();

            this.txtPassword.Location = new System.Drawing.Point(480, 280); 
            this.txtPassword.Size = new System.Drawing.Size(130, 20);
            this.txtPassword.Text = "Contraseña";
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.UseSystemPasswordChar = false; 
            this.Controls.Add(this.txtPassword);


            this.ClientSize = new System.Drawing.Size(650, 350);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnEliminar);
            this.Text = "Gestión de Usuarios";

            this.Load += new EventHandler(this.frmGestionUsuarios_Load);
        }
    }
}