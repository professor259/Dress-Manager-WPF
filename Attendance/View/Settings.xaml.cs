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
using Microsoft.Win32;
namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
          ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            var val = rk.GetValue(System.AppDomain.CurrentDomain.FriendlyName);
            if(val != null)
            {
                StartUp.IsChecked = true;
            }
            
        }

        private void StartUp_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
           ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (StartUp.IsChecked == true)
            {
                rk.SetValue(System.AppDomain.CurrentDomain.FriendlyName, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
            else
            {
                rk.DeleteValue(System.AppDomain.CurrentDomain.FriendlyName, false);
            }
        }
    }
}
