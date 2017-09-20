using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UserDataTransfer
{
    class MainProgram
    {
        private DataTable dDataTableSource = new DataTable();
        private DataTable dDataTableTarget = new DataTable();

        public Dictionary<string, string> PullValuesFromConfig()
        {
            try
            {
                Dictionary<string, string> dValuesFromConfig = new Dictionary<string, string>();
                string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string configFile = Path.Combine(appPath, "UserDataTransfer.exe.config");
                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                configFileMap.ExeConfigFilename = configFile;
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                dValuesFromConfig.Add("sSourceSQLDatbase", config.AppSettings.Settings["sSourceSQLDatbase"].Value);
                dValuesFromConfig.Add("sSourceSQLLocation", config.AppSettings.Settings["sSourceSQLLocation"].Value);
                dValuesFromConfig.Add("sSourceSQLUsername", config.AppSettings.Settings["sSourceSQLUsername"].Value);
                dValuesFromConfig.Add("sDestinationSQLDatabase", config.AppSettings.Settings["sDestinationSQLDatabase"].Value);
                dValuesFromConfig.Add("sDestinationSQLLocation", config.AppSettings.Settings["sDestinationSQLLocation"].Value);
                dValuesFromConfig.Add("sDestinationSQLUsername", config.AppSettings.Settings["sDestinationSQLUsername"].Value);
                return dValuesFromConfig;
            }
            catch
            { return null; }
        }
        
        public void RunMainProgram(string pConnStringSource, string pConnStringTarget)
        {
            dDataTableSource = PullData(pConnStringSource);
            dDataTableTarget = PullData(pConnStringTarget);
        }
        public DataTable PullData(string pConnString)
        {
            DataTable datatable = new DataTable();
            string query = @"SELECT * FROM Users, Tabs, TabFields, Filters, FilterItems
                             WHERE Users.User_ID = Tabs.User_ID AND Tabs.Tab_ID = TabFields.Tab_ID AND
                             Users.User_ID = Filters.User_ID AND Filters.Filter_ID = FilterItems.Filter_ID";

            SqlConnection conn = new SqlConnection(pConnString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(datatable);
            conn.Close();
            return datatable;
        }
    }
}
