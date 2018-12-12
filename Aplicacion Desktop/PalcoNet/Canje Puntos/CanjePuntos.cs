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

            //Se verifica que el usuario actual se un cliente
            if (Sesion.getInstance().rol.Nombre == "Cliente")
            {
                this.Cliente.NombreUsuario = sesion.usuario.NombreUsuario;

                //Aca traemos los puntos que tiene el usuario actual y los mostramos en el textBox
                //Y  traemos una lista de todos los premios de la base, los guardamos en la lista premios y 
                //los mostramos en el checkedListBox

                SqlDataReader reader = servidor.query("EXEC dbo.getPuntos_sp '" + sesion.usuario.NombreUsuario + "', '" + Sesion.getInstance().fecha + "' ");
                while (reader.Read())
                {
                    puntosOriginales = Convert.ToInt32(reader["cantidad_puntos"]);
                    textBoxPuntos.Text = puntosOriginales.ToString();

                }
                reader.Close();

                reader = servidor.query("EXEC dbo.getPremios_sp");
                while (reader.Read())
                {
                    Premio p = new Premio();
                    p.Descripcion = reader["descripcion"].ToString();
                    p.CantidadDePuntos = Convert.ToInt16(reader["puntos"]);
                    this.Premios.Add(p);
                    checkedListBoxPremios.Items.Add(p.Descripcion + " (" + p.CantidadDePuntos + " puntos)");

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
            //Aca hacemos que se actualicen los puntos

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
            //Como los puntos tienen un vencimiento usamos primero los puntos mas viejos y despues los mas nuevos


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Con esto actualizamos los puntos acumulados actuales
        private void checkedListBoxPremios_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Premios.Count(); i++)
            {
                Console.WriteLine(this.Premios[i].CantidadDePuntos);
            }

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
