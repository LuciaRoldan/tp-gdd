using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Comprar
{
    public partial class NuevoMP : Form
    {
        Tarjeta tarjeta;

        internal Tarjeta Tarjeta
        {
            get { return tarjeta; }
            set { tarjeta = value; }
        }

        public NuevoMP()
        {
            InitializeComponent();
        }
        public bool verificarCampos() {
            string errores = "";
            int dni;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNumero.Text)
                && !string.IsNullOrWhiteSpace(textBoxTitular.Text);

            if (!camposCompletos)
            {
                errores += "Se debe completar al menos un campo para realizar la búsqueda.";
            }
            else
            {
                if (!int.TryParse(textBoxNumero.Text, out dni)) { errores += "El Número de Tarjeta debe ser un valor numérico."; }
            }

            if (errores != "")
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos()){
                this.Tarjeta = new Tarjeta();
                this.Tarjeta.NumeroDeTarjeta = Int32.Parse(this.textBoxNumero.Text);
                this.Tarjeta.Titular = this.textBoxTitular.Text;
                //Agregar el medio de pago a la base

                MessageBox.Show("El nuevo medio de pago se ha ingresado al sistema exitosamente.", "Medio de Pago", MessageBoxButtons.OK);
            }
        }
    }
}
