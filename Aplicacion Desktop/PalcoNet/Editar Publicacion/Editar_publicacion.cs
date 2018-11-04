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
    public partial class Editar_publicacion : Form
    {
        public Editar_publicacion()
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
            //Hace que se completen los campos de abajo con la informacion de la publicacion seleccionada
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Agrega una publicacion a la lista de abajo
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Agrega una fecha a la lista de abajo
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hace los cambios que haya que hacer en la base
            //Podria salir un cartelito que diga que los cambios se guardaron correctamente
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Le tiene que pasar el id de la publicacion para que solo muestre sus ubicaciones
            new Editar_ubicacion_particular().Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Le tiene que pasar el id de la publicacion para que solo muestre sus fechas
            new Editar_fecha_particular().Show();
            this.Hide();
        }
    }
}
