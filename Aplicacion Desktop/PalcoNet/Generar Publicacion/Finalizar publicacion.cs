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
                this.Empresa = (Empresa)Sesion.getInstance().usuario;
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
            MessageBox.Show("La publicación se creó exitosamente!", "Publicación", MessageBoxButtons.OK);
            this.cerrarAnteriores(); 
        }
    }
}
