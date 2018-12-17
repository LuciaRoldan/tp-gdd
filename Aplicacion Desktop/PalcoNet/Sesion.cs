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

        //Trae una instancia de la sesion
        public static Sesion getInstance()
        {
            if (sesion == null)
            {
                sesion = new Sesion();

            }
            return sesion;
        }

        //Trae los datos de empresa de la base relacionados con el usuario
        public Empresa traerEmpresa() {
            Empresa empresa = new Empresa();
            
            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarEmpresaPorUsername_sp '" + this.usuario.NombreUsuario + "'");
            
            Console.WriteLine("EXEC MATE_LAVADO.buscarEmpresaPorUsername_sp '" + this.usuario.NombreUsuario + "'");
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
                empresa.Id = int.Parse(reader["id_empresa"].ToString());

                //Falta traer telefono, localidad y ciudad?
            }
            return empresa;
        }

        //Trae los datos de cliente de la base relacionados con el usuario
        public Cliente traerCliente()
        {
            Cliente cliente = new Cliente();

            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.buscarClientePorUsername_sp '" + this.usuario.NombreUsuario + "'");

            while (reader.Read())
            {
                cliente.Calle = reader["calle"].ToString();
                cliente.CodigoPostal = reader["codigo_postal"].ToString();
                var cuil = reader["cuil"];
                if (!(cuil is DBNull)) { cliente.Cuil = Convert.ToInt64(cuil); }
                cliente.Departamento = reader["depto"].ToString();
                //cliente.FechaDeCreacion = (DateTime)reader["fecha_creacion"];
                cliente.NumeroDeCalle = Convert.ToInt32(reader["numero_calle"]);
                cliente.Mail = reader["mail"].ToString();
                cliente.Apellido = reader["apellido"].ToString();
                cliente.FechaDeNacimiento = (DateTime) reader["fecha_nacimiento"];
                cliente.Nombre = reader["nombre"].ToString();
                var doc = reader["documento"];
                if (!(doc is DBNull)) { cliente.NumeroDeDocumento = Convert.ToInt64(doc); }
                //cliente.NumeroDeDocumento = Convert.ToInt64(reader["documento"]);
                var tel = reader["telefono"];
                if (!(tel is DBNull)) cliente.Telefono = Convert.ToInt64(tel);
                cliente.TipoDocumento = reader["tipo_documento"].ToString();
                cliente.Piso = Convert.ToInt32(reader["piso"]);
                cliente.Id = int.Parse(reader["id_cliente"].ToString());
            }

            SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.getPuntos_sp '" + sesion.usuario.NombreUsuario + "', '" + Sesion.getInstance().fecha.ToString("yyyy-MM-dd hh:mm:ss.fff") + "' ");

            reader2.Read();
            cliente.Puntos = Convert.ToInt32(reader2["cantidad_puntos"]);

            return cliente;
        }
    }
}
