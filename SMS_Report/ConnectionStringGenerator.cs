using Cipher;
using System.Configuration;
using System.Data.SqlClient;

namespace SMS_Report
{
    public class ConnectionStringGenerator
    {
        private static string _serverAddress = ConfigurationManager.AppSettings["ServerAddress"];
        private static string _serverName = ConfigurationManager.AppSettings["ServerName"];
        private static string _databaseName = ConfigurationManager.AppSettings["DatabaseName"];
        private static string _userId = StringCipher.DecryptData("UserId");
        private static string _userPassword = StringCipher.DecryptData("UserPassword");

        public static SqlConnectionStringBuilder GenerateConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = _serverAddress + @"\" + _serverName,
                InitialCatalog = _databaseName,
                UserID = _userId,
                Password = _userPassword,
                ConnectTimeout = 5
            };

            return builder;
        }
    }
}
