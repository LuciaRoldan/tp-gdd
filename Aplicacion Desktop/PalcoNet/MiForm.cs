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
            this.Hide();
            if ((this.anterior is SeleccionarFuncionalidad)) { anterior.ShowDialog(); }
            //if (this.anterior == null) { new LogIn().Show(); }
            else { anterior.cerrarAnteriores(); }
            this.Close();
        }

        /*private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (string.Equals((sender as Button).Name, @"CloseButton"))
            {
                this.cerrarAnteriores();
            }
            else {
                this.cerrarAnteriores();
            }
        }*/  //Para que ce cierren las escondidas cuando se toca X
    }
}
