﻿using System;
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

namespace PalcoNet.Comprar
{
    public partial class Ubicaciones : MiForm
    {
        Compra compra;
        Servidor servidor = Servidor.getInstance();
        Sesion sesion = Sesion.getInstance();
        public List<Asiento> asientosDisponibles = new List<Asiento>();
        Ubicacion ubicacion = new Ubicacion();

        internal Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        internal Ubicacion Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; }
        }

        List<Ubicacion> ubicacionesDisponibles = new List<Ubicacion>();

        internal List<Ubicacion> UbicacionesDisponibles
        {
            get { return ubicacionesDisponibles; }
            set { ubicacionesDisponibles = value; }
        }

        public Ubicaciones(Compra compra, MiForm anterior) : base(anterior)
        {
            this.Compra = compra;
            this.Compra.Ubicaciones = new List<Ubicacion>();
            InitializeComponent();
            numericUpDownCantidad.Enabled = false;

            //Aca traemos de la base una lista de las ubicaciones disponibles de la compra que se nos paso y las guardamos en ubicacionesDisponibles

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarUbicacionesPorEspectaculo_sp " + compra.Espectaculo.Id);

            while (reader.Read())
            {
                Ubicacion u = new Ubicacion();
                u.Numerada = ! bool.Parse(reader["sin_numerar"].ToString());
                u.CantidadAsientos = int.Parse(reader["asientos"].ToString());
                if (u.Numerada) { u.CantidadFilas = int.Parse(reader["filas"].ToString()); }
                u.Precio = decimal.Parse(reader["precio"].ToString());
                u.Id = int.Parse(reader["id_ubicacion"].ToString());
                u.TipoAsiento = reader["descripcion"].ToString();
                comboBoxUbicaciones.Items.Add(u.TipoAsiento + ", $" + u.Precio.ToString());
                this.UbicacionesDisponibles.Add(u);
            }
            reader.Close();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Volvemos a la pantalla para seleccionar la fecha del espectaculo
        private void button2_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        //Con este botón vamos al siguiente paso, que consiste en elegir el medio de pago
        private void button3_Click(object sender, EventArgs e)
        {
            new MedioPago(this, this.Compra).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que ir gurdando una lista de todas las entradas
            //Verificamos que haya elegido al menos una ubicacion 

            if (this.numericUpDownCantidad.Value > 0 && this.comboBoxUbicaciones.SelectedIndex > -1)
            {

                var ubicacionPedida = new Ubicacion(ubicacion, numericUpDownCantidad.Value);

                //verificamos si el tipo de ubicacion es numerada para saber de que forma persistirla
                if (ubicacion.Numerada)
                {
                    Servidor servidor = Servidor.getInstance();

                    SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.ubicNumeradaDisponiblesSegunEspectaculoYTipoUbicacion_sp " + this.compra.Espectaculo.Id + ", '" + this.ubicacion.TipoAsiento + "', " + this.ubicacion.Precio);

                    while (reader.Read())
                    {
                        //Se guarda una lista con todos los asientos disponibles para ese espectaculo y ese tipo de ubicacion al comenzar la seleccion, para pasarsela
                        //a la pantalla de seleccionar asientos, actualizandola cada vez que se selecciona un asiento nuevo.
                        Char fila = (Convert.ToChar(reader["fila"]));
                        Int32 asiento = (Convert.ToInt32(reader["asiento"]));
                        Int32 id = (Convert.ToInt32(reader["id_ubicacion_espectaculo"]));
                        Asiento unAsiento = new Asiento();
                        unAsiento.Asiento1 = asiento;
                        unAsiento.Fila = fila;
                        unAsiento.Id = id;
                        this.asientosDisponibles.Add(unAsiento);
                    }
                    reader.Close();

                    this.Compra.Ubicaciones.Add(ubicacionPedida);

                    List<Asiento> asientos = new List<Asiento>();
                    for (int i = 0; i < this.numericUpDownCantidad.Value; i++)
                    {
                        SeleccionAsiento seleccion = new SeleccionAsiento(this, ubicacion, compra);
                        seleccion.ShowDialog();
                        seleccion.Close();
                    }
                    
                }
                //igual que antes persistimos pero de la forma sin que las ubicaciones esten numeradas
                if (!ubicacion.Numerada)
                {

                    Servidor servidor = Servidor.getInstance();

                    String query = "EXEC MATE_LAVADO.ubicSinNumerarDisponiblesSegunEspectaculoYTipoUbicacion_sp " + this.compra.Espectaculo.Id + ", '" + this.ubicacion.TipoAsiento + "'";

                    SqlDataReader reader = servidor.query(query);

                    while (reader.Read())
                    {
                        //Se guarda una lista con todos los asientos disponibles para ese espectaculo y ese tipo de ubicacion al comenzar la seleccion, para pasarsela
                        //a la pantalla de seleccionar asientos, actualizandola cada vez que se selecciona un asiento nuevo.
                        Int32 id = (Convert.ToInt32(reader["id_ubicacion_espectaculo"]));
                        Asiento unAsiento = new Asiento();
                        unAsiento.Id = id;
                        this.asientosDisponibles.Add(unAsiento);
                    }
                    reader.Close();

                    this.Compra.Ubicaciones.Add(ubicacionPedida);

                    List<Asiento> asientosDisponiblesActuales = new List<Asiento>();
                        
                    asientosDisponiblesActuales.AddRange(this.asientosDisponibles);

                    for (int i = 0; i < this.numericUpDownCantidad.Value; i++)
                    {
                        Asiento elAsiento = asientosDisponiblesActuales[i];
                        this.asientosDisponibles.Remove(elAsiento);
                        //Agrego el asiento elegido a la lista de asientos de la compra
                        this.compra.Ubicaciones.Find(u => u.TipoAsiento == this.ubicacion.TipoAsiento).Asientos.Add(elAsiento);
                    }
                }

                MessageBox.Show("Los asientos se agregaron al carrito!", "Seleccionar Asientos", MessageBoxButtons.OK);

                this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].CantidadAsientos -= ubicacionPedida.CantidadAsientos;

                if (this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].CantidadAsientos == 0)
                {
                    this.comboBoxUbicaciones.Items.RemoveAt(this.comboBoxUbicaciones.SelectedIndex);
                }

                numericUpDownCantidad.Enabled = false;
                numericUpDownCantidad.Value = 0;
                comboBoxUbicaciones.SelectedIndex = -1;

            }

        }

        private void checkedListBoxUbicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

         private void checkedListBoxUbicaciones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

         private void numericUpDownCantidad_ValueChanged(object sender, EventArgs e)
         {
             if (this.comboBoxUbicaciones.SelectedIndex > -1)
             {
                 if (numericUpDownCantidad.Value > this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex].CantidadAsientos) { numericUpDownCantidad.Value--; }
             }
         }

        //verifica que haya elegido al menos una ubicación
         private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (this.comboBoxUbicaciones.SelectedIndex > -1)
             {
                 this.ubicacion = this.UbicacionesDisponibles[this.comboBoxUbicaciones.SelectedIndex];
                 numericUpDownCantidad.Enabled = true;             
             }

         }

        //Función que busca entre los asientos disponibles el tipo que eligio el usuario y una vez seleccionado
        //lo eliminamos de la lista de los asientos disponibles.
         public void asientoSeleccionado(Char fila, int asiento)
         {
             Asiento elAsiento = asientosDisponibles.Find(a => a.Asiento1 == asiento && a.Fila == fila);
             this.compra.Ubicaciones.Find(u => u.TipoAsiento == this.ubicacion.TipoAsiento).Asientos.Add(elAsiento);
             asientosDisponibles.Remove(elAsiento);             
         }
            
    }
}
