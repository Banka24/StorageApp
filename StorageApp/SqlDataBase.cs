using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace StorageApp
{
    class SqlDataBase
    {
        private SqlConnection SqlConnection;
        public SqlDataBase()
        {
           SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["StorageDB"].ConnectionString);
        }

        public void ConnectionOpen()
        {
            if(SqlConnection.State == ConnectionState.Closed)
            {
                SqlConnection.Open();
            }
        }

        public void ConnectionClose()
        {
            if (SqlConnection.State == ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return SqlConnection;
        }
    }
}
