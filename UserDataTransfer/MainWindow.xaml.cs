using System;
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string sSourceSQLLocation = tbSourceSQLServerLocation.Text.ToString();
            string sSourceSQLDatbase = tbSourceSQLServerDatabase.Text.ToString();
            string sSourceSQLUsername = tbSourceSQLUsername.Text.ToString();
            string sSourceSQLPassword = tbSourceSQLPassword.Text.ToString();
            string sDestinationSQLLocation = tbTargetSQLServerLocation.Text.ToString();
            string sDestinationSQLDatabase = tbTargetSQLDatabase.Text.ToString();
            string sDestinationSQLUsername = tbTargetSQLUsername.Text.ToString();
            string sDestinationSQLPassword = tbTargetSQLPassword.Text.ToString();
            string sConnSource = "";
            string sConnTarget = "";
            MaPro.RunMainProgram(sConnSource, sConnTarget);
        }
    }
}
