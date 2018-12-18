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

namespace PalcoNet.Editar_Publicacion
{
    public partial class Editar_publicacion : MiForm
    {
        List<Publicacion> publicaciones = new List<Publicacion>();
        Publicacion publicacionElegida;
        bool hayCambios = false;

        public bool HayCambios
        {
            get { return hayCambios; }
            set { hayCambios = value; }
        }

        public Publicacion PublicacionElegida
        {
            get { return publicacionElegida; }
            set { publicacionElegida = value; }
        }

        public List<Publicacion> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; }
        }

        public Editar_publicacion(MiForm anterior)
        {
            InitializeComponent();

            if (Sesion.getInstance().esEmpresa())
            {
                Empresa empresa = Sesion.getInstance().traerEmpresa();
                Servidor servidor = Servidor.getInstance();
                //Aca buscamos en la base todas las publicaciones de la empresa y las guardamos en la lista de arriba

                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarPublicacionesPorEmpresa_sp '" + Sesion.getInstance().traerEmpresa().RazonSocial + "'");

                while (reader.Read()) {
                    Publicacion publicacion = new Publicacion();
                    publicacion.Descripcion = reader["descripcion"].ToString();
                    publicacion.Direccion = reader["direccion"].ToString();
                    publicacion.EstadoDePublicacion = reader["estado"].ToString();
                    publicacion.Id = Convert.ToInt32(reader["id"]);
                    publicacion.Rubro = reader["rubro"].ToString();
                    publicaciones.Add(publicacion);
                    comboBoxPublicaciones.Items.Add(publicacion.Descripcion);
                }
            }
            else //En el caso de que no sea Empresa no podrá llevar a cabo esta funcionalidad
            {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
                button4.Enabled = false;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
        }

        //Verifica que tenga todos los campos completos
        public bool VerificarCamposUbicacion()
        {
            string errores = "";
            int x;
            decimal y;
            if(checkBoxNumerado.Checked ? !int.TryParse(textBoxFilas.Text, out x) : false) {errores += "El campo Cantidad de Filas debe contener un valor numérico.\n"; }
            if (!int.TryParse(textBoxCantidad.Text, out x)) { errores += "El campo Cantidad de Asientos debe contener un valor numérico.\n"; }
            if(!decimal.TryParse(textBoxPrecio.Text, out y)){errores += "El campo Precio debe contener un valor numérico.\n"; }
            if (string.IsNullOrWhiteSpace(textBoxAsiento.Text)) { errores += "Se debe escribir un Tipo de Asiento.\n"; }
           
            if (errores != "") { 
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        public bool VerificarCampos() {
            string errores = "";
            if (string.IsNullOrWhiteSpace(textBoxDescripcion.Text)) { errores += "El campo Descripción no puede estar vacío.\n"; }
            if (string.IsNullOrWhiteSpace(textBoxDireccion.Text)) { errores += "El campo Dirección no puede estar vacío.\n"; }
            if (comboBoxEstado.SelectedIndex < -1) { errores += "Se debe seleccionar un Estado.\n"; }
            if (comboBoxRubro.SelectedIndex < -1) { errores += "Se debe seleccionar un Rubro.\n"; }
            if (dataGridViewFechas.Rows.Count < 1) { errores += "Debe haber por lo menos una Fecha.\n"; }
            if (dataGridViewUbicaciones.Rows.Count < 1) { errores += "Debe haber por lo menos una Ubicación.\n"; }

            if (errores != "")
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.HayCambios = false;

            //Hace se completan los campos con la informacion de la publicacion seleccionada
            if (comboBoxPublicaciones.SelectedIndex > -1) {
                this.publicacionElegida = this.Publicaciones[comboBoxPublicaciones.SelectedIndex];
                textBoxDescripcion.Text = publicacionElegida.Descripcion;
                textBoxDireccion.Text = publicacionElegida.Direccion;
                comboBoxRubro.Text = publicacionElegida.Rubro;
                comboBoxEstado.Text = publicacionElegida.EstadoDePublicacion;
                Servidor servidor = Servidor.getInstance();
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarEspectaculosPorPublicacion_sp " + this.PublicacionElegida.Id);

                while (reader.Read())
                {
                    DateTime fecha = (DateTime)reader["fecha_evento"];
                    this.PublicacionElegida.Fechas.Add(fecha);
                }
                this.actualizarFechas();

                SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.buscarUbicacionesPorPublicacionEdicion_sp " + this.PublicacionElegida.Id);

                while (reader2.Read())
                {
                    Ubicacion ubicacion = new Ubicacion();
                    ubicacion.CantidadAsientos = Int32.Parse(reader2["asientos"].ToString());
                    ubicacion.Numerada = !bool.Parse(reader2["sin_numerar"].ToString());
                    if (ubicacion.Numerada) { ubicacion.CantidadFilas = Int32.Parse(reader2["filas"].ToString()); }
                    ubicacion.Precio = decimal.Parse(reader2["precio"].ToString());
                    ubicacion.TipoAsiento = reader2["descripcion"].ToString();
                    this.PublicacionElegida.Ubicaciones.Add(ubicacion);
                }
                reader2.Close();

                this.actualizarUbicaciones();
                this.cargarRubros();

                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
            } else {
                MessageBox.Show("Se debe sleccionar alguna publicación", "Error", MessageBoxButtons.OK);
            } 
        }

        private void cargarRubros()
        {
            SqlDataReader reader = Servidor.getInstance().query("EXEC MATE_LAVADO.getRubros_sp");

            while (reader.Read())
            {
                comboBoxRubro.Items.Add(reader["descripcion"].ToString());
            }
            reader.Close();
        }

        public void actualizarFechas() {
          var bindingList = new BindingList<DateTime>(this.PublicacionElegida.Fechas);
            var source = new BindingSource(bindingList, null);
            dataGridViewFechas.DataSource = source;
        }

        public void actualizarUbicaciones(){
            var bindingList = new BindingList<Ubicacion>(this.PublicacionElegida.Ubicaciones);
            var source = new BindingSource(bindingList, null);
            dataGridViewUbicaciones.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Agrega una ubicacion a la lista de abajo

            this.HayCambios = true;

            if (this.VerificarCamposUbicacion())
            {
                Ubicacion ubicacion = new Ubicacion();

                ubicacion.CantidadAsientos = Int32.Parse(textBoxCantidad.Text);
                ubicacion.Precio = Int32.Parse(textBoxPrecio.Text);
                ubicacion.TipoAsiento = textBoxAsiento.Text;
                ubicacion.Numerada = checkBoxNumerado.Checked;
                if (checkBoxNumerado.Checked) { ubicacion.CantidadFilas = Int32.Parse(textBoxFilas.Text); }

                this.PublicacionElegida.Ubicaciones.Add(ubicacion);
                this.actualizarUbicaciones();

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Agrega una fecha a la lista de abajo y validamos que la fecha sea posterior al dia de hoy


            this.HayCambios = true;

            DateTime fecha = dateTimePickerFecha.Value.Date + dateTimePickerHora.Value.TimeOfDay;
            if (fecha > Sesion.getInstance().fecha)
            {
                this.PublicacionElegida.Fechas.Add(fecha);
                this.actualizarFechas();
            }
            else
            {
                MessageBox.Show("La fecha debe ser posterior a la actual.", "Error", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hace los cambios correspondientes en la base
            //Podria salir un cartelito que diga que los cambios se guardaron correctamente

            if (this.VerificarCampos() && this.hayCambios)
            {
                this.PublicacionElegida.Descripcion = textBoxDescripcion.Text;
                this.PublicacionElegida.Direccion = textBoxDireccion.Text;
                this.PublicacionElegida.EstadoDePublicacion = comboBoxEstado.Text;
                this.PublicacionElegida.Rubro = comboBoxRubro.Text;

                //Actualizar fechas y ubicaciones
                List<DateTime> fechas = new List<DateTime>();
                foreach (DataGridViewRow dr in dataGridViewFechas.Rows){
                    DateTime fecha = (DateTime) dr.DataBoundItem;
                    fechas.Add(fecha);
                }

                List<Ubicacion> ubicaciones = new List<Ubicacion>();
                foreach (DataGridViewRow dr in dataGridViewUbicaciones.Rows)
                {
                    Ubicacion ubicacion = (Ubicacion)dr.DataBoundItem;
                    ubicaciones.Add(ubicacion);
                }

                this.PublicacionElegida.Fechas = fechas;
                this.PublicacionElegida.Ubicaciones = ubicaciones;

                Servidor servidor = Servidor.getInstance();
                servidor.query("EXEC MATE_LAVADO.actualizarPublicacion_sp " + this.PublicacionElegida.Id + ", '" + this.PublicacionElegida.Descripcion +
                    "', '" + this.PublicacionElegida.Direccion + "', '" + this.PublicacionElegida.EstadoDePublicacion + "', '" + this.PublicacionElegida.Rubro + "'");

                //Limpia los espectaculos existente y agrega los seleccionados a la publicacion
                servidor.query("EXEC MATE_LAVADO.vaciarEspectaculosPublicacion_sp " + this.PublicacionElegida.Id);

                List<Int32> ids_espectaculos = new List<Int32>();

                foreach (DateTime f in this.PublicacionElegida.Fechas)
                {
                    string query2 = "'" + this.PublicacionElegida.Id + "', '" + f.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + this.PublicacionElegida.EstadoDePublicacion + "', '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    SqlDataReader readerEspectaculo = servidor.query("EXEC MATE_LAVADO.agregarEspectaculo_sp " + query2);

                    readerEspectaculo.Read();
                    Int32 id = Convert.ToInt32(readerEspectaculo["id_espectaculo"]);
                    ids_espectaculos.Add(id);

                }

                //Agrega las ubicaciones y la relacion de ellas con la publicacion a la base
                List<Int32> ids_ubicaciones = new List<Int32>();

                foreach (Ubicacion u in this.PublicacionElegida.Ubicaciones)
                {
                    string query3 = "'" + u.TipoAsiento + "', '"
                    + u.CantidadAsientos + "', '" + (u.Numerada ? u.CantidadFilas : 0) + "', '" + u.Precio + "'";

                    SqlDataReader readerUbicaciones = servidor.query("EXEC MATE_LAVADO.agregarUbicaciones_sp " + query3);

                    while (readerUbicaciones.Read())
                    {
                        ids_ubicaciones.Add(Convert.ToInt32(readerUbicaciones["id_ubicacion"]));
                    }
                }

                foreach (Int32 id_u in ids_ubicaciones)
                {
                    foreach (Int32 id_e in ids_espectaculos)
                    {
                        string query4 = "'" + id_u + "', '" + id_e + "'";
                        servidor.query("EXEC MATE_LAVADO.agregarUbicacionXEspectaculo_sp " + query4);
                    }

                }

                MessageBox.Show("Los cambios se registraron exitosamente!", "Editar publicación", MessageBoxButtons.OK);

            }
            else
            {
                string mensaje = "Los siguientes campos deben ser completados:";
                if (string.IsNullOrWhiteSpace(textBoxDescripcion.Text)) { mensaje = mensaje + "\n Descripción"; }
                if (string.IsNullOrWhiteSpace(textBoxDireccion.Text)) { mensaje = mensaje + "\n Dirección"; }
                if (comboBoxEstado.SelectedIndex <= -1) { mensaje = mensaje + "\n Estado"; }
                if (comboBoxRubro.SelectedIndex <= -1) { mensaje = mensaje + "\n Rubro"; }

                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDescripcion_TextChanged(object sender, EventArgs e)
        {
            this.HayCambios = true;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxDireccion_TextChanged(object sender, EventArgs e)
        {
            this.HayCambios = true;
        }

        private void comboBoxEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HayCambios = true;
        }

        private void comboBoxRubro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.publicacionElegida = this.Publicaciones[comboBoxPublicaciones.SelectedIndex];

            Servidor servidor = Servidor.getInstance();
            string query5 = "'" + this.publicacionElegida.Id + "'";
            servidor.query("EXEC MATE_LAVADO.eliminarPublicacion_sp " + query5);




        }

        private void dataGridViewUbicaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
