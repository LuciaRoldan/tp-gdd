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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Tarjeta = new Tarjeta();
            this.Tarjeta.NumeroDeTarjeta = Int32.Parse(this.textBoxNumero.Text);
            this.Tarjeta.Titular = this.textBoxTitular.Text;
            //Agregar el medio de pago a la base

            MessageBox.Show("El nuevo medio de pago se ha ingresado al sistema exitosamente.", "Medio de Pago", MessageBoxButtons.OK);
        }
    }
}
