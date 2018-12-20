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
        //Se inicializa en -1 para traer la primera pagina con un offset de 0
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

        //Verifica que los campos esten completos
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
            //Si los campos estan completos abre el listado de clientes con mayor cantidad de compras, si no advierte al usuario con un error
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
            //Si los campos estan completos abre el listado de empresas con mayor cantidad de localidades no vendidas, 
            //si no advierte al usuario con un error
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
            //Si los campos estan completos abre el listado de clientes con mayor cantidad de puntos vencidos, 
            //si no advierte al usuario con un error
            if (this.verificarCampos())
            {
                DateTime inicio = armarFechaInicio();
                DateTime fin = armarFechaFin();

                //Para este caso como no se requiere informacion adicional se realiza la busqueda en la base y se le pasa la lista de resultados a la pantalla
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.top5ClientesPuntosVencidos_sp '" + inicio.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '"
                    + fin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
                List<ClientePuntosListado> clientesOrdenadosPorPuntos = new List<ClientePuntosListado>();
                Console.WriteLine("EXEC MATE_LAVADO.top5ClientesPuntosVencidos_sp '" + inicio.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '"
                    + fin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
                while (reader.Read())
                {
                    ClientePuntosListado cliente = new ClientePuntosListado();
                    cliente.Nombre = reader["nombre"].ToString();
                    cliente.Apellido = reader["apellido"].ToString();
                    cliente.Puntos = int.Parse(reader["Puntos Vencidos"].ToString());

                    clientesOrdenadosPorPuntos.Add(cliente);
                }
                reader.Close();

                new ClientesPuntos(clientesOrdenadosPorPuntos, this).Show();
                this.Hide();
            }
            else {
                MessageBox.Show("Se deben seleccionar el año y el trimestre", "Error", MessageBoxButtons.OK);
            }
        }

        private void anioCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Se actualiza el valor del año seleccionado
            anio = Int32.Parse(anioCombobox.Text.ToString());
        }


        private void trimestreCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Se actualiza el valor del trimestre seleccionado
            trimestre = trimestreCombobox.Text;
        }

        private DateTime armarFechaInicio()
        {
            //Se devuelve la fecha de inicio segun el trimestre seleccionado
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
            //Se devuelve la fecha de fin segun el trimestre seleccionado
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
