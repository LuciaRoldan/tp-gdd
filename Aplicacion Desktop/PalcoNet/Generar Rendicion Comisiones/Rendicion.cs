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
            //Aca hay que buscar en la base de datos todas las Empresas y las ponemos en el comboBox para que el
            //usuario elija cual quiere

            Servidor servidor = Servidor.getInstance();

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.traerTodasRazonesSociales_sp");

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
                //Aca se persiste la rendicion de las comprasSeleccionadas que consiste en la factura y los items
                //de la factura
                Factura factura = new Factura();
                factura.ImporteTotal = comprasSeleccionadas.Sum(c => c.Importe * c.Comision);

                Servidor servidor = Servidor.getInstance();
                string query = "'" + this.EmpresaSeleccionada + "', '" + factura.ImporteTotal + "'";

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.agregarFactura_sp " + query);

                int idFactura = 0;
                while (reader.Read())
                {
                    idFactura = Convert.ToInt32(reader["id_factura"]);

                }

                foreach (Compra c in comprasSeleccionadas)
                {
                    string query2 = idFactura + ", " + c.Id + " , " + c.Ubicaciones[0].Id + " , " + c.CantidadEntradas + " , " + c.Importe + " , " + c.Comision;
                    servidor.query("EXEC MATE_LAVADO.crearItemFactura_sp " + query2);
                }

                MessageBox.Show("La rendición de comisiones se realizó exitosamente.", "Rendición de Comisiones", MessageBoxButtons.OK);
                checkedListBox1.Items.Clear();

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

            //Aca guardo en esa lista las compras de la EmpresaSeleccionada que todavia
            //no hayan sido rendidas,
            //o sea que no esten en ninguna factura
            //Incluyendo el id!

            checkedListBox1.Items.Clear();

            Servidor servidor = Servidor.getInstance();

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarComprasNoFacturadas_sp '" + this.EmpresaSeleccionada + "'");

            while (reader.Read())
            {
                Compra compra = new Compra();
                compra.Id = Int32.Parse(reader["id_compra"].ToString());
                compra.Comision = decimal.Parse(reader["comision"].ToString());
                compra.CantidadEntradas = Int32.Parse(reader["cantidad"].ToString());
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.Id = Int32.Parse(reader["id_tipo_ubicacion"].ToString());
                ubicacion.TipoAsiento = reader["descripcion"].ToString();
                ubicacion.Precio = decimal.Parse(reader["precio"].ToString());
                compra.Ubicaciones = new List<Ubicacion>();
                compra.Ubicaciones.Add(ubicacion);
                checkedListBox1.Items.Add("Compra " + compra.Id + ", " + compra.CantidadEntradas + " entradas tipo " + ubicacion.TipoAsiento + " a $" + ubicacion.Precio);
                compra.Importe = ubicacion.Precio * compra.CantidadEntradas;
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
