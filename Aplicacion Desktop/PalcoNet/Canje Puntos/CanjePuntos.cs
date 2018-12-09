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

        public CanjePuntos(MiForm anterior) : base(anterior)
        {
            Cliente cliente = new Cliente();
            String username = sesion.usuario.NombreUsuario;
            String descripcion;
            int puntos;
           
            Console.WriteLine("LLEGA ACA CON ROL: " + sesion.rol.Nombre);

                     if (sesion.rol.Nombre == "Cliente") {
                         
                          SqlDataReader reader = servidor.query("EXEC dbo.getPuntos_sp '" + username + "'");
                          Console.WriteLine("ENTONCES ENTRA ACA CON USER: " + username);
                          
                         while (reader.Read())
                          {
                              Console.WriteLine(reader["cantidad_puntos"]);
                              int guarda_puntos = Convert.ToInt32(reader["cantidad_puntos"]);
                             
                          }
                          
                          reader.Close();

                           reader = servidor.query("EXEC dbo.getPremios_sp");

                          while (reader.Read())
                          {
                              Console.WriteLine("LLEGA ACA TAMBIEN");
                              Premio premio = new Premio();
                              premio.Descripcion = reader["descripcion"].ToString();
                              premio.CantidadDePuntos = Convert.ToInt32(reader["puntos"]);
                              
                        //      checkedListBoxPremios.Items.Add("descripcion" + " (" + "puntos" + " puntos)");
            
                                                           
                          }
                          reader.Close();
                          Console.WriteLine("TERMINA LOS DOS WHILE");
                        //  checkedListBoxPremios.Items.Add(premio.Descripcion + " (" + premio.CantidadDePuntos + " puntos)");

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
            
             cliente.Puntos = puntosOriginales - puntosAcumulados;
            List<Premio> premiosSeleccionados = new List<Premio>();
            foreach (int index in this.checkedListBoxPremios.SelectedIndices) {
                premiosSeleccionados.Add(this.premios[index]);
            }

            //Aca hay que actualizar los puntos del cliente y persistir los nuevos premios adquiridos
            //Como los puntos tienen un vencimiento hay que usar primero los puntos mas viejos y despues los mas nuevos

            MessageBox.Show("El canje se realizó de forma exitosa.", "Premios", MessageBoxButtons.OK);

            puntosOriginales = puntosOriginales - puntosAcumulados;
            foreach (int index in this.checkedListBoxPremios.SelectedIndices){
                checkedListBoxPremios.SetItemChecked(this.checkedListBoxPremios.SelectedIndex, false);
                puntosAcumulados = puntosAcumulados - premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            this.textBoxPuntos.Text = puntosOriginales.ToString();
            this.textBoxTotal.Text = puntosAcumulados.ToString();

            //Cambiar los puntos en el objeto cliente de la sesion
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBoxPuntos.Text = puntosAcumulados.ToString();
        }

        private void checkedListBoxPremios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.checkedListBoxPremios.GetItemCheckState(this.checkedListBoxPremios.SelectedIndex) == CheckState.Checked){
                if (puntosAcumulados + this.premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos <= puntosOriginales) {
                    puntosAcumulados = puntosAcumulados + this.premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
                } else {
                    checkedListBoxPremios.SetItemChecked(this.checkedListBoxPremios.SelectedIndex, false);
                }
            } else {
               puntosAcumulados = puntosAcumulados - this.premios[this.checkedListBoxPremios.SelectedIndex].CantidadDePuntos;
            }
            this.textBoxTotal.Text = puntosAcumulados.ToString();
        }
    }
}
