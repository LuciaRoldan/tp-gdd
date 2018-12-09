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
        Servidor servidor = Servidor.getInstance();
        Sesion sesion = Sesion.getInstance();
        Cliente cliente;
        int puntosOriginales;
        int puntosAcumulados = 0;
        List<Premio> premios = new List<Premio>();

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
            Cliente cliente = new Cliente();
            String username = sesion.usuario.NombreUsuario;
           
            Console.WriteLine("LLEGA ACA CON ROL: " + sesion.rol.Nombre);

                     if (sesion.rol.Nombre == "Cliente") {
                         
                          SqlDataReader reader = servidor.query("EXEC dbo.getPuntos_sp '" + username + "'");
                          Console.WriteLine("ENTONCES ENTRA ACA CON USER: " + username);
                          while (reader.Read())
                          {

                              puntosAcumulados = Convert.ToInt32(reader["cantidad_puntos"]);
                              cliente.Puntos = Convert.ToInt32(reader["cantidad_puntos"]);
                              Console.WriteLine("SALE DEL WHILE CON PUNTOS: " + puntosAcumulados);
                          }
                          
                          reader.Close();
                         
                          reader = servidor.query("EXEC dbo.getPremios_sp");

                          while (reader.Read())
                          {
                              Console.WriteLine("LLEGA ACA TAMBIEN");
                              Premio premio = new Premio();
                              premio.Descripcion = reader["nombre"].ToString();
                              premio.CantidadDePuntos = Convert.ToInt32(reader["puntos"]);
                              checkedListBoxPremios.Items.Add(premio.Descripcion + " (" + premio.CantidadDePuntos + " puntos)");
                              premios.Add(premio);
                              Console.WriteLine("LLEGA AL FINAL CON PREMIO: " + premio.Descripcion);
                              
                          }
                          reader.Close();
                          Console.WriteLine("TERMINA LOS DOS WHILE");

                          //Aca hay que traer una lista de todos los premios de la base y guardarlos en la lista premios

                      } else {
                          MessageBox.Show("Se encuentra loggeado como " + sesion.rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
                        button1.Enabled = false;
      
                      }
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se cambien los puntos
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
            
             cliente.Puntos = this.PuntosOriginales - this.PuntosAcumulados;
            List<Premio> premiosSeleccionados = new List<Premio>();
            foreach (int index in this.checkedListBoxPremios.SelectedIndices) {
                premiosSeleccionados.Add(this.Premios[index]);
            }

            //Aca hay que actualizar los puntos del cliente y persistir los nuevos premios adquiridos
            //Como los puntos tienen un vencimiento hay que usar primero los puntos mas viejos y despues los mas nuevos

            MessageBox.Show("El canje se realizó de forma exitosa.", "Premios", MessageBoxButtons.OK);

            this.PuntosOriginales = this.PuntosOriginales - this.PuntosAcumulados;
            foreach (int index in this.checkedListBoxPremios.SelectedIndices){
                checkedListBoxPremios.SetItemChecked(this.checkedListBoxPremios.SelectedIndex, false);
                this.PuntosAcumulados = this.PuntosAcumulados - this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            this.textBoxPuntos.Text = this.PuntosOriginales.ToString();
            this.textBoxTotal.Text = this.PuntosAcumulados.ToString();

            //Cambiar los puntos en el objeto cliente de la sesion
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBoxPremios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.checkedListBoxPremios.GetItemCheckState(this.checkedListBoxPremios.SelectedIndex) == CheckState.Checked){
                if (this.PuntosAcumulados + this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos <= this.PuntosOriginales) {
                    this.PuntosAcumulados = this.PuntosAcumulados + this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
                } else {
                    checkedListBoxPremios.SetItemChecked(this.checkedListBoxPremios.SelectedIndex, false);
                }
            } else {
                this.PuntosAcumulados = this.PuntosAcumulados - this.Premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            this.textBoxTotal.Text = this.PuntosAcumulados.ToString();
        }
    }
}
