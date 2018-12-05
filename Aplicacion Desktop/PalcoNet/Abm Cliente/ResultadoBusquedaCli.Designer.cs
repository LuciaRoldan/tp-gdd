namespace PalcoNet.Abm_Cliente
{
    partial class ResultadoBusquedaCli
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridResultados = new System.Windows.Forms.DataGridView();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Apellido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CUIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Telefono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nro_documento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoDocumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Calle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Piso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ciudad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Localidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contraseña = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaNa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puntos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonModificar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultados)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(573, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Nueva Búsqueda";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 279);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 28;
            this.button3.Text = "Inicio";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 24);
            this.label2.TabIndex = 27;
            this.label2.Text = "Resultados";
            // 
            // dataGridResultados
            // 
            this.dataGridResultados.AllowUserToAddRows = false;
            this.dataGridResultados.AllowUserToDeleteRows = false;
            this.dataGridResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResultados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nombre,
            this.Apellido,
            this.CUIL,
            this.Email,
            this.Telefono,
            this.Nro_documento,
            this.TipoDocumento,
            this.Calle,
            this.Nro,
            this.Piso,
            this.Ciudad,
            this.CP,
            this.Localidad,
            this.Usuario,
            this.Contraseña,
            this.FechaNa,
            this.FechaC,
            this.puntos});
            this.dataGridResultados.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridResultados.Location = new System.Drawing.Point(12, 63);
            this.dataGridResultados.MultiSelect = false;
            this.dataGridResultados.Name = "dataGridResultados";
            this.dataGridResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridResultados.ShowEditingIcon = false;
            this.dataGridResultados.Size = new System.Drawing.Size(661, 201);
            this.dataGridResultados.TabIndex = 26;
            this.dataGridResultados.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Nombre
            // 
            this.Nombre.DataPropertyName = "nombre";
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.Width = 69;
            // 
            // Apellido
            // 
            this.Apellido.DataPropertyName = "apellido";
            this.Apellido.HeaderText = "Apellido";
            this.Apellido.Name = "Apellido";
            this.Apellido.Width = 69;
            // 
            // CUIL
            // 
            this.CUIL.DataPropertyName = "cuil";
            this.CUIL.HeaderText = "CUIL";
            this.CUIL.Name = "CUIL";
            this.CUIL.Width = 56;
            // 
            // Email
            // 
            this.Email.DataPropertyName = "mail";
            this.Email.HeaderText = "Email";
            this.Email.Name = "Email";
            this.Email.Width = 57;
            // 
            // Telefono
            // 
            this.Telefono.DataPropertyName = "telefono";
            this.Telefono.HeaderText = "Telefono";
            this.Telefono.Name = "Telefono";
            this.Telefono.Width = 74;
            // 
            // Nro_documento
            // 
            this.Nro_documento.DataPropertyName = "numeroDeDocumento";
            this.Nro_documento.HeaderText = "Nro. documento";
            this.Nro_documento.Name = "Nro_documento";
            this.Nro_documento.Width = 99;
            // 
            // TipoDocumento
            // 
            this.TipoDocumento.DataPropertyName = "tipoDocumento";
            this.TipoDocumento.HeaderText = "Tipo documento";
            this.TipoDocumento.Name = "TipoDocumento";
            // 
            // Calle
            // 
            this.Calle.DataPropertyName = "calle";
            this.Calle.HeaderText = "Calle";
            this.Calle.Name = "Calle";
            this.Calle.Width = 55;
            // 
            // Nro
            // 
            this.Nro.DataPropertyName = "NumeroDeCalle";
            this.Nro.HeaderText = "Nro. Calle";
            this.Nro.Name = "Nro";
            this.Nro.Width = 72;
            // 
            // Piso
            // 
            this.Piso.DataPropertyName = "piso";
            this.Piso.HeaderText = "Piso";
            this.Piso.Name = "Piso";
            this.Piso.Width = 52;
            // 
            // Ciudad
            // 
            this.Ciudad.DataPropertyName = "ciudad";
            this.Ciudad.HeaderText = "Ciudad";
            this.Ciudad.Name = "Ciudad";
            this.Ciudad.Width = 65;
            // 
            // CP
            // 
            this.CP.DataPropertyName = "codigoPostal";
            this.CP.HeaderText = "CP";
            this.CP.Name = "CP";
            this.CP.Width = 46;
            // 
            // Localidad
            // 
            this.Localidad.DataPropertyName = "localidad";
            this.Localidad.HeaderText = "Localidad";
            this.Localidad.Name = "Localidad";
            this.Localidad.Width = 78;
            // 
            // Usuario
            // 
            this.Usuario.DataPropertyName = "nombreUsuario";
            this.Usuario.HeaderText = "Usuario";
            this.Usuario.Name = "Usuario";
            this.Usuario.Visible = false;
            this.Usuario.Width = 68;
            // 
            // Contraseña
            // 
            this.Contraseña.DataPropertyName = "contrasenia";
            this.Contraseña.HeaderText = "Contraseña";
            this.Contraseña.Name = "Contraseña";
            this.Contraseña.Visible = false;
            this.Contraseña.Width = 86;
            // 
            // FechaNa
            // 
            this.FechaNa.DataPropertyName = "FechaDeNacimiento";
            this.FechaNa.HeaderText = "Fecha de nacimiento";
            this.FechaNa.Name = "FechaNa";
            this.FechaNa.Width = 120;
            // 
            // FechaC
            // 
            this.FechaC.DataPropertyName = "FechaDeCreacion";
            this.FechaC.HeaderText = "Fecha de creacion";
            this.FechaC.Name = "FechaC";
            this.FechaC.Width = 111;
            // 
            // puntos
            // 
            this.puntos.DataPropertyName = "puntos";
            this.puntos.HeaderText = "Puntos";
            this.puntos.Name = "puntos";
            this.puntos.Width = 65;
            // 
            // buttonModificar
            // 
            this.buttonModificar.Location = new System.Drawing.Point(283, 279);
            this.buttonModificar.Name = "buttonModificar";
            this.buttonModificar.Size = new System.Drawing.Size(92, 23);
            this.buttonModificar.TabIndex = 30;
            this.buttonModificar.Text = "Editar cliente";
            this.buttonModificar.UseVisualStyleBackColor = true;
            this.buttonModificar.Click += new System.EventHandler(this.buttonModificar_Click);
            // 
            // ResultadoBusquedaCli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 316);
            this.Controls.Add(this.buttonModificar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridResultados);
            this.Name = "ResultadoBusquedaCli";
            this.Text = "Resultado Busqueda";
            this.Load += new System.EventHandler(this.ResultadoBusquedaCli_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResultados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridResultados;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Apellido;
        private System.Windows.Forms.DataGridViewTextBoxColumn CUIL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Telefono;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nro_documento;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoDocumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Calle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nro;
        private System.Windows.Forms.DataGridViewTextBoxColumn Piso;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ciudad;
        private System.Windows.Forms.DataGridViewTextBoxColumn CP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Localidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contraseña;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaNa;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaC;
        private System.Windows.Forms.DataGridViewTextBoxColumn puntos;
        private System.Windows.Forms.Button buttonModificar;
    }
}