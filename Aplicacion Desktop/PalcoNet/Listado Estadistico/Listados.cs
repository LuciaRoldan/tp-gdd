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

namespace PalcoNet.Listado_Estadistico
{
    public partial class Listados : MiForm
    {

        int anio = -1;
        Servidor servidor = Servidor.getInstance();

        public int Anio
        {
            get { return anio; }
            set { anio = value; }
        }
        string trimestre = "";

        public string Trimestre
        {
            get { return trimestre; }
            set { trimestre = value; }
        }

        DateTime fechaInicio;

        public DateTime FechaInicio
        {
            get { return fechaInicio; }
            set { fechaInicio = value; }
        }
        DateTime fechaFin;

        public DateTime FechaFin
        {
            get { return fechaFin; }
            set { fechaFin = value; }
        }

        public Listados(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
        }

        private bool verificarCampos() {
            trimestre = trimestreCombobox.Text;
            return anio != -1 && trimestre != "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos())
            {
                DateTime inicio = armarFechaInicio();
                DateTime fin = armarFechaFin();
                new ClientesMuchasCompras(inicio, fin, this).Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Se deben seleccionar el año y el trimestre", "Error", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos())
            {
                DateTime inicio = armarFechaInicio();
                DateTime fin = armarFechaFin();
                new EmpresaLocalidades(inicio, fin, this).Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Se deben seleccionar el año y el trimestre", "Error", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            trimestre = trimestreCombobox.Text.ToString();

            if (this.verificarCampos())
            {
                DateTime inicio = armarFechaInicio();
                DateTime fin = armarFechaFin();

                SqlDataReader reader = servidor.query("EXEC dbo.top5ClientesPuntosVencidos_sp '" + inicio.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + fin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
                List<Cliente> clientesOrdenadosPorPuntos = new List<Cliente>();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.Nombre = reader["nombre"].ToString();
                    cliente.Apellido = reader["apellido"].ToString();
                    //cliente.Puntos Vencidos

                    clientesOrdenadosPorPuntos.Add(cliente);
                }
                reader.Close();

            
                //aca consulta a la BD por la lista de todos los clientes ordenados
                //por la cantidad de puntos vencidos DESC
                //a esa consulta le voy a pasar dos fechas: inicio y fin

                new ClientesPuntos(clientesOrdenadosPorPuntos, this).Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Se deben seleccionar el año y el trimestre", "Error", MessageBoxButtons.OK);
            }
        }

        private void anioCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            anio = Int32.Parse(anioCombobox.Text.ToString());
        }


        private void trimestreCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            trimestre = trimestreCombobox.Text;
        }

        private DateTime armarFechaInicio()
        {
            switch (trimestre)
            {
                case "Enero-Marzo": return new DateTime(anio, 1, 1);
                case "Abril-Junio": return new DateTime(anio, 4, 1);
                case "Julio-Septiembre": return new DateTime(anio, 7, 1);
                case "Octubre-Diciembre": return new DateTime(anio, 10, 1);
                default: return new DateTime();
            }
        }

        private DateTime armarFechaFin()
        {
            switch (trimestre)
            {
                case "Enero-Marzo": return new DateTime(anio, 3, 31);
                case "Abril-Junio": return new DateTime(anio, 6, 30);
                case "Julio-Septiembre": return new DateTime(anio, 9, 30);
                case "Octubre-Diciembre": return new DateTime(anio, 12, 31);
                default: return new DateTime();
            }
        }
    }
}
