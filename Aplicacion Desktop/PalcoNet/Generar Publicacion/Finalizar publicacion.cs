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

               SqlDataReader reader = servidor.query("EXEC dbo.agregarPublicacion_sp " + query);
            

                while (reader.Read())
                {
                    publicacion.Id = Convert.ToInt32(reader["id_publicacion"]);
                    
                }

 
                string queryUbicacion;
                foreach (Ubicacion u in this.publicacion.Ubicaciones)
                {
                    if(u.Numerada){
                        queryUbicacion = "'" + publicacion.Id + "', '" + u.TipoAsiento + "', '"
                                         + u.CantidadAsientos + "', '" + u.CantidadFilas + "', '"
                                         + u.Precio + "'";
                       servidor.realizarQuery("EXEC dbo.agregarUbicacionNumerada_sp " + query);


                    } else{
                        queryUbicacion = "'" + publicacion.Id + "', '" + u.TipoAsiento + "', '"
                                         + u.CantidadAsientos + "', '" + u.Precio + "'";
                        servidor.realizarQuery("EXEC dbo.agregarUbicacionSinNumerar_sp " + query);
                    }

                }

            MessageBox.Show("La publicación se creó exitosamente!", "Publicación", MessageBoxButtons.OK);
            this.cerrarAnteriores(); 
        }
    }
}
