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

        public FinalizarCompra(MiForm anterior, Compra compra) : base(anterior)
        {
            InitializeComponent();

            Cliente cliente = Sesion.getInstance().traerCliente();
            this.Compra = compra;

            this.Compra.Importe = this.Compra.calcularImporte();

            //Estaria bueno hacer alguna magia para que en la compra aparezca el total calculado y la cantidad de entradas calculada

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

        private void button2_Click(object sender, EventArgs e)
        {
            string query = Sesion.getInstance().traerCliente().Id + ", " + this.Compra.MedioDePago.Id + ", " + this.Compra.Importe + ", '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'";
            SqlDataReader reader = Servidor.getInstance().query("exec MATE_LAVADO.registrarCompra_sp " + query);
            while (reader.Read())
            {
                this.Compra.Id = int.Parse(reader["id"].ToString());
            }
            reader.Close();

            foreach (Ubicacion u in this.Compra.Ubicaciones)
            {
                    /*if (!u.Numerada)
                    {
                        for (int i = 0; i < u.CantidadAsientos; i++)
                        {
                            string q = this.Compra.Id + ", " + (u.Id + i) + ", " + this.Compra.Espectaculo.Id;
                            Servidor.getInstance().query("exec MATE_LAVADO.registrarCompraExU_sp " + q);
                        }
                    }
                    else
                    {*/
                        foreach (Asiento a in u.Asientos)
                        {
                            string q = this.Compra.Id + ", " + (a.Id);
                            Servidor.getInstance().query("exec MATE_LAVADO.registrarCompraUbicacion_sp " + q);
                        }
                    //}
            }
            

            MessageBox.Show("La compra se realizó exitosamente!", "Compra", MessageBoxButtons.OK);
            this.cerrarAnteriores();
        }

        private void FinalizarCompra_Load(object sender, EventArgs e)
        {

        }
    }
}
