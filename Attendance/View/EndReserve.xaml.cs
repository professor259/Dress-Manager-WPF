using Attendance.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class EndReserve : UserControl
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // allows only digits
            e.Handled = regex.IsMatch(e.Text);
        }
        public string IDContext;
        public Model.ReserveModel Model;
        public EndReserve(bool editmode = false, Model.ReserveModel student = null)
        {
            InitializeComponent();
            this.EditMode = editmode;
            this.IDContext = student.ID;
            Model = student;
            this.NameValue.Text = student.Name;
            this.RentedID.Text = student.DressID;
            this.PriceRent.Text = student.DressModel.RentPrice + " L.E";
            this.PaidAmount.Text = student.Paid +" L.E";
            this.RemainAmount.Text = student.Remain;
            this.Start.Text = student.StartDate;
            this.Deliver.Text = student.DeliverDate;
            this.End.Text = student.EndDate;
            if (!EditMode)
            {
                AddBtn.Content = "Deliver";
                ShowBtn.Content = "Deliver Reservation";
                AddBtn.Background = new SolidColorBrush(
    (Color)ColorConverter.ConvertFromString("#293a4b"));
            }
        }
        public bool EditMode = false;


        public void Reset()
        {
            this.Dispatcher.Invoke(() =>
            {
                AddBtn.IsEnabled = true;
                if (!EditMode)
                    AddBtn.Content = "Add Reservation";
                else AddBtn.Content = "Edit";
            });
        }
        public string NameContext;
        public string PasswordContext;
        public string PaidContext;
        public string PhoneContext;
        public string AddressContext;
        public string StartDateContext;
        public string DeliveryContext;
        public string EndDateContext;
        public string DressIDContext;
        private void Button_Click(object sender, RoutedEventArgs e)
        {          
            AddBtn.IsEnabled = false;
            AddBtn.Content = "Please Wait...";
            PaidZContext =   Paid.Text.Split(' ')[0];
            EndMember();
        }
        public string PaidZContext;
        public async Task EndMember()
        {
            await Task.Run(() =>
            {
                {
                    if (EditMode)
                    {
                        int OriginalPaid = int.Parse(Model.Paid);
                        if (PaidZContext != "")
                        {

                            int PaidNow = int.Parse(PaidZContext);
                            OriginalPaid += PaidNow;
                        }
                        Database.DB.CompleteReserve(Model.ID, OriginalPaid.ToString());
                        Program.MyApp.ShowBox("Completed!");
                        Program.Application.ReservationWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });
                    }
                    else
                    {
                        int OriginalPaid = int.Parse(Model.Paid);
                        if (PaidZContext != "")
                        {

                            int PaidNow = int.Parse(PaidZContext);
                            OriginalPaid += PaidNow;
                        }
                        Database.DB.DeliverReserve(Model.ID, OriginalPaid.ToString());
                        Program.MyApp.ShowBox("Deliver Success!");
                        Program.Application.ReservationWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });
                    }
                }
                // else
                {
                    //Program.MyApp.ShowBox("StudentID already exists!");
                    //Reset();
                }

            });
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = (Window)this.Parent;
            parentWindow.Close();
        }

        private void GradeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public Model.DressModel DressData;        
    }
}
