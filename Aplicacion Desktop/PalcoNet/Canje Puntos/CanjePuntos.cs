using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Canje_Puntos
{
    public partial class CanjePuntos : Form
    {
        public CanjePuntos()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se cambien los puntos
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
        }
    }
}
