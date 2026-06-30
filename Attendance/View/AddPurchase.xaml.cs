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
    public partial class AddPurchase : UserControl
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // allows only digits
            e.Handled = regex.IsMatch(e.Text);
        }
        public Model.PurchaseModel Model;
        public AddPurchase(bool editmode = false, Model.PurchaseModel classm = null)
        {
            this.EditMode = editmode;
            InitializeComponent();
            if(classm != null)
            this.IDContext = classm.ID;
            Model = classm;
            if (EditMode)
            {
                this.AddBtn.Content = "Edit";
                this.ShowBtn.Content = "Edit Purchase";
            }
            if (Program.Account.State == 1)
            {
                foreach (var item in Program.PurchaseType.Values)
                {
                    this.Type.Items.Add(item);
                }
            }
            else
            {
                for (int x = 0; x < 3; x++)
                {
                    var item = Program.PurchaseType[x];
                    this.Type.Items.Add(item);
                }
            }
        }
        public string IDContext = null;
        public bool EditMode = false;
    

        public void Reset()
        {
            this.Dispatcher.Invoke(() =>
            {
                AddBtn.IsEnabled = true;
                if (!EditMode)
                    AddBtn.Content = "Add Class";
                else AddBtn.Content = "Edit";
            });
        }
        public string UsernameContext;
        public string NameContext;
        public string PasswordContext;
        public string EmailContext;
        public string PhoneContext;
        public string AdminIDContext;
        public long StartDateTick;
        public long EndDateTick;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Type.SelectedItem == null)
            {
                Program.MyApp.ShowBox("Please select Type!");
                return;
            }
            else if (Price.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter price first!");
                return;
            }
            else if (Paid.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify paid first!");
                return;
            }
            DateTime? selectedDate = Time.SelectedDate;
            if (selectedDate.HasValue)
            {
                StartDateTick = selectedDate.Value.Ticks;
            }
            else
            {
                Program.MyApp.ShowBox("Please verify class start date first!");
                return;
            }            
            AddBtn.IsEnabled = false;
            AddBtn.Content = "Please Wait...";
            AddPurchaseSection(this.Name.Text,this.Type.SelectedItem.ToString(),this.Price.Text,this.Paid.Text,this.Time.SelectedDate.Value.ToString("dd/MM/yyyy"),this.Note.Text);
        }
        public async Task AddPurchaseSection(string Name,string Type,string Price,string Paid,string Time,string Note)
        {
            await Task.Run(() =>
            {
                {
                    if (!EditMode)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            int Remain = int.Parse(Price) - int.Parse(Paid);                         
                            IDContext = Database.DB.AddPuchase(0, Name, Type, Price, Paid, Remain.ToString(), Time, Note,Program.Username, false).ToString();
                            Database.DB.AddPuchaseCollection(IDContext, Name, Type, Price, Paid, Time, Note, Program.Username, false);
                        });
                        Program.MyApp.ShowBox("Purchase has been added!");
                        Program.Application.PurchaseWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });

                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            int Remain = int.Parse(Price) - int.Parse(Paid);
                           Database.DB.AddPuchase(int.Parse(IDContext), Name, Type, Price, Paid, Remain.ToString(), Time, Note, Model.User, true);
                            Database.DB.AddPuchaseCollection(IDContext, Name, Type, Price, Paid, Time, Note,Model.User, true);
                        });
                        Program.MyApp.ShowBox("Purchase has been edited successfully!");
                        Program.Application.PurchaseWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });
                    }
                }
            });
        }
        public string CurrentFacultyID = string.Empty;
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = (Window)this.Parent;
            parentWindow.Close();
        }
        private void FacultySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void DateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        
    }
}
