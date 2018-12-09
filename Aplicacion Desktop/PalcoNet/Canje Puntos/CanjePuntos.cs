using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Canje_Puntos
{
    public partial class CanjePuntos : MiForm
    {
        Cliente cliente = new Cliente();
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
        int puntosOriginales;
        int puntosAcumulados = 0;
        List<Premio> premios = new List<Premio>();
        Premio premio = new Premio();

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        internal List<Premio> Premios
        {
            get { return premios; }
            set { premios = value; }
        }

        public int PuntosOriginales
        {
            get { return puntosOriginales; }
            set { puntosOriginales = value; }
        }

        public int PuntosAcumulados
        {
            get { return puntosAcumulados; }
            set { puntosAcumulados = value; }
        }

        public CanjePuntos(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            if (Sesion.getInstance().rol.Nombre == "Cliente")
            {
                this.Cliente.NombreUsuario = sesion.usuario.NombreUsuario;
              //  this.PuntosOriginales = this.Cliente.Puntos;
              //  this.textBoxPuntos.Text = this.Cliente.Puntos.ToString();

                //Aca hay que traer una lista de todos los premios de la base y guardarlos en la lista premios

                SqlDataReader reader = servidor.query("EXEC dbo.getPuntos_sp '" + sesion.usuario.NombreUsuario + "'");
                while (reader.Read())
                {
                    puntosOriginales = Convert.ToInt32(reader["cantidad_puntos"]);
                    textBoxPuntos.Text = puntosOriginales.ToString();

                }
                reader.Close();

                reader = servidor.query("EXEC dbo.getPremios_sp");
                while (reader.Read())
                {
                    premio.Descripcion = reader["descripcion"].ToString();
                    premio.CantidadDePuntos = Convert.ToInt16(reader["puntos"]);
                    this.Premios.Add(premio);
                    checkedListBoxPremios.Items.Add(premio.Descripcion + " (" + premio.CantidadDePuntos + " puntos)");

                }
                reader.Close();
            }
            else
            {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
                button1.Enabled = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se cambien los puntos
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
            if (this.puntosOriginales >= this.PuntosAcumulados)
            {
                servidor.realizarQuery("EXEC borrarPuntos_sp '" + this.PuntosAcumulados + "', '" + sesion.usuario.NombreUsuario + "'");
                this.Cliente.Puntos = this.PuntosOriginales - this.PuntosAcumulados;
                List<Premio> premiosSeleccionados = new List<Premio>();
                foreach (int index in this.checkedListBoxPremios.SelectedIndices)
                {
                    premiosSeleccionados.Add(this.Premios[index]);
                }
                MessageBox.Show("El canje se realizó de forma exitosa.", "Premios", MessageBoxButtons.OK);

                this.PuntosOriginales = this.PuntosOriginales - this.PuntosAcumulados;
                foreach (int index in this.checkedListBoxPremios.SelectedIndices)
                {
                    checkedListBoxPremios.SetItemChecked(this.checkedListBoxPremios.SelectedIndex, false);
                    this.PuntosAcumulados = this.PuntosAcumulados - this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
                }
                this.textBoxPuntos.Text = this.PuntosOriginales.ToString();
                this.textBoxTotal.Text = this.PuntosAcumulados.ToString();
            }
            else
            {
                MessageBox.Show("Su monto actual de puntos no le permite canjear el obsequio, prube en otro momento.", "Premios", MessageBoxButtons.OK);
            }
            

            //Aca hay que actualizar los puntos del cliente y persistir los nuevos premios adquiridos
            //Como los puntos tienen un vencimiento hay que usar primero los puntos mas viejos y despues los mas nuevos


            //Cambiar los puntos en el objeto cliente de la sesion
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBoxPremios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.checkedListBoxPremios.GetItemCheckState(this.checkedListBoxPremios.SelectedIndex) == CheckState.Checked)
            {
                 this.PuntosAcumulados = this.PuntosAcumulados + this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            else
            {
                this.PuntosAcumulados = this.PuntosAcumulados - this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            this.textBoxTotal.Text = this.PuntosAcumulados.ToString();
        }

        private void textBoxTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
