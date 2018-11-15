using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Dominio;

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class BusquedaEmp : MiForm
    {
        public BusquedaEmp(MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
        }

        public bool verificarCampos(){
            return !string.IsNullOrWhiteSpace(textBox1.Text)
                || !string.IsNullOrWhiteSpace(textBox2.Text)
                || !string.IsNullOrWhiteSpace(textBox4.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que con los datos se busquen las empresas y se pase una lista abajo
            if (this.verificarCampos())
            {
                Empresa empresa = new Empresa();
                if (!string.IsNullOrWhiteSpace(textBox1.Text)) { empresa.Cuit = Int32.Parse(textBox1.Text); }
                if (!string.IsNullOrWhiteSpace(textBox2.Text)) { empresa.Mail = textBox2.Text; }
                if (!string.IsNullOrWhiteSpace(textBox4.Text)) { empresa.RazonSocial = textBox4.Text; }
                List<Empresa> resultados = new List<Empresa>();

                //Aca hay que buscar en la base

                new ResultadosBusquedaEmp(resultados, this).Show();
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            Empresa cliente = new Empresa();
            new RegistroDeEmpresa(cliente, this).Show();
            this.Hide(); 
        }
    }
}
