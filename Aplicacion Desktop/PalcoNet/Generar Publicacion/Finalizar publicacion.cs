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

namespace PalcoNet.Generar_Publicacion
{
    public partial class Finalizar_publicacion : MiForm
    {
        Empresa empresa;
        Publicacion publicacion;
        Servidor servidor = Servidor.getInstance();

        public Publicacion Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }

        public Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        public Finalizar_publicacion(MiForm anterior, Empresa empresa, Publicacion publicacion)
        {
            this.Empresa = empresa;
            this.Publicacion = publicacion;
            InitializeComponent();
            textBoxDescripcion.Text = this.publicacion.Descripcion;
            textBoxFechas.Text = this.publicacion.Fechas.Count().ToString();
            textBoxUbicaciones.Text = this.publicacion.Ubicaciones.Count().ToString();
            int asientos = 0;
            foreach (Ubicacion u in this.publicacion.Ubicaciones) { asientos = asientos + u.CantidadAsientos; }
            textBoxAsientos.Text = asientos.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Se guarda todo lo que se tenga que guardar en la base
            //insertar la publicacion falla con un tema de datos que no se que es

            string query = "'" + Sesion.sesion.usuario + "', '" + publicacion.GradoDePublicacion + "', '"
                            + publicacion.Rubro + "', '" + publicacion.Descripcion + "', '"
                            + publicacion.CantidadDeAsientos + "', '" + publicacion.Direccion + "','" + publicacion.FechaDeInicio + "', '"
                            + publicacion.FechaDeEvento + "', '" + publicacion.EstadoDePublicacion + "'";

                Console.WriteLine(query);

                servidor.realizarQuery("EXEC dbo.agregarPublicacion_sp " + query);
 
             /*   string queryUbicacion;
                foreach (Ubicacion ubicacion in publicacion.Ubicaciones)
                {
                              queryUbicacion = "'" + agregarCodigoTipo + "', '" + ubicacion.TipoAsiento + "', '"
                                        + ubicacion.fi + "', '" + publicacion.Descripcion + "', '"
                                        + publicacion.CantidadDeAsientos + "', '" + publicacion.Direccion + "','" + publicacion.FechaDeInicio + "', '"
                                        + publicacion.FechaDeEvento + "', " + publicacion.EstadoDePublicacion + "'"; ;
            esto seria lo que iria para el SP que se haga para agregar la ubicacion
                }
                    //Osea aca lo que hay que hacer es ver el sp de insertar la publicacion con el espectaculo que lo arme 
              * pero hay algo que esta mal y el sp para insertar la ubicacion con ubicacionXespectaculo

            */

            MessageBox.Show("La publicación se creó exitosamente!", "Publicación", MessageBoxButtons.OK);
            this.cerrarAnteriores(); 
        }
    }
}
