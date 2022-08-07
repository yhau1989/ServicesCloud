using Microsoft.Extensions.Configuration;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Clase que maneja la persistencia con la base de oracle
    /// </summary>
    public class DbOracleContext
    {
        private string cConexion, nameApp = "";
        private string ConnectionString;
        IConfiguration config;
        private OracleConnection connection = null;
        private Logger logger = null;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public DbOracleContext()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            cConexion = config.GetValue<string>("AppSettings:ConnectionStringOracle");

            //desencriptando cadena
            cConexion = testMD5.decrypt(cConexion);
            //desencriptando cadena

            //nameApp = config.GetValue<string>("nameApp");
            //logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        }


        /// <summary>
        /// ejecuta el sp sp_scloud_creditosora
        /// </summary>
        /// <param name="emisor">codigo de emisor</param>
        /// <param name="numSolicitude">numero de solicitud</param>
        /// <param name="client">ci del cliente</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg getCredits(string emisor, string numSolicitude, string client)
        {
            ResponseMsg rsp;
            try
            {
                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "sp_scloud_creditosora";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("l_emisor", OracleDbType.BinaryFloat, emisor, ParameterDirection.Input);
                command.Parameters.Add("l_numsoli", OracleDbType.BinaryFloat, numSolicitude, ParameterDirection.Input);
                command.Parameters.Add("l_cliente", OracleDbType.Varchar2, client, ParameterDirection.Input);
                command.Parameters.Add("retorno", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable table = new DataTable();

                OracleDataReader dataReader = command.ExecuteReader();
                table.Load(dataReader);

                command.Dispose();
                connection.Close();

                if (table.Rows.Count > 0)
                {
                    rsp = new ResponseMsg()
                    {
                        status = 0,
                        msg = "ok",
                        data = table
                    };
                }
                else
                {
                    rsp = new ResponseMsg()
                    {
                        status = 1,
                        msg = $"ORACLE:sp_scloud_creditosora no registra datos para los parametros recibidos",
                    };
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                rsp = new ResponseMsg() { status = 99, msg = $"ORACLE:sp_scloud_creditosora {ex.Message}"};
            }

            return rsp;

        }

    }
}
