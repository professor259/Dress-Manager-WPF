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
using System.Windows.Shapes;

namespace Attendance.WPF
{
    /// <summary>
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class GUI : Window
    {
        public ViewModel.NavigationVM Navigation = new ViewModel.NavigationVM();
        public View.Reserve ReservationWindow;
        public View.AttendanceMenu AttendanceWindow;
        public View.Search SearchWindow;
        public View.Admins AdminsWindow;
        public View.Dress DressManager;
        public View.Materials MateirlasWindow;
        public View.Purchases PurchaseWindow;
        public bool HideGUI()
        {
            return Program.LaunchedViaStartup;
        }
        public GUI()
        {
            InitializeComponent();
            Program.PurchaseType.Add(0, "اكل");
            Program.PurchaseType.Add(1, "مصروف");
            Program.PurchaseType.Add(2, "مرتب موظف");
            Program.PurchaseType.Add(3, "قماش");
            Program.PurchaseType.Add(4, "ايجار محل");          
            Program.PurchaseType.Add(5, "تصميم فستان");           
            if (Program.Account.State != 1)
            {
                AdminBtn.Visibility = Visibility.Collapsed;
                DressBtn.Visibility = Visibility.Collapsed;
                ReportBtn.Visibility = Visibility.Collapsed;
                AdminBtn.Visibility = Visibility.Collapsed;
            }
            Program.Application = this;
            if (HideGUI())
            {
                WindowState = WindowState.Minimized;
                Hide();
                using (System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon())
                {
                    ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.FriendlyName);
                    ni.Visible = true;
                    ni.DoubleClick +=
                              delegate (object sender, EventArgs args)
                              {
                                  this.Show();
                                  this.WindowState = WindowState.Normal;
                                  ni.Visible = false;
                              };
                }
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();                
            }
        }
        public void HandleSize()
        {
            if (IsMaxmized)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1080;
                this.Height = 720;
                IsMaxmized = false;                
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                IsMaxmized = true;                
            }
        }
        public bool IsMaxmized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                HandleSize();
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            string keyName = @"SOFTWARE\AttSys";
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                if (key == null)
                {                    
                }
                else
                {
                    key.DeleteValue("Username");
                    key.DeleteValue("Password");
                    Environment.Exit(0);
                }
            }
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
