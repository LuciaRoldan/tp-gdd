using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Editar_Publicacion
{
    public partial class Editar_ubicacion_particular : Form
    {
        public Editar_ubicacion_particular()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Hace que aparezcan los datos de la ubicacion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Le pasa el id de la publicacion asi aparece esa
            new Editar_publicacion().Show();
            this.Hide(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hace que se guarde en la base
            //Que salga un cartelito de que salio todo bien
        }
    }
}
