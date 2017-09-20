using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Security.Cryptography;

namespace UserDataTransfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainProgram MaPro = new MainProgram();
        public MainWindow()
        {
            Dictionary<string, string> dictionary = MaPro.PullValuesFromConfig();
            if (dictionary["sSourceSQLDatbase"] != "") tbSourceSQLServerDatabase.Text = dictionary["sSourceSQLDatbase"].ToString();
            if (dictionary["sSourceSQLLocation"] != "") tbSourceSQLServerLocation.Text = dictionary["sSourceSQLLocation"].ToString();
            if (dictionary["sSourceSQLUsername"] != "") tbSourceSQLUsername.Text = dictionary["sSourceSQLUsername"].ToString();
            if (dictionary["sDestinationSQLDatabase"] != "") tbTargetSQLDatabase.Text = dictionary["sDestinationSQLDatabase"].ToString();
            if (dictionary["sDestinationSQLLocation"] != "") tbTargetSQLServerLocation.Text = dictionary["sDestinationSQLLocation"].ToString();
            if (dictionary["sDestinationSQLUsername"] != "") tbTargetSQLUsername.Text = dictionary["sDestinationSQLUsername"].ToString();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string sSourceSQLLocation = tbSourceSQLServerLocation.Text.ToString();
            string sSourceSQLDatbase = tbSourceSQLServerDatabase.Text.ToString();
            string sSourceSQLUsername = tbSourceSQLUsername.Text.ToString();
            string sSourceSQLPassword = pbSourceSQLPassword.Password.ToString();
            string sDestinationSQLLocation = tbTargetSQLServerLocation.Text.ToString();
            string sDestinationSQLDatabase = tbTargetSQLDatabase.Text.ToString();
            string sDestinationSQLUsername = tbTargetSQLUsername.Text.ToString();
            string sDestinationSQLPassword = pbTargetSQLPassword.Password.ToString();
            string sConnSource = $"Server = {sSourceSQLLocation}; Database = {sSourceSQLDatbase}; User Id = {sSourceSQLUsername}; Password = {sSourceSQLPassword};";
            string sConnTarget = $"Server = {sDestinationSQLLocation}; Database = {sDestinationSQLDatabase}; User Id = {sDestinationSQLUsername}; Password = {sDestinationSQLPassword};";
            
            SavingInformationInConfig();
            //MaPro.RunMainProgram(sConnSource, sConnTarget);
        }

        public void SavingInformationInConfig()
        {
            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, "UserDataTransfer.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            if (tbSourceSQLServerDatabase.Text != "") config.AppSettings.Settings["sSourceSQLDatbase"].Value = tbSourceSQLServerDatabase.Text.ToString();
            if (tbSourceSQLServerLocation.Text != "") config.AppSettings.Settings["sSourceSQLLocation"].Value = tbSourceSQLServerLocation.Text.ToString();
            if (tbSourceSQLUsername.Text != "") config.AppSettings.Settings["sSourceSQLUsername"].Value = tbSourceSQLUsername.Text.ToString();
            if (tbTargetSQLDatabase.Text != "") config.AppSettings.Settings["sDestinationSQLDatabase"].Value = tbTargetSQLDatabase.Text.ToString();
            if (tbTargetSQLServerLocation.Text != "") config.AppSettings.Settings["sDestinationSQLLocation"].Value = tbTargetSQLServerLocation.Text.ToString();
            if (tbTargetSQLUsername.Text != "") config.AppSettings.Settings["sDestinationSQLUsername"].Value = tbTargetSQLUsername.Text.ToString();
            config.Save();
        }

        private string GenerateSaltedHash(string sInputString)
        {
            byte[] data = Encoding.ASCII.GetBytes(sInputString);
            data = new SHA256Managed().ComputeHash(data);
            String hash = Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}
