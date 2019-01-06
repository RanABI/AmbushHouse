using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Utils
{
    public static class Db_Utils
    {
        public static DataTable GetDataTable(string sql_query) {
            SqlConnection cn_Connection = get_Connection();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql_query, cn_Connection);
            adapter.Fill(table);

            return table;
        }

        public static SqlConnection get_Connection()
        {

            string cn_String = Properties.Settings.Default.connection_string;

            SqlConnection cn_Connection = new SqlConnection(cn_String);

            if (cn_Connection.State != System.Data.ConnectionState.Open)
                cn_Connection.Open();

            return cn_Connection;
        }

        public static void Execute_Sql(string sql_query)
        {
            SqlConnection cn_Connection = get_Connection();

            SqlCommand cmd = new SqlCommand(sql_query, cn_Connection);

            cmd.ExecuteNonQuery();
            
        }

        public static void closeConnection()
        {
            SqlConnection cn_Connection = get_Connection();
            cn_Connection.Close();
        }
    }
}
