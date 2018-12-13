namespace PalcoNet.Generar_Publicacion
{
    partial class Finalizar_publicacion
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFechas = new System.Windows.Forms.TextBox();
            this.textBoxUbicaciones = new System.Windows.Forms.TextBox();
            this.textBoxAsientos = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 24);
            this.label2.TabIndex = 85;
            this.label2.Text = "Finalizar publicación";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(195, 195);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 88;
            this.button2.Text = "Aceptar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 87;
            this.button1.Text = "Cancelar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 89;
            this.label1.Text = "Descripcion:";
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(85, 55);
            this.textBoxDescripcion.Multiline = true;
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.ReadOnly = true;
            this.textBoxDescripcion.Size = new System.Drawing.Size(185, 41);
            this.textBoxDescripcion.TabIndex = 90;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 91;
            this.label3.Text = "Cantidad de fechas:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 92;
            this.label4.Text = "Cantidad de asientos total:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 93;
            this.label5.Text = "Cantidad de ubicaciones distintas:";
            // 
            // textBoxFechas
            // 
            this.textBoxFechas.Location = new System.Drawing.Point(186, 103);
            this.textBoxFechas.Name = "textBoxFechas";
            this.textBoxFechas.ReadOnly = true;
            this.textBoxFechas.Size = new System.Drawing.Size(83, 20);
            this.textBoxFechas.TabIndex = 94;
            // 
            // textBoxUbicaciones
            // 
            this.textBoxUbicaciones.Location = new System.Drawing.Point(186, 129);
            this.textBoxUbicaciones.Name = "textBoxUbicaciones";
            this.textBoxUbicaciones.ReadOnly = true;
            this.textBoxUbicaciones.Size = new System.Drawing.Size(83, 20);
            this.textBoxUbicaciones.TabIndex = 95;
            // 
            // textBoxAsientos
            // 
            this.textBoxAsientos.Location = new System.Drawing.Point(186, 155);
            this.textBoxAsientos.Name = "textBoxAsientos";
            this.textBoxAsientos.ReadOnly = true;
            this.textBoxAsientos.Size = new System.Drawing.Size(83, 20);
            this.textBoxAsientos.TabIndex = 96;
            // 
            // Finalizar_publicacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 230);
            this.Controls.Add(this.textBoxAsientos);
            this.Controls.Add(this.textBoxUbicaciones);
            this.Controls.Add(this.textBoxFechas);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Name = "Finalizar_publicacion";
            this.Text = "Finalizar Publicación";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxFechas;
        private System.Windows.Forms.TextBox textBoxUbicaciones;
        private System.Windows.Forms.TextBox textBoxAsientos;
    }
}