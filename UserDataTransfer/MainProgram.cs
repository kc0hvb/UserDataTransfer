using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDataTransfer
{
    class MainProgram
    {
        private DataTable dDataTable = new DataTable();
        public void RunMainProgram(string pConnStringSource, string pConnStringTarget)
        {
            
        }
        public void PullData(string pConnStringSource, string pConnStringTarget)
        {
            string connString = pConnStringSource;
            string query = "select * from table";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dDataTable);
            conn.Close();
            da.Dispose();
        }

    }
}
