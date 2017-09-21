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
using System.Security.Cryptography;

namespace UserDataTransfer
{
    class MainProgram
    {
        private DataTable dDataTableSource = new DataTable();
        private DataTable dDataTableTarget = new DataTable();
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

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

        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public Configuration ConfigLocation()
        {

            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFile = Path.Combine(appPath, "UserDataTransfer.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            return config;
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
