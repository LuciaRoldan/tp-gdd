﻿using System;
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

        public Finalizar_publicacion(MiForm anterior, Publicacion publicacion)
        {
            if (Sesion.getInstance().rol.Nombre == "Empresa")
            {
                this.Empresa = Sesion.getInstance().traerEmpresa();
                this.Publicacion = publicacion;
                InitializeComponent();
                textBoxDescripcion.Text = this.publicacion.Descripcion;
                textBoxFechas.Text = this.publicacion.Fechas.Count().ToString();
                textBoxUbicaciones.Text = this.publicacion.Ubicaciones.Count().ToString();
                int asientos = 0;
                foreach (Ubicacion u in this.publicacion.Ubicaciones) { asientos = asientos + u.CantidadAsientos; }
                textBoxAsientos.Text = asientos.ToString();
            }
            else
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
            //Se guarda todo lo que se tenga que guardar en la base
            //insertar la publicacion falla con un tema de datos que no se que es

            /*string query = "'" + Sesion.getInstance().traerEmpresa().RazonSocial + "', '" + publicacion.GradoDePublicacion + "', '"
                            + publicacion.Rubro + "', '" + publicacion.Descripcion + "', '"
                            + publicacion.CantidadDeAsientos + "', '" + publicacion.Direccion + "','" + publicacion.FechaDeInicio + "', '"
                            + publicacion.FechaDeEvento + "', '" + publicacion.EstadoDePublicacion + "'";*/
            string query = "'" + Sesion.getInstance().traerEmpresa().RazonSocial + "', '" + publicacion.GradoDePublicacion + "', '"
                    + publicacion.Rubro + "', '" + publicacion.Descripcion + "', '"
                    + publicacion.EstadoDePublicacion + "', '" + publicacion.Direccion + "'";

            SqlDataReader reader = servidor.query("EXEC dbo.registrarPublicacion_sp " + query);
            

            while (reader.Read())
            {
                publicacion.Id = Convert.ToInt32(reader["id_publicacion"]);
                    
            }

            foreach (DateTime f in publicacion.Fechas) {
                foreach (Ubicacion u in publicacion.Ubicaciones) {
                    string query2 = "'" + publicacion.Id + "', '" + f + "', '"
                    + publicacion.EstadoDePublicacion + "', '" + u.TipoAsiento + "', '"
                    + u.CantidadAsientos + "', '" + (u.Numerada? u.CantidadFilas : 0) +  "', '" + u.Precio + "'";

                    servidor.query("EXEC dbo.ragregarEspectaculoYUbicaciones_sp " + query);
                }
            }

            MessageBox.Show("La publicación se creó exitosamente!", "Publicación", MessageBoxButtons.OK);
            this.cerrarAnteriores(); 
        }
    }
}
