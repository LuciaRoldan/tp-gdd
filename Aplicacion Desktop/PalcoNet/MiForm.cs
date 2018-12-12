using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet
{
    public partial class MiForm : Form
    {
        MiForm anterior;

        public MiForm Anterior
        {
            get { return anterior; }
            set { anterior = value; }
        }

        public MiForm(MiForm formAnterior)
        {
            this.anterior = formAnterior;
           //InitializeComponent();
        }

        public MiForm() { }

        public void cerrarAnteriores()
        {
            //Cierra todas las pantallas anteriores hasta llegar al home (SeleccionarFuncionalidad) y si no encuentra nada abre el login
            this.Hide();
            if ((this.anterior is SeleccionarFuncionalidad)) { anterior.ShowDialog(); }            
            else if (this.anterior == null) { new LogIn().Show(); }
            else { anterior.cerrarAnteriores(); }
            this.Close();
        }
    }
}
