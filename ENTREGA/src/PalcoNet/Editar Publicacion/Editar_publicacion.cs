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
        bool hayCambiosDeFecha = false;
        bool hayCambiosDeUbicaciones = false;
        bool hayCambiosDeBase = false;

        public bool HayCambiosDeFecha
        {
            get { return hayCambiosDeFecha; }
            set { hayCambiosDeFecha = value; }
        }

        public bool HayCambiosDeUbicaciones
        {
            get { return hayCambiosDeUbicaciones; }
            set { hayCambiosDeUbicaciones = value; }
        }

        public bool HayCambiosDeBase
        {
            get { return hayCambiosDeBase; }
            set { hayCambiosDeBase = value; }
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

            //verificamos que el usuario sea empresa
            if (Sesion.getInstance().esEmpresa())
            {
                Empresa empresa = Sesion.getInstance().traerEmpresa();
                
                this.buscarPublicaciones();             

                dateTimePickerFecha.MinDate = Sesion.getInstance().fecha;
            }
            else //En el caso de que no sea Empresa no podrá llevar a cabo esta funcionalidad
            {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
                button4.Enabled = false;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button6.Enabled = false;
            button6.Enabled = true;
        }

        //Traemos todas las publicaciones de la empresa y las mostramos en el combo box para que el usuario pueda
        //elegir cual modificar
        public void buscarPublicaciones() {

            comboBoxPublicaciones.Items.Clear();
            publicaciones.Clear();
            comboBoxPublicaciones.Text = "";

            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarPublicacionesPorEmpresa_sp '" + Sesion.getInstance().traerEmpresa().RazonSocial + "'");

            while (reader.Read())
            {
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

        //Verifica que tenga todos los campos completos y con el tipo de dato correcto
        public bool VerificarCamposUbicacion()
        {
            string errores = "";
            int x;
            decimal y;
            if(checkBoxNumerado.Checked ? !int.TryParse(textBoxFilas.Text, out x) : false) {errores += "El campo Cantidad de Filas debe contener un valor numérico.\n"; }
            if (!int.TryParse(textBoxCantidad.Text, out x)) { errores += "El campo Cantidad de Asientos debe contener un valor numérico.\n"; }
            if(!decimal.TryParse(textBoxPrecio.Text, out y)){errores += "El campo Precio debe contener un valor numérico.\n"; }
            if (string.IsNullOrWhiteSpace(textBoxAsiento.Text)) { errores += "Se debe escribir un Tipo de Asiento.\n"; }
            if (decimal.TryParse(textBoxPrecio.Text, out y)) { if (decimal.Parse(textBoxPrecio.Text) <= 0) { errores += "El campo Precio debe contener un valor positivo.\n"; } }
           
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

        //Volvemos a la pantalla anterior a seleccionar funcionalidad
        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        //Cuando elegimos una publicacion se limpian todos los campos 
        private void button4_Click(object sender, EventArgs e)
        {
            this.HayCambiosDeFecha = false;
            this.HayCambiosDeUbicaciones = false;
            this.HayCambiosDeBase = false;

            this.limpiar();

            //Se completan los campos con la informacion (traida de la base) de la publicacion seleccionada
            if (comboBoxPublicaciones.SelectedIndex > -1) {
                this.publicacionElegida = this.Publicaciones[comboBoxPublicaciones.SelectedIndex];
                textBoxDescripcion.Text = publicacionElegida.Descripcion;
                textBoxDireccion.Text = publicacionElegida.Direccion;
                comboBoxRubro.Text = publicacionElegida.Rubro;
                comboBoxEstado.Text = publicacionElegida.EstadoDePublicacion;
                Servidor servidor = Servidor.getInstance();
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarEspectaculosBorradorPorPublicacion_sp " + this.PublicacionElegida.Id);

                this.PublicacionElegida.Fechas.Clear();
                while (reader.Read())
                {
                    DateTime fecha = (DateTime)reader["fecha_evento"];
                    this.PublicacionElegida.Fechas.Add(fecha);
                }
                this.actualizarFechas();

                SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.buscarUbicacionesPorPublicacionEdicion_sp " + this.PublicacionElegida.Id);

                this.PublicacionElegida.Ubicaciones.Clear();
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
                button7.Enabled = true;
                button8.Enabled = true;
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

        //mostramos en la tabla las fehas traidas de la base de la publicación elegida
        public void actualizarFechas() {
          var bindingList = new BindingList<DateTime>(this.PublicacionElegida.Fechas);
            var source = new BindingSource(bindingList, null);
            dataGridViewFechas.DataSource = source;
        }

        //mostramos en la tabla las ubicaciones traidas de la base de la publicación elegida
        public void actualizarUbicaciones(){
            var bindingList = new BindingList<Ubicacion>(this.PublicacionElegida.Ubicaciones);
            var source = new BindingSource(bindingList, null);
            dataGridViewUbicaciones.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Agrega una ubicacion a la lista de abajo

            this.HayCambiosDeUbicaciones = true;

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

            DateTime fecha = dateTimePickerFecha.Value.Date + dateTimePickerHora.Value.TimeOfDay;
            if (!PublicacionElegida.Fechas.Contains(fecha))
            {
                this.HayCambiosDeFecha = true;

                if (fecha > this.fechaMaximaActual())
                {
                    this.PublicacionElegida.Fechas.Add(fecha);
                    this.actualizarFechas();
                }
                else
                {
                    MessageBox.Show("La fecha debe ser posterior a la actual.", "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("No puede haber fechas repetidas.", "Error", MessageBoxButtons.OK);
            }
        }

        private DateTime fechaMaximaActual()
        {
            var fechas = this.PublicacionElegida.Fechas;
            if (fechas.Count == 0) { return Sesion.getInstance().fecha; }
            return fechas.Max();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Verifica los cambios y valida que cosas han sido modificadas para luego mediante un sp actualizar 
            //la informacion de la publicación en la base

            if (this.VerificarCampos() && (this.hayCambiosDeBase || this.HayCambiosDeUbicaciones || this.HayCambiosDeFecha))
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

                this.PublicacionElegida.Fechas.Clear();
                this.PublicacionElegida.Ubicaciones.Clear();
                this.PublicacionElegida.Fechas = fechas;
                this.PublicacionElegida.Ubicaciones = ubicaciones;

                Servidor servidor = Servidor.getInstance();
                if(this.HayCambiosDeBase){
                    servidor.query("EXEC MATE_LAVADO.actualizarPublicacion_sp " + this.PublicacionElegida.Id + ", '" + this.PublicacionElegida.Descripcion +
                        "', '" + this.PublicacionElegida.Direccion + "', '" + this.PublicacionElegida.EstadoDePublicacion + "', '" + this.PublicacionElegida.Rubro + "'");
                }

                //Limpia los espectaculos existente y agrega los seleccionados a la publicacion
                if(this.HayCambiosDeFecha || this.HayCambiosDeUbicaciones){
                    progressBar1.Maximum = this.PublicacionElegida.Fechas.Count() * this.PublicacionElegida.Ubicaciones.Sum(ubi => ubi.CantidadAsientos);
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
                            this.progressBar1.Increment(1);
                        }

                    }
                }

                MessageBox.Show("Los cambios se registraron exitosamente!", "Editar publicación", MessageBoxButtons.OK);

                this.limpiar();

                this.buscarPublicaciones();

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

        //Función para limpiar los campos
        public void limpiar() {

            dataGridViewFechas.DataSource = null;
            dataGridViewUbicaciones.DataSource = null;
            this.textBoxAsiento.Text = "";
            this.textBoxCantidad.Text = "";
            this.textBoxDescripcion.Text = "";
            this.textBoxDireccion.Text = "";
            this.textBoxFilas.Text = "";
            this.textBoxPrecio.Text = "";
            this.comboBoxEstado.SelectedIndex = -1;
            this.comboBoxRubro.SelectedIndex = -1;
            dateTimePickerFecha.Value = dateTimePickerFecha.MinDate;
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
            this.HayCambiosDeBase = true;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxDireccion_TextChanged(object sender, EventArgs e)
        {
            this.HayCambiosDeBase = true;
        }

        private void comboBoxEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HayCambiosDeBase = true;
        }

        private void comboBoxRubro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //Borrar espectaculo
            this.hayCambiosDeFecha = true;
            this.publicacionElegida.Fechas.Remove((DateTime) this.dataGridViewFechas.CurrentRow.DataBoundItem);
            this.actualizarFechas();
        }

        //Borra la publicación de la base y los espectaculos relacionados a ella
        private void button7_Click_1(object sender, EventArgs e)
        {
            this.publicacionElegida = this.Publicaciones[comboBoxPublicaciones.SelectedIndex];

            Servidor servidor = Servidor.getInstance();
            servidor.query("EXEC MATE_LAVADO.vaciarEspectaculosPublicacion_sp " + this.PublicacionElegida.Id);
            servidor.query("EXEC MATE_LAVADO.eliminarPublicacion_sp " + this.publicacionElegida.Id);

            MessageBox.Show("La publicación se eliminó exitosamente!", "Editar publicación", MessageBoxButtons.OK);
        }

        private void dataGridViewUbicaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Borrar ubicacion
            this.publicacionElegida.Ubicaciones.Remove((Ubicacion)dataGridViewUbicaciones.CurrentRow.DataBoundItem);
            this.actualizarUbicaciones();
            this.HayCambiosDeUbicaciones = true;
        }

        private void checkBoxNumerado_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNumerado.Checked)
            {
                textBoxFilas.Enabled = true;
            }
            else
            {
                textBoxFilas.Enabled = false;
            }
        }

        private void dataGridViewFechas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void comboBoxRubro_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.HayCambiosDeBase = true;
        }

        private void textBoxPrecio_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
