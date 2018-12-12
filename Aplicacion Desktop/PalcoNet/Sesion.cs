using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Dominio;
using System.Configuration;
using System.Data.SqlClient;

namespace PalcoNet
{
    class Sesion
    {
        public Rol rol { get; set; }
        public Usuario usuario { get; set; }
        public DateTime fecha = DateTime.ParseExact(ConfigurationManager.AppSettings["horarioSistema"], "yyyy-dd-MM HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

        public static Sesion sesion { get; set; }

        public static Sesion getInstance()
        {
            if (sesion == null)
            {
                sesion = new Sesion();

            }
            return sesion;
        }

        public Empresa traerEmpresa() {
            Empresa empresa = new Empresa();
            
            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC buscarEmpresaPorUsername_sp '" + this.usuario.NombreUsuario + "'");
            
            Console.WriteLine("EXEC buscarEmpresaPorUsername_sp '" + this.usuario.NombreUsuario + "'");
            while (reader.Read())
            {
                empresa.Calle = reader["calle"].ToString();
                empresa.CodigoPostal = reader["codigo_postal"].ToString();
                empresa.Cuit = Int64.Parse(reader["cuit"].ToString());
                empresa.Departamento = reader["depto"].ToString();
                empresa.FechaDeCreacion = (DateTime) reader["fecha_creacion"];
                empresa.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                empresa.Mail = reader["mail"].ToString();
                empresa.RazonSocial = reader["razon_social"].ToString();

                //Falta traer telefono, localidad y ciudad?
            }
            return empresa;
        }

        public Cliente traerCliente()
        {
            Cliente cliente = new Cliente();

            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC buscarClientePorUsername_sp '" + this.usuario.NombreUsuario + "'");

            while (reader.Read())
            {
                cliente.Calle = reader["calle"].ToString();
                cliente.CodigoPostal = reader["codigo_postal"].ToString();
                var cuil = reader["cuil"];
                if (!(cuil is DBNull)) cliente.Cuil = Convert.ToInt32(cuil);
                cliente.Departamento = reader["depto"].ToString();
                //cliente.FechaDeCreacion = (DateTime)reader["fecha_creacion"];
                cliente.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                cliente.Mail = reader["mail"].ToString();
                cliente.Apellido = reader["apellido"].ToString();
                //cliente.FechaDeNacimiento = (DateTime) reader["fecha_nacimiento"];
                cliente.Nombre = reader["nombre"].ToString();
                cliente.NumeroDeDocumento = Convert.ToInt32(reader["documento"]);
                cliente.TipoDocumento = reader["tipo_documento"].ToString();
                cliente.Piso = Convert.ToInt32(reader["piso"]);
                cliente.Id = int.Parse(reader["id_cliente"].ToString());
                Console.WriteLine("converti cosas");
            }

            SqlDataReader reader2 = servidor.query("EXEC dbo.getPuntos_sp '" + sesion.usuario.NombreUsuario + "', '" + Sesion.getInstance().fecha + "' ");

            Console.WriteLine("traje puntos");

            reader2.Read();
            cliente.Puntos = Convert.ToInt32(reader2["cantidad_puntos"]);

            Console.WriteLine("mmms");

            Console.WriteLine(cliente.Puntos);

            return cliente;
        }
    }
}
