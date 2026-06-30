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
using System.Runtime.InteropServices;
namespace Attendance.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void AllocConsole();
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        public Login()
        {
            InitializeComponent();           
        }
        private void OnStartUp(Object sender, StartupEventArgs e)
        {

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        public string[] Files
        { get; set; }

        private void Background_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                // Background.Position = TimeSpan.FromMilliseconds(1);
            });
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public static bool Checked = false;
        private void AutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            if (Checked)
            {
                this.AutoLogin.IsChecked = false;
            }
            else
            {
                Checked = false; this.AutoLogin.IsChecked = true;
            }
        }
        private bool JustChecked;
        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            JustChecked = true;
        }

        private void RB_Clicked(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            if (this.AutoLogin.IsChecked == true)
                this.AutoLogin.IsChecked = false;
        }
        private void ShowPasswordCharsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Password.Visibility = System.Windows.Visibility.Hidden;
            MyTextBox.Visibility = System.Windows.Visibility.Visible;
            MyTextBox.Text = Password.Password;
            MyTextBox.Focus();
        }
        private void ShowPasswordCharsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Password.Visibility = System.Windows.Visibility.Visible;
            MyTextBox.Visibility = System.Windows.Visibility.Hidden;

            Password.Focus();
        }
        public void Launch()
        {
            this.Dispatcher.Invoke(() =>
            {
                //Status.Content = "Launching....";
            });
        }
        public void ProcessHide()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Hide();
            });
        }
        public void Secure()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    //if (IsEnabled)
                    {
                        // Status.Content = "Encrypting";
                        //   Status.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void Done()
        {
            this.Dispatcher.Invoke(() =>
            {
                // Status.Content = "Connected";
                // Status.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            });
            System.Threading.Thread.Sleep(2000);
        }
        public void TryToLogin()
        {
            this.Dispatcher.Invoke(() =>
            {
                LoginB.Content = "Logging...";
                LoginB.IsEnabled = false;
            });
        }
        public void ApplyCred()
        {
            this.Dispatcher.Invoke(() =>
            {
                LoginB.Content = "Logging...";
                UserName.BorderBrush = System.Windows.Media.Brushes.Green;
                Password.BorderBrush = System.Windows.Media.Brushes.Green;
                UserText.Visibility = Visibility.Hidden;
                PassText.Visibility = Visibility.Hidden;
                AutoLogin.Visibility = Visibility.Hidden;
                LoginB.Visibility = Visibility.Hidden;
                UserName.Visibility = Visibility.Hidden;
                Password.Visibility = Visibility.Hidden;
                ShowPasswordCharsCheckBox.Visibility = Visibility.Hidden;
                LoadGif.Visibility = Visibility.Visible;
                //Status.Visibility = Visibility.Visible;

            });
        }
        public void InvaildCred()
        {
            this.Dispatcher.Invoke(() =>
            {
                LoginB.IsEnabled = true;
                LoginB.Content = "Log-In";
                UserName.BorderBrush = System.Windows.Media.Brushes.Red;
                Program.MyApp.ShowBox("Invaild Username!");
            });
        }
        public void InvaildPassword()
        {
            this.Dispatcher.Invoke(() =>
            {
                LoginB.IsEnabled = true;
                LoginB.Content = "Log-In";
                UserName.BorderBrush = System.Windows.Media.Brushes.Green;
                Password.BorderBrush = System.Windows.Media.Brushes.Red;
                Program.MyApp.ShowBox("Invaild Password!");
            });
        }
        public void CustomInValid(string MessageB)
        {
            this.Dispatcher.Invoke(() =>
            {
                LoginB.IsEnabled = true;
                LoginB.Content = "Log-In";
                UserName.BorderBrush = System.Windows.Media.Brushes.Red;
                Password.BorderBrush = System.Windows.Media.Brushes.Red;
                /*MessageBoxResult result = MessageBox.Show(MessageB, "Credentials", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                   // System.Windows.Application.Current.Shutdown();

                }*/
            });
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (this.UserName.Text != "")
                {
                    if (this.Password.Password != "")
                    {
                        //if (Program.IsConnectedToInternet())
                        {
                            Program.MyApp.AuthenticateTask(this.UserName.Text,this.Password.Password);
                        }
                       // else
                        {
                          //  Program.MyApp.ShowBox("No Internet Connection");
                        }
                    }
                    else
                    {
                        Program.MyApp.ShowBox("Please enter your password first");
                    }
                }
                else
                {
                    Program.MyApp.ShowBox("Please enter username first");
                }
            });
        }

        private void Background_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
