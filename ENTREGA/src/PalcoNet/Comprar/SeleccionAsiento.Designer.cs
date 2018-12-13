namespace PalcoNet.Comprar
{
    partial class SeleccionAsiento
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
            this.comboBoxFila = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxAsiento = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxFila
            // 
            this.comboBoxFila.FormattingEnabled = true;
            this.comboBoxFila.Location = new System.Drawing.Point(45, 41);
            this.comboBoxFila.Name = "comboBoxFila";
            this.comboBoxFila.Size = new System.Drawing.Size(64, 21);
            this.comboBoxFila.TabIndex = 124;
            this.comboBoxFila.SelectedIndexChanged += new System.EventHandler(this.comboBoxFila_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 123;
            this.label4.Text = "Fila:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(163, 68);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 122;
            this.button2.Text = "Aceptar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 24);
            this.label2.TabIndex = 121;
            this.label2.Text = "Seleccionar asiento";
            // 
            // comboBoxAsiento
            // 
            this.comboBoxAsiento.FormattingEnabled = true;
            this.comboBoxAsiento.Location = new System.Drawing.Point(174, 41);
            this.comboBoxAsiento.Name = "comboBoxAsiento";
            this.comboBoxAsiento.Size = new System.Drawing.Size(64, 21);
            this.comboBoxAsiento.TabIndex = 126;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 125;
            this.label1.Text = "Asiento:";
            // 
            // SeleccionAsiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 108);
            this.ControlBox = false;
            this.Controls.Add(this.comboBoxAsiento);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxFila);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Name = "SeleccionAsiento";
            this.Text = "SeleccionAsiento";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFila;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxAsiento;
        private System.Windows.Forms.Label label1;
    }
}