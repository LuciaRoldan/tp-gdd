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

namespace PalcoNet.Comprar
{
    public partial class FinalizarCompra : MiForm
    {
        Compra compra;

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        //Completamos los campos con la info que recolectamos de los pasos anteriores para mostrarle una vista previa a la compra al cliente
        public FinalizarCompra(MiForm anterior, Compra compra) : base(anterior)
        {
            InitializeComponent();

            Cliente cliente = Sesion.getInstance().traerCliente();
            this.Compra = compra;

            this.Compra.Importe = this.Compra.calcularImporte();

            this.textBoxEspectaculo.Text = this.Compra.Publicacion.Descripcion;
            this.textBoxCantidad.Text = this.Compra.calcularCantidadAsientos().ToString();
            this.textBoxTotal.Text = this.Compra.Importe.ToString();
            this.textBoxFecha.Text = this.Compra.Espectaculo.Fecha.ToString();

            if (cliente == null)
            {
                button2.Enabled = false;
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        //Cuando el cliente acepta esa información se registra la compra mediante un sp al que le pasamos todos los datos
        // y las ubicaciones mediante otro sp donde se relacionara con la compra también
        private void button2_Click(object sender, EventArgs e)
        {
            string query = Sesion.getInstance().traerCliente().Id + ", " + this.Compra.MedioDePago.Id + ", " + this.Compra.Importe 
                + ", '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd hh:mm:ss.fff") + "', " + this.Compra.calcularCantidadAsientos()
                + " , '" + this.Compra.Publicacion.GradoDePublicacion + "'";
            SqlDataReader reader = Servidor.getInstance().query("exec MATE_LAVADO.registrarCompra_sp " + query);
            while (reader.Read())
            {
                this.Compra.Id = int.Parse(reader["id"].ToString());
            }
            reader.Close();

            foreach (Ubicacion u in this.Compra.Ubicaciones)
            {
                        foreach (Asiento a in u.Asientos)
                        {
                            string q = this.Compra.Id + ", " + (a.Id);
                            Servidor.getInstance().query("exec MATE_LAVADO.registrarCompraUbicacion_sp " + q);
                        }
            }
            

            MessageBox.Show("La compra se realizó exitosamente!", "Compra", MessageBoxButtons.OK);
            this.cerrarAnteriores();
        }

        private void FinalizarCompra_Load(object sender, EventArgs e)
        {

        }
    }
}
