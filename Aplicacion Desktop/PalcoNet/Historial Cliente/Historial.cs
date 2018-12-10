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

namespace PalcoNet.Historial_Cliente
{
    public partial class Historial : MiForm
    {
        int offset = -1;
        Cliente cliente;

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }


        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Historial(MiForm anterior) : base(anterior)
        {
            InitializeComponent();

            if (Sesion.getInstance().rol.Nombre == "Cliente")
            {
                this.Cliente = Sesion.getInstance().traerCliente();
                this.leer(true);
            }
            else 
            {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void leer(bool paraAdelante) {
            List<ElementoHistorialCliente> historial = new List<ElementoHistorialCliente>();

            if (paraAdelante) { this.Offset++; } else { this.Offset--; }

            if (this.Offset >= 0)
            {
                Servidor servidor = Servidor.getInstance();
                SqlDataReader reader = servidor.query("exec historialClienteConOffset_sp " + this.Cliente.Id + " ," + this.Offset * 10);
                while (reader.Read())
                {
                    Console.WriteLine("+++++++++++++++++");
                    ElementoHistorialCliente e = new ElementoHistorialCliente();
                    e.CantidadAsientos = int.Parse(reader["cantidad_asientos"].ToString());
                    e.Descripcion = reader["descripcion"].ToString();
                    e.Fecha = (DateTime)reader["fecha_evento"];
                    e.Importe = decimal.Parse(reader["importe"].ToString());
                    e.NumeroTarjeta = int.Parse(reader["nro"].ToString());
                    historial.Add(e);
                }
            }
            if (historial.Count() == 0)
            {
                if (paraAdelante) { this.Offset--; } else { this.Offset++; }
                MessageBox.Show("No existen más resultados para " + (paraAdelante? "adelante." : "atrás."), "Advertencia", MessageBoxButtons.OK);
            }
            else 
            {
                var bindingList = new BindingList<ElementoHistorialCliente>(historial);
                var source = new BindingSource(bindingList, null);
                tabla.DataSource = source;
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los siguientes n elementos de la paginacion y guardarlos en la lista de abajo
            this.leer(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los anteriores n elementos de la paginacion y guardarlos en la lista de abajo
            this.leer(false);
        }
    }
}
