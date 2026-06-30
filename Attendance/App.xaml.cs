using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Microsoft.Win32;
namespace Attendance
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static WPF.Login AuthWindow;
        public static WPF.Login Application;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args != null && e.Args.Count() > 0)
            {
                Program.LaunchedViaStartup = true;
            }
            Database.DB.ClearDbFolder();
            /*string fileEnd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance.db.enc");
            string fileIn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance.db");
            DbProtector.EncryptFile(fileIn, fileEnd);*/
         //   Database.Connection.CreateConnection("attendance", "newPass", "attendance", "54.39.202.236");
            DateTime Now = DateTime.Now;
            this.Dispatcher.Invoke(() =>
            {
                AuthWindow = new WPF.Login();
                AuthWindow.LoginText.Visibility = Visibility.Hidden;
                AuthWindow.UserText.Visibility = Visibility.Hidden;
                AuthWindow.PassText.Visibility = Visibility.Hidden;
                AuthWindow.AutoLogin.Visibility = Visibility.Hidden;
                AuthWindow.LoginB.Visibility = Visibility.Hidden;
                AuthWindow.UserName.Visibility = Visibility.Hidden;
                AuthWindow.Password.Visibility = Visibility.Hidden;
                AuthWindow.ShowPasswordCharsCheckBox.Visibility = Visibility.Hidden;
                AuthWindow.Show();
                this.Exit += new ExitEventHandler(OnExitHandler);
                Program.MyApp = this;
            });

            ShowAuth();
        }
        public void Launch()
        {
            if (Program.Application == null)
            {
                AuthWindow.Launch();
                this.Dispatcher.Invoke(() =>
                {

                    Thread newWindowThread = new Thread(new ThreadStart(() =>
                    {
                        Program.Application = new WPF.GUI();
                        if(!Program.Application.HideGUI())
                        Program.Application.Show();
                        AuthWindow.ProcessHide();
                        System.Windows.Threading.Dispatcher.Run();
                    }));
                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();
                });
            }
        }
        public bool IsAuth()
        {
            return AuthWindow.IsEnabled;
        }
        public void Secure()
        {
            AuthWindow.Secure();
        }
        public void Done()
        {
            AuthWindow.Done();
        }
        public void ApplyCred()
        {
            AuthWindow.ApplyCred();
            this.Dispatcher.Invoke(() =>
            {
                if (Program.Application == null)
                {
                    if (AuthWindow.AutoLogin.IsChecked == true)
                    {
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\AttSys", "Username", AuthWindow.UserName.Text);
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\AttSys", "Password", AuthWindow.Password.Password);
                    }
                    Program.Username = AuthWindow.UserName.Text;
                    Program.Password = AuthWindow.Password.Password;
                }

                Launch();

            });
            /*this.Dispatcher.Invoke(() =>
            {               
                Launch();

            });*/
        }
        public void InvaildCred()
        {
            AuthWindow.InvaildCred();
        }
        public void OnExitHandler(object sender, ExitEventArgs e)
        {
            //Program.Shutdown();
        }
        public void ExitAll()
        {
            this.Dispatcher.Invoke(() =>
            {
                AuthWindow.Close();
                //  System.Windows.Application.Current.Shutdown();
                System.Environment.Exit(0);
            });
        }
        public void CustomMessage(string Message)
        {
            SendScreenAuth();
            AuthWindow.CustomInValid(Message);
            ShowBox(Message);
        }
        public void InvaildPassword()
        {
            AuthWindow.InvaildPassword();
        }
        public void ShowLoginScreen()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (AuthWindow.LoginText.Visibility == Visibility.Visible)
                {
                    AuthWindow.LoginText.Visibility = Visibility.Hidden;
                    AuthWindow.LoadGif.Visibility = Visibility.Hidden;
                    //AuthWindow.Status.Visibility = Visibility.Hidden;
                    AuthWindow.UserText.Visibility = Visibility.Visible;
                    AuthWindow.PassText.Visibility = Visibility.Visible;
                    AuthWindow.AutoLogin.Visibility = Visibility.Visible;
                    AuthWindow.LoginB.Visibility = Visibility.Visible;
                    AuthWindow.UserName.Visibility = Visibility.Visible;
                    AuthWindow.Password.Visibility = Visibility.Visible;
                    AuthWindow.ShowPasswordCharsCheckBox.Visibility = Visibility.Visible;
                }
            });
        }
        public void SendScreenAuth()
        {
            this.Dispatcher.Invoke(() =>
            {
                AuthWindow.LoginText.Visibility = Visibility.Hidden;
                AuthWindow.LoadGif.Visibility = Visibility.Hidden;
                // AuthWindow.Status.Visibility = Visibility.Hidden;
                AuthWindow.UserText.Visibility = Visibility.Visible;
                AuthWindow.PassText.Visibility = Visibility.Visible;
                AuthWindow.AutoLogin.Visibility = Visibility.Visible;
                AuthWindow.LoginB.Visibility = Visibility.Visible;
                AuthWindow.UserName.Visibility = Visibility.Visible;
                AuthWindow.Password.Visibility = Visibility.Visible;
                AuthWindow.ShowPasswordCharsCheckBox.Visibility = Visibility.Visible;
            });
        }
        public void ShowBox(string msg)
        {
            this.Dispatcher.Invoke(() =>
            {
                WPF.MyBox box = new WPF.MyBox();
                box.WriteLine(msg);               
                box.Activate();
                box.Topmost = true;
                box.ShowDialog();
            });
        }
        public async Task AuthenticateTask(string username, string password)
        {
            await Task.Run(() => {
                Authenticate(username,password);
            });          
        }
        public bool Authenticate(string username,string password)
        {           
            AuthWindow.TryToLogin();
            Program.Account = new Database.AccountTable(username);           
            if (Program.Account.exists)
            {
                if (Program.Account.VerifyPassword(password))
                {
                    Database.DB.LoadAdmins();
                    Database.DB.LoadDress();
                    Database.DB.LoadReservation();
                    Database.DB.LoadPurchase();
                    /*Database.DB.LoadClasses();
                    Database.DB.LoadAttendance();
                    Database.DB.LoadMaterials();*/
                    ApplyCred();
                    return true;
                }
                else
                {
                    InvaildPassword();
                }
            }
            else
            {
                InvaildCred();
            }
            return false;
        }
        public void ShowAuth()
        {
            try
            {
                Console.WriteLine("ShowAuth");
                try
                {
                    object regValue = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\AttSys",
                                    "Username", null);
                    if (regValue != null)
                    {
                        Program.Username = regValue.ToString();
                        this.Dispatcher.Invoke(() =>
                        {
                            AuthWindow.UserName.Text = Program.Username;

                        });
                    }
                    else
                    {
                        SendScreenAuth();
                        return;
                    }
                }
                catch
                {
                    SendScreenAuth();
                    return;
                }
                if (Program.Username == "NULL")
                {
                    SendScreenAuth();
                }
                else
                {
                    Program.Password = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\AttSys",
                               "Password", "NULL").ToString();
                    if (Program.Password != null)
                    {
                        this.Dispatcher.Invoke(() =>
                {
                    AuthWindow.Password.Password = Program.Password;
                });
                        Console.WriteLine("Process Login");
                        /*if (!Program.IsConnectedToInternet())
                        {
                            WPF.MyBox box = new WPF.MyBox();
                            box.WriteLine("No Internet Connection", true);
                            box.ShowDialog();
                            box.Activate();
                            box.Topmost = true;
                        }*/
                        AuthenticateTask(Program.Username,Program.Password);
                    }
                    else
                    {
                        SendScreenAuth();
                    }
                }               
            }
            catch (Exception e)
            {
            }
        }
    }
}
