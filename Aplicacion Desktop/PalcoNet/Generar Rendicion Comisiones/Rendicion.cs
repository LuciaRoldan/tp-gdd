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

namespace PalcoNet.Generar_Rendicion_Comisiones
{
    public partial class Rendicion : MiForm
    {

        Empresa empresaSeleccionada;
        List<Empresa> empresas = new List<Empresa>();

        public Empresa EmpresaSeleccionada
        {
            get { return empresaSeleccionada; }
            set { empresaSeleccionada = value; }
        }

        public List<Empresa> Empresas
        {
            get { return empresas; }
            set { empresas = value; }
        }

        public Rendicion(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            //Aca hay que buscar en la base de datos todas las Empresas

            foreach (Empresa e in this.empresas)
            {
                comboBoxEmpresas.Items.Add(e.RazonSocial);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Compra> comprasSeleccionadas = new List<Compra>();
            if (checkedListBox1.CheckedIndices.Count > 0){
                foreach(Compra c in checkedListBox1.CheckedItems){
                    comprasSeleccionadas.Add(c);
                }
            }

            if (comprasSeleccionadas.Count() > 0) {
                //Aca se persiste la rendicion de las comprasSeleccionadas

                MessageBox.Show("La rendición de comisiones se realizó exitosamente.", "Rendición de Comisiones", MessageBoxButtons.OK);
            } else {
                MessageBox.Show("No se seleccionó ninguna venta.", "Error", MessageBoxButtons.OK);
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.empresaSeleccionada = this.Empresas[comboBoxEmpresas.SelectedIndex];
            this.actualizarCompras();
        }

        private void actualizarCompras()
        {
            List<Compra> comprasEmpresaSeleccionada = new List<Compra>();

            //Aca guardo en esa lista las compras de la EmpresaSeleccionada que todavia
            //no hayan sido rendidas,
            //o sea que no esten en ninguna factura

           foreach(Compra c in comprasEmpresaSeleccionada){
                checkedListBox1.Items.Add(c.Publicacion + c.Importe.ToString());
            }

        }


        private void Rendicion_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        
        }
    }
}
