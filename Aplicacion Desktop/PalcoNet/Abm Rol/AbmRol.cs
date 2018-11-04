using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Abm_Rol
{
    public partial class AbmRol : Form
    {
        public AbmRol()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se cree el rol y se actualicen las tablas
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se guarden los cambios en las tablas
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
        }
    }
}
