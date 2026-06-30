using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for AddAdmin.xaml
    /// </summary>
    public partial class AddAdmin: UserControl
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // allows only digits
            e.Handled = regex.IsMatch(e.Text);
        }
        public AddAdmin(bool editmode = false, Model.AdminsModel admin = null)
        {
            InitializeComponent();
            this.EditMode = editmode;
            if (editmode)
            {
                this.Username.Text = admin.Username;
                this.Username.IsEnabled = false;
                this.Password.Text = admin.Password;
                this.Email.Text = admin.Email;
                this.Phone.Text = admin.Phone;
                this.AdminID.Text = admin.ID;
                this.AdminID.IsEnabled = false;
                this.FullName.Text = admin.Name;
                this.AddBtn.Content = "Edit";
                this.ShowBtn.Content = "Edit Admin";
            }
        }
        public bool EditMode = false;
        public async Task AddAdminMember()
        {
            await Task.Run(() =>
            {
                {
                    if (!EditMode)
                    {
                        if (Database.DB.VerifyAdminUsername(UsernameContext))
                        {

                            if (Database.DB.VerifyAdminID(AdminIDContext))
                            {
                                Database.DB.AddAdmin(UsernameContext, NameContext, PasswordContext, EmailContext, PhoneContext, AdminIDContext,(byte)AdminLevelContext);
                                Database.DB.AddAdminCollection(UsernameContext, PasswordContext, AdminIDContext, EmailContext, NameContext, PhoneContext, (byte)AdminLevelContext);
                                Program.MyApp.ShowBox("Admin has been added!");
                                Program.Application.AdminsWindow.ShowPageRaws();
                                this.Dispatcher.Invoke(() =>
                                {                                  
                                    Window parentWindow = (Window)this.Parent;
                                    parentWindow.Close();
                                });
                            }
                            else
                            {
                                Program.MyApp.ShowBox("AdminID already exists!");
                                Reset();
                            }
                        }
                        else
                        {
                            Program.MyApp.ShowBox("Username already exists!");
                            Reset();
                        }
                    }
                    else
                    {
                        Database.DB.EditAdmin(NameContext, PasswordContext, EmailContext, PhoneContext, AdminIDContext);
                        Database.DB.EditAdminCollection(UsernameContext, PasswordContext, AdminIDContext, EmailContext, NameContext, PhoneContext);
                        Program.MyApp.ShowBox("Admin has been edited successfully!");
                        Program.Application.AdminsWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });
                    }
                }
            });
        }

        public void Reset()
        {
            this.Dispatcher.Invoke(() =>
            {
                AddBtn.IsEnabled = true;
                if (!EditMode)
                    AddBtn.Content = "Add Admin";
                else AddBtn.Content = "Edit";
            });
        }
        public string UsernameContext;
        public string NameContext;
        public string PasswordContext;
        public string EmailContext;
        public string PhoneContext;
        public string AdminIDContext;
        public int AdminLevelContext;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter username first!");
                return;
            }
            else if (Password.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter password first!");
                return;
            }
            else if (Phone.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter phone number first!");
                return;
            }
            else if (Email.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter email first!");
                return;
            }
            else if (AdminID.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify admin id first!");
                return;
            }
            else if (FullName.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify admin name first!");
                return;
            }
            else if (AdminSelect.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify admin level first!");
                return;
            }
            AddBtn.IsEnabled = false;
            AddBtn.Content = "Please Wait...";
            UsernameContext = Username.Text;
            PasswordContext = Password.Text;
            NameContext = FullName.Text;
            EmailContext = Email.Text;
            PhoneContext = Phone.Text;
            AdminIDContext = AdminID.Text;
            AdminLevelContext = AdminSelect.SelectedIndex;
            AddAdminMember();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = (Window)this.Parent;
            parentWindow.Close();
        }

    }
}
