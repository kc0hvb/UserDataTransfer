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
            InitializeComponent();
            settingValues();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            
            SavingInformationInConfig();
            MaPro.RunMainProgram(sConnSource, sConnTarget);
        }

        
        private void settingValues()
        {
            try
            {
                Configuration config = MaPro.ConfigLocation();
                tbSourceSQLServerDatabase.Text = config.AppSettings.Settings["sSourceSQLDatbase"].Value;
                tbSourceSQLServerLocation.Text = config.AppSettings.Settings["sSourceSQLLocation"].Value;
                tbSourceSQLUsername.Text = config.AppSettings.Settings["sSourceSQLUsername"].Value;
                tbTargetSQLDatabase.Text = config.AppSettings.Settings["sDestinationSQLDatabase"].Value;
                tbTargetSQLServerLocation.Text = config.AppSettings.Settings["sDestinationSQLLocation"].Value;
                tbTargetSQLUsername.Text = config.AppSettings.Settings["sDestinationSQLUsername"].Value;
                if (config.AppSettings.Settings["sSourceSQLPassword"].Value != "") pbSourceSQLPassword.Password = MaPro.Decrypt(config.AppSettings.Settings["sSourceSQLPassword"].Value);
                if (config.AppSettings.Settings["sDestinationSQLPassword"].Value != "") pbTargetSQLPassword.Password = MaPro.Decrypt(config.AppSettings.Settings["sDestinationSQLPassword"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string ConfigSettings(string pToFrom)
        {
            string sSourceSQLLocation = tbSourceSQLServerLocation.Text.ToString();
            string sSourceSQLDatbase = tbSourceSQLServerDatabase.Text.ToString();
            string sSourceSQLUsername = tbSourceSQLUsername.Text.ToString();
            string sSourceSQLPassword = pbSourceSQLPassword.Password.ToString();
            string sDestinationSQLLocation = tbTargetSQLServerLocation.Text.ToString();
            string sDestinationSQLDatabase = tbTargetSQLDatabase.Text.ToString();
            string sDestinationSQLUsername = tbTargetSQLUsername.Text.ToString();
            string sDestinationSQLPassword = pbTargetSQLPassword.Password.ToString();
            if (pToFrom.Contains("source"))
            {
                string sConnSource = $"Server = {sSourceSQLLocation}; Database = {sSourceSQLDatbase}; User Id = {sSourceSQLUsername}; Password = {sSourceSQLPassword};";
            }
            string sConnTarget = $"Server = {sDestinationSQLLocation}; Database = {sDestinationSQLDatabase}; User Id = {sDestinationSQLUsername}; Password = {sDestinationSQLPassword};";
        }

        public void SavingInformationInConfig()
        {
            Configuration config = MaPro.ConfigLocation();

            if (tbSourceSQLServerDatabase.Text != "") config.AppSettings.Settings["sSourceSQLDatbase"].Value = tbSourceSQLServerDatabase.Text.ToString();
            if (tbSourceSQLServerLocation.Text != "") config.AppSettings.Settings["sSourceSQLLocation"].Value = tbSourceSQLServerLocation.Text.ToString();
            if (tbSourceSQLUsername.Text != "") config.AppSettings.Settings["sSourceSQLUsername"].Value = tbSourceSQLUsername.Text.ToString();
            if (tbTargetSQLDatabase.Text != "") config.AppSettings.Settings["sDestinationSQLDatabase"].Value = tbTargetSQLDatabase.Text.ToString();
            if (tbTargetSQLServerLocation.Text != "") config.AppSettings.Settings["sDestinationSQLLocation"].Value = tbTargetSQLServerLocation.Text.ToString();
            if (tbTargetSQLUsername.Text != "") config.AppSettings.Settings["sDestinationSQLUsername"].Value = tbTargetSQLUsername.Text.ToString();
            if (pbSourceSQLPassword.Password != "")
                config.AppSettings.Settings["sSourceSQLPassword"].Value = MaPro.Encrypt(pbSourceSQLPassword.Password.ToString());
            if (pbTargetSQLPassword.Password != "")
                config.AppSettings.Settings["sDestinationSQLPassword"].Value = MaPro.Encrypt(pbSourceSQLPassword.Password.ToString()); 
            config.Save();
        }

        private string GenerateSaltedHash(string sInputString)
        {
            byte[] data = Encoding.ASCII.GetBytes(sInputString);
            data = new SHA256Managed().ComputeHash(data);
            String hash = Encoding.ASCII.GetString(data);
            return hash;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
