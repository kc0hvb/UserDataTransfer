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
        private DataTable dDataTableSource = new DataTable();
        private DataTable dDataTableTarget = new DataTable();
        public void RunMainProgram(string pConnStringSource, string pConnStringTarget)
        {
            
        }
        public void PullData(string pConnStringSource, string pConnStringTarget)
        {
            string query = "select * from table";

            SqlConnection connSource = new SqlConnection(pConnStringSource);
            SqlCommand cmdSource = new SqlCommand(query, connSource);
            connSource.Open();

            SqlConnection connTarget = new SqlConnection(pConnStringTarget);
            SqlCommand cmdTarget = new SqlCommand(query, connTarget);
            connTarget.Open();

            // create data adapter
            SqlDataAdapter daSource = new SqlDataAdapter(cmdSource);
            // this will query your database and return the result to your datatable
            daSource.Fill(dDataTableSource);
            connSource.Close();
            daSource.Dispose();

            SqlDataAdapter daTarget = new SqlDataAdapter(cmdTarget);
            // this will query your database and return the result to your datatable
            daTarget.Fill(dDataTableTarget);
            connTarget.Close();
            daTarget.Dispose();
        }

    }
}
