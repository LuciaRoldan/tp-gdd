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
            string errores = "";
            int cuit;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBox1.Text)
                || !string.IsNullOrWhiteSpace(textBox2.Text)
                || !string.IsNullOrWhiteSpace(textBox4.Text);

            if (!camposCompletos) {
                errores += "Se debe completar al menos un campo para realizar la búsqueda.";
            } else {
                if (!int.TryParse(textBox1.Text, out cuit)) { errores += "El CUIT debe ser un valor numérico"; }
            }

            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que con los datos se busquen las empresas y se pase una lista abajo
            if (this.verificarCampos())
            {
                Empresa empresa = new Empresa();
                empresa.Cuit = Int32.Parse(textBox1.Text);
                empresa.Mail = textBox2.Text;
                empresa.RazonSocial = textBox4.Text;
                List<Empresa> resultados = new List<Empresa>();

                //Aca hay que buscar en la base
                //Verificar que la lista no este vacia

                var bindingList = new BindingList<Empresa>(resultados);
                var source = new BindingSource(bindingList, null);
                dataGridViewResultados.DataSource = source;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            Empresa cliente = new Empresa();
            new RegistroDeEmpresa(cliente, this).Show();
            this.Hide(); 
        }

        private void BusquedaEmp_Load(object sender, EventArgs e)
        {

        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            Empresa empresa = (Empresa)dataGridViewResultados.CurrentRow.DataBoundItem;
            this.Hide();
            new ModificarEmp(empresa, this).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }
    }
}
