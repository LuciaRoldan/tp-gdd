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
using System.Data.SqlClient;

namespace PalcoNet.Generar_Rendicion_Comisiones
{
    public partial class Rendicion : MiForm
    {
        List<Compra> comprasEmpresaSeleccionada = new List<Compra>();
        string empresaSeleccionada;

        public string EmpresaSeleccionada
        {
            get { return empresaSeleccionada; }
            set { empresaSeleccionada = value; }
        }

        public List<Compra> ComprasEmpresaSeleccionada
        {
            get { return comprasEmpresaSeleccionada; }
            set { comprasEmpresaSeleccionada = value; }
        }

        public Rendicion(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            //Aca hay que buscar en la base de datos todas las Empresas

            Servidor servidor = Servidor.getInstance();

            SqlDataReader reader = servidor.query("EXEC dbo.traerTodasRazonesSociales_sp");

            while (reader.Read()) {
                string leido = reader["razon_social"].ToString();
                comboBoxEmpresas.Items.Add(leido);

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
                factura.Empresa = new Empresa();
                factura.Empresa.RazonSocial = this.EmpresaSeleccionada;
                factura.ImporteTotal = comprasSeleccionadas.Sum(c => c.Importe * c.Comision);

                Servidor servidor = Servidor.getInstance();
                string query = "'" + factura.Empresa.RazonSocial + "', '" + factura.ImporteTotal + "'";

                SqlDataReader reader = servidor.query("EXEC dbo.agregarFactura_sp " + query);

                int idFactura = 0;
                while (reader.Read())
                {
                    idFactura = Convert.ToInt32(reader["id_factura"]);

                }

                foreach (Compra c in comprasSeleccionadas) {
                    string query2 = "'" + idFactura + "', '" + c.Id + "'";
                    servidor.query("EXEC dbo.actualizarCompraFactura_sp " + query2);
                }

                MessageBox.Show("La rendición de comisiones se realizó exitosamente.", "Rendición de Comisiones", MessageBoxButtons.OK);
                checkedListBox1.Items.Clear();
                //this.Anterior.Show();
                //this.Close();
            }
            else
            {
                MessageBox.Show("No se seleccionó ninguna venta.", "Error", MessageBoxButtons.OK);
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void actualizarCompras()
        {
            Console.WriteLine(" \n Tengo que actualizar para " + this.EmpresaSeleccionada);

            //Aca guardo en esa lista las compras de la EmpresaSeleccionada que todavia
            //no hayan sido rendidas,
            //o sea que no esten en ninguna factura
            //Incluyendo el id!

            checkedListBox1.Items.Clear();

            Servidor servidor = Servidor.getInstance();

            SqlDataReader reader = servidor.query("EXEC dbo.buscarComprasNoFacturadas_sp '" + this.EmpresaSeleccionada + "'");

            while (reader.Read())
            {
                Compra compra = new Compra();
                compra.Id = Int32.Parse(reader["id_compra"].ToString());
                compra.Importe = decimal.Parse(reader["importe"].ToString());
                compra.Comision = decimal.Parse(reader["comision"].ToString());
                Publicacion publicacion = new Publicacion();
                publicacion.Descripcion = reader["descripcion"].ToString();
                compra.Publicacion = publicacion;
                checkedListBox1.Items.Add(compra.Publicacion.Descripcion + ", $" + compra.Importe);
                this.ComprasEmpresaSeleccionada.Add(compra);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.empresaSeleccionada = comboBoxEmpresas.Text;
            this.actualizarCompras();
        }
    }
}
