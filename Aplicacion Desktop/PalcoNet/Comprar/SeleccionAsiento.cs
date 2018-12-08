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
    public partial class SeleccionAsiento : Form
    {
        Ubicacion ubicacion;
        Asiento asiento;
        Compra compra;

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        internal Asiento Asiento
        {
            get { return asiento; }
            set { asiento = value; }
        }

        public Ubicacion Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; }
        }


        public SeleccionAsiento(Ubicacion ubicacion, Compra compra)
        {
            InitializeComponent();

            List<int> filas = new List<int>();
            //Aca hay que buscar en la base todas las filas que tinen asientos disponibles para la publicacion seleccionada

            foreach (int f in filas) {
                comboBoxFila.Items.Add(f);
            }
        }

        private void comboBoxFila_SelectedIndexChanged(object sender, EventArgs e)
        {
            int filaSeleccionada = (int)comboBoxFila.SelectedItem;
            List<int> asientos = new List<int>();
            //Aca hay que buscar en la base todas os asientos disponibles para la fila seleccionada de la publicacion seleccionada

            foreach (int a in asientos)
            {
                comboBoxAsiento.Items.Add(a);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxAsiento.SelectedIndex > -1 && comboBoxFila.SelectedIndex > -1){
                this.Asiento.Fila = (int)comboBoxFila.SelectedItem;
                this.Asiento.Asiento1 = (int)comboBoxAsiento.SelectedItem;
                this.Hide();
            } else {
                MessageBox.Show("Se debe seleccionar un asiento", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
