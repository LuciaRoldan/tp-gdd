using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Comprar
{
    public partial class Ubicaciones : Form
    {
        public Ubicaciones()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //new Seleccion().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new MedioPago().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que ir gurdando una lista de todas las entradas
            //Tambien podria salir un cartelito de que las cosas salieron bien
        }
    }
}
