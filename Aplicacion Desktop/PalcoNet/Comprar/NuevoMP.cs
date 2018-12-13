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
    public partial class NuevoMP : Form
    {
        Tarjeta tarjeta;

        internal Tarjeta Tarjeta
        {
            get { return tarjeta; }
            set { tarjeta = value; }
        }

        MedioPago anterior;

        public MedioPago Anterior
        {
            get { return anterior; }
            set { anterior = value; }
        }

        public NuevoMP(MedioPago anterior)
        {
            this.Anterior = anterior;
            InitializeComponent();
        }
        public bool verificarCampos() {
            string errores = "";
            long dni;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNumero.Text)
                && !string.IsNullOrWhiteSpace(textBoxTitular.Text);

            if (!camposCompletos)
            {
                errores += "Se debe completar ambos campos.";
            }
            else
            {
                if (!long.TryParse(textBoxNumero.Text, out dni)) { errores += "El Número de Tarjeta debe ser un valor numérico."; }
            }

            if (errores != "")
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            anterior.updateMP();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos()){
                this.Tarjeta = new Tarjeta();
                this.Tarjeta.NumeroDeTarjeta = long.Parse(this.textBoxNumero.Text);
                this.Tarjeta.Titular = this.textBoxTitular.Text;
                //Agregar el medio de pago a la base registrarMedioDePago_sp

                try
                {
                    Servidor servidor = Servidor.getInstance();
                    SqlDataReader reader = servidor.query("exec MATE_LAVADO.registrarMedioDePago_sp " + Sesion.getInstance().traerCliente().Id + " ," + this.Tarjeta.NumeroDeTarjeta + ", '" + this.Tarjeta.Titular + "'");
                    while (reader.Read()) {
                        tarjeta.Id = int.Parse(reader["id_mp"].ToString());
                    }
                    this.Anterior.actualizar(this.Tarjeta);

                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++");
                    MessageBox.Show("El nuevo medio de pago se ha ingresado al sistema exitosamente.", "Medio de Pago", MessageBoxButtons.OK);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}
