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
        List<Compra> comprasEmpresaSeleccionada = new List<Compra>();

        public List<Compra> ComprasEmpresaSeleccionada
        {
            get { return comprasEmpresaSeleccionada; }
            set { comprasEmpresaSeleccionada = value; }
        }

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
            List<Factura> facturas = new List<Factura>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.CheckedIndices.Contains(i))
                {
                    comprasSeleccionadas.Add(this.ComprasEmpresaSeleccionada[i]);
                    
                }
            }

            if (comprasSeleccionadas.Count() > 0)
            {
                //Aca se persiste la rendicion de las comprasSeleccionadas
                Factura factura = new Factura();
                factura.Empresa = this.EmpresaSeleccionada;
                factura.FechaDeFacturacion = Sesion.getInstance().fecha;
                factura.ImporteTotal = comprasSeleccionadas.Sum(c => c.Importe);

                Servidor servidor = Servidor.getInstance();
                string query = "'" + factura.Empresa.RazonSocial + "', '" + factura.ImporteTotal + "'";
                //Ahi arriba no es la razon soacial, es el id de la publicacion, hay que hacer como en generar publicacion (ultima pantalla)

                servidor.query("EXEC dbo.agregarFactura_sp " + query);

                foreach (Compra c in comprasSeleccionadas) {
                    string query2 = "'" + factura.Empresa.RazonSocial + "', '" + c.Id + "'";
                    servidor.query("EXEC dbo.actualizarCompraFactura_sp " + query2);
                }

                MessageBox.Show("La rendición de comisiones se realizó exitosamente.", "Rendición de Comisiones", MessageBoxButtons.OK);
            }
            else
            {
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
            

            //Aca guardo en esa lista las compras de la EmpresaSeleccionada que todavia
            //no hayan sido rendidas,
            //o sea que no esten en ninguna factura
            //Incluyendo el id!

           foreach(Compra c in comprasEmpresaSeleccionada){
                checkedListBox1.Items.Add(c.Publicacion.Descripcion + ", " + c.Importe.ToString());
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
