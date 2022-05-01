using System.Data.Odbc;

namespace DbSybaseService
{
    /// <summary>
    /// CONEXIÓN A BASE SYBASE
    /// </summary>
    /// 
    public class DbSybaseServiceOdbc
    {
        public ConnectionInfo ConnectionInfo { get; set; }
        public string ConnectionString { get; }

        public OdbcConnection connection = new OdbcConnection();


        public DbSybaseServiceOdbc(string stringCnx)
        {

            ConnectionInfo connection = new ConnectionInfo();
            connection.ConnectionString = stringCnx;          
            this.ConnectionString = connection.ConnectionString;
            ConnectionInfo = connection;
        }


        public System.Data.ConnectionState Status
        {
            get
            {
                if (connection != null)
                    return connection.State;
                else
                    return System.Data.ConnectionState.Closed;
            }
        }

        public void Initialize()
        {
            try
            {

                connection.ConnectionString = this.ConnectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Dispose();

                throw ex;
            }

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }

}
public class ConnectionInfo
{
    public string? ConnectionString { get; internal set; }
    public string? Password { get; internal set; }
    public string? UserId { get; internal set; }
}