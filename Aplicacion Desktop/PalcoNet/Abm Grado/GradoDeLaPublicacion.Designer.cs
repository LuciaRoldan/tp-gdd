namespace PalcoNet.Abm_Grado
{
    partial class GradoDeLaPublicacion
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
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBoxPublicaciones = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxGrado = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAceptar = new System.Windows.Forms.Button();
            this.buttonVolver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "Grado de mis publicaciones";
            // 
            // checkedListBoxPublicaciones
            // 
            this.checkedListBoxPublicaciones.CheckOnClick = true;
            this.checkedListBoxPublicaciones.FormattingEnabled = true;
            this.checkedListBoxPublicaciones.Location = new System.Drawing.Point(16, 62);
            this.checkedListBoxPublicaciones.Name = "checkedListBoxPublicaciones";
            this.checkedListBoxPublicaciones.Size = new System.Drawing.Size(221, 124);
            this.checkedListBoxPublicaciones.TabIndex = 15;
            this.checkedListBoxPublicaciones.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxPublicaciones_SelectedIndexChanged);
            // 
            // checkedListBoxGrado
            // 
            this.checkedListBoxGrado.CheckOnClick = true;
            this.checkedListBoxGrado.Enabled = false;
            this.checkedListBoxGrado.FormattingEnabled = true;
            this.checkedListBoxGrado.Items.AddRange(new object[] {
            "Alto",
            "Medio",
            "Bajo"});
            this.checkedListBoxGrado.Location = new System.Drawing.Point(269, 62);
            this.checkedListBoxGrado.Name = "checkedListBoxGrado";
            this.checkedListBoxGrado.Size = new System.Drawing.Size(100, 49);
            this.checkedListBoxGrado.TabIndex = 16;
            this.checkedListBoxGrado.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxGrado_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Publicacion:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Grado:";
            // 
            // buttonAceptar
            // 
            this.buttonAceptar.Location = new System.Drawing.Point(269, 127);
            this.buttonAceptar.Name = "buttonAceptar";
            this.buttonAceptar.Size = new System.Drawing.Size(100, 23);
            this.buttonAceptar.TabIndex = 46;
            this.buttonAceptar.Text = "Aceptar cambios";
            this.buttonAceptar.UseVisualStyleBackColor = true;
            this.buttonAceptar.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonVolver
            // 
            this.buttonVolver.Location = new System.Drawing.Point(16, 194);
            this.buttonVolver.Name = "buttonVolver";
            this.buttonVolver.Size = new System.Drawing.Size(75, 23);
            this.buttonVolver.TabIndex = 45;
            this.buttonVolver.Text = "Inicio";
            this.buttonVolver.UseVisualStyleBackColor = true;
            this.buttonVolver.Click += new System.EventHandler(this.button3_Click);
            // 
            // GradoDeLaPublicacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 229);
            this.Controls.Add(this.buttonAceptar);
            this.Controls.Add(this.buttonVolver);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxGrado);
            this.Controls.Add(this.checkedListBoxPublicaciones);
            this.Controls.Add(this.label2);
            this.Name = "GradoDeLaPublicacion";
            this.Text = "Grado de la publicacion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBoxPublicaciones;
        private System.Windows.Forms.CheckedListBox checkedListBoxGrado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAceptar;
        private System.Windows.Forms.Button buttonVolver;
    }
}