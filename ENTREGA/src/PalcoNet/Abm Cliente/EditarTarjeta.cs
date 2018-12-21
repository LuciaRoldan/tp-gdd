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

namespace PalcoNet.Abm_Cliente
{
    public partial class EditarTarjeta : MiForm
    {
        Cliente cliente;
        Servidor servidor = Servidor.getInstance();
        Tarjeta tarjetaVieja;
        bool modificoTarjeta;

        public Tarjeta TarjetaVieja
        {
          get { return tarjetaVieja; }
          set { tarjetaVieja = value; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }
        
        //Traemos el form anterior, la tarjeta seleccionada y el cliente dueño de la tarjeta a modificar
        public EditarTarjeta(EditarMPs anterior, Tarjeta tarjeta, Cliente cliente)
        {
            InitializeComponent();
            this.Anterior = anterior;
            this.Cliente = cliente;
            this.tarjetaVieja = tarjeta;

            textBoxNumero.Text = tarjetaVieja.NumeroDeTarjeta.ToString();
            textBox1.Text = TarjetaVieja.Titular;
        }

        //Botón para volver al data grid de las tarjetas del cliente
        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        //Una vez hechas las modificaciones verificamos que los campos sean validos y usamos un sp para actualizar la informacion en la base
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos())
            {
                Tarjeta tarjetaModificada = new Tarjeta();

                    tarjetaModificada.NumeroDeTarjeta = long.Parse(textBoxNumero.Text);
                    tarjetaModificada.Titular = textBox1.Text;

                    String query = tarjetaVieja.Id + ", '" + tarjetaModificada.NumeroDeTarjeta + "', '" + tarjetaModificada.Titular + "'";

                    servidor.realizarQuery("EXEC MATE_LAVADO.modificarTarjeta_sp " + query);

                ((EditarMPs)this.Anterior).actualizarMP();
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar medio de pago", MessageBoxButtons.OK);
                this.Anterior.Show();
                this.Close();
            }
        }

        //Función para verificar todos los campos y tipos de datos
        public bool verificarCampos()
        {
            string error = "";
            long i;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxNumero.Text) && !string.IsNullOrWhiteSpace(textBox1.Text);

            if (!camposCompletos)
            {
                error += "Se debe ingresar un número de tarjeta y el nombre del titular";
            }
            else
            {
                bool esNumero = long.TryParse(textBoxNumero.Text, out i);
                if (this.TarjetaVieja.NumeroDeTarjeta != i) { if (!esNumero) { modificoTarjeta = true; error += "El número de tarjeta debe ser un valor numérico."; } }
            }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void textBoxNumero_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
}
