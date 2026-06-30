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
    public partial class AddReserve : UserControl
    {
        public string IDContext;
        public AddReserve(bool editmode = false,Model.ReserveModel student = null)
        {
            InitializeComponent();
            this.EditMode = editmode;
            StartDate.SelectedDate = DateTime.Today;
            EndDate.DisplayDateStart = DateTime.Today;
            //foreach (var faculty in Program.DressPool.Values)
            //   CodeSelect.Items.Add(faculty);
            CodeSelect.ItemsSource = Program.DressPool.Values;
            CodeSelect.DisplayMemberPath = "ID";
            CodeSelect.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
              new TextChangedEventHandler(CodeSelect_TextChanged));
            if (editmode)
            {
                ForceClose = true;
                this.IDContext = student.ID;
                this.Name.Text = student.Name;
                if (Program.Account.State == 0)
                    this.Name.IsEnabled = false;
              
                    CodeSelect.IsEnabled = false;
                CodeSelect.SelectedItem = Program.DressPool[int.Parse(student.DressModel.ID)];
                this.Address.Text = student.Address;          
                this.Phone.Text = student.Phone;
                this.Paid.Text = student.Paid;
                DateTime Start = DateTime.ParseExact(student.StartDate,"dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.StartDate.SelectedDate = Start.Date;
                System.Threading.Thread.Sleep(100);
                DateTime Deliver = DateTime.ParseExact(student.DeliverDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                try
                {
                    this.DeliverDate.SelectedDate = Deliver.Date;
                    DateTime End = DateTime.ParseExact(student.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    this.EndDate.SelectedDate = End.Date;
                }
                catch
                {
                    this.DeliverDate.SelectedDate = Deliver.Date;
                    try
                    {
                        DateTime End = DateTime.ParseExact(student.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        this.EndDate.SelectedDate = End.Date;
                    }
                    catch
                    {
                        DateTime End = DateTime.ParseExact(student.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        this.EndDate.SelectedDate = End.Date;
                    }
                }
                if (Program.Account.State == 0)
                {
                    this.StartDate.IsEnabled = false;
                    this.DeliverDate.IsEnabled = false;
                    this.EndDate.IsEnabled = false;
                }
                this.AddBtn.Content = "Edit";
                this.ShowBtn.Content="Edit Reservation";
            }
          
         
        }
        public bool ForceClose = false;
        private void CodeSelect_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource;
            string filter = tb.Text;
            
            if (string.IsNullOrEmpty(filter))
            {
                CodeSelect.ItemsSource = Program.DressPool.Values;
            }
            else
            {
                CodeSelect.ItemsSource = Program.DressPool.Values.Where(x => x.ID.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            if(!ForceClose)
            // Open dropdown automatically
            CodeSelect.IsDropDownOpen = true;
            ForceClose = false;
        }
        public bool EditMode = false;
       

        public void Reset()
        {
            this.Dispatcher.Invoke(() =>
            {
                AddBtn.IsEnabled = true;
                if(!EditMode)
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
            if (Name.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter Name first!");
                return;
            }
            else if (CodeSelect.SelectedItem == null)
            {
                Program.MyApp.ShowBox("Please select address first!");
                return;
            }
            else if (Phone.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter phone number first!");
                return;
            }
            else if (Paid.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please enter paid amount first!");
                return;
            }
            else if (Address.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify address first!");
                return;
            }
            else if (StartDate.Text == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify StartDate first!");
                return;
            }
            else if (DeliverDate.SelectedDate == null)
            {
                Program.MyApp.ShowBox("Please verify Deliver Date first!");
                return;
            }
            else if (EndDate.SelectedDate == null)
            {
                Program.MyApp.ShowBox("Please verify End Date first!");
                return;
            }
            if (DressIDContext == null || DressIDContext == string.Empty)
            {
                Program.MyApp.ShowBox("Please verify dress first!");
                return;
            }
            AddBtn.IsEnabled = false;
            AddBtn.Content = "Please Wait...";
            NameContext = Name.Text;
            PhoneContext = Phone.Text;
            PaidContext = Paid.Text + " L.E";
            AddressContext = Address.Text;                   
            StartDateContext = StartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            DeliveryContext = DeliverDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            EndDateContext = EndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            NoteContext = Note.Text;
            AddStudentMember(Name.Text, Phone.Text, Paid.Text, Address.Text, StartDateContext, DeliveryContext, EndDateContext, NoteContext);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // allows only digits
            e.Handled = regex.IsMatch(e.Text);
        }
        public string NoteContext;
        public async Task AddStudentMember(string Name,string Phone,string Paid,string Address,string StartDate,string DeliverDate,string EndDate,string Note)
        {
            await Task.Run(() =>
            {
                {
                    if (!EditMode)
                    {
                        // if (Database.DB.VerifyUsername(UsernameContext))
                        {

                            //  if (Database.DB.VerifyStudentID(StudentIDContext))
                            {
                                IDContext= Database.DB.AddReservation(Name, Phone, Paid, Address, StartDate, DeliverDate, EndDate, DressIDContext, NoteContext);                               
                                Database.DB.AddReserveCollection(IDContext,1,Program.Account.Username, Name, Phone, Paid, Address, StartDate, DeliverDate, EndDate, DressData, NoteContext,false);
                                Program.MyApp.ShowBox("Reservation has been added!");
                                Database.DB.UpdateAvailability(DressIDContext, 1);
                                Program.Application.ReservationWindow.ShowPageRaws();
                                this.Dispatcher.Invoke(() =>
                                {
                                    Window parentWindow = (Window)this.Parent;
                                    parentWindow.Close();
                                });
                            }
                            // else
                            {
                                //Program.MyApp.ShowBox("StudentID already exists!");
                                //Reset();
                            }
                        }
                        // else
                        {
                            // Program.MyApp.ShowBox("Username already exists!");
                            //Reset();
                        }
                    }
                    else
                    {
                        Database.DB.EditReserve(IDContext,1, DressIDContext, Name, Phone, Paid, Address, StartDate, DeliverDate, EndDate, NoteContext);
                        Database.DB.AddReserveCollection(IDContext,1, Program.Account.Username, Name, Phone, Paid, Address, StartDate, DeliverDate, EndDate, DressData, NoteContext, true);
                        Program.MyApp.ShowBox("Reservation has been edited successfully!");
                        Program.Application.ReservationWindow.ShowPageRaws();
                        this.Dispatcher.Invoke(() =>
                        {
                            Window parentWindow = (Window)this.Parent;
                            parentWindow.Close();
                        });
                    }
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
        private void FacultySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Dress = CodeSelect.SelectedItem as Model.DressModel;
            DressData = Dress;
            DressIDContext = Dress.ID;
            this.PriceRent.Text = DressData.RentPrice + " L.E";
            InitiateDates(DressIDContext);


        }
        private List<(DateTime start, DateTime end)> blockedPeriods = new List<(DateTime start, DateTime end)>();
        public void InitiateDates(string DressID)
        {
            blockedPeriods.Clear(); // Clear previous periods

            foreach (var reserve in Program.ReservePool.Values.Where(p => p.DressID == DressID))
            {
                DateTime startDate = DateTime.ParseExact(reserve.DeliverDate, "dd/MM/yyyy", null);
                DateTime endDate = DateTime.ParseExact(reserve.EndDate, "dd/MM/yyyy", null);

                // Add to blocked periods list
                blockedPeriods.Add((startDate, endDate));

                // Add to blackout dates in DatePickers
                DeliverDate.BlackoutDates.Add(new CalendarDateRange(startDate, endDate.AddDays(-1)));
                EndDate.BlackoutDates.Add(new CalendarDateRange(startDate, endDate.AddDays(-1)));
            }
        }
        /* public void InitiateDates(string DressID)
         {
             foreach(var reserve in Program.ReservePool.Values.Where(p=>p.DressID == DressID).ToList())
             {
                 DateTime startDate = DateTime.ParseExact(reserve.DeliverDate, "dd/MM/yyyy", null);
                 DateTime endDate = DateTime.ParseExact(reserve.EndDate, "dd/MM/yyyy", null);
                 DeliverDate.BlackoutDates.Add(new CalendarDateRange(startDate, endDate.AddDays(-1)));
                 EndDate.BlackoutDates.Add(new CalendarDateRange(startDate, endDate.AddDays(-1)));
             }
         }*/
        private void MyDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void EndDate_Loaded(object sender, RoutedEventArgs e)
        {
            // Make the internal textbox read-only so user cannot type manually
            if (EndDate.Template.FindName("PART_TextBox", EndDate) is System.Windows.Controls.Primitives.DatePickerTextBox textBox)
            {
                textBox.IsReadOnly = true;
                textBox.Cursor = Cursors.Arrow; // Optional: show arrow cursor
            }
        }
        private void EndDate_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!EndDate.IsDropDownOpen)
            {
                EndDate.IsDropDownOpen = true;
                e.Handled = true; // Prevent default handling
            }

        }

        private void DeliverDate_Loaded(object sender, RoutedEventArgs e)
        {
            // Make the internal textbox read-only so user cannot type manually
            if (DeliverDate.Template.FindName("PART_TextBox", DeliverDate) is System.Windows.Controls.Primitives.DatePickerTextBox textBox)
            {
                textBox.IsReadOnly = true;
                textBox.Cursor = Cursors.Arrow; // Optional: show arrow cursor
            }
        }
        private void DeliverDate_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!DeliverDate.IsDropDownOpen)
            {
                DeliverDate.IsDropDownOpen = true;
                e.Handled = true; // Prevent default handling
            }

        }


        private void StartDate_Loaded(object sender, RoutedEventArgs e)
        {
            // Make the internal textbox read-only so user cannot type manually
            if (StartDate.Template.FindName("PART_TextBox", StartDate) is System.Windows.Controls.Primitives.DatePickerTextBox textBox)
            {
                textBox.IsReadOnly = true;
                textBox.Cursor = Cursors.Arrow; // Optional: show arrow cursor
            }
        }
        private void StartDate_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!StartDate.IsDropDownOpen)
            {
                StartDate.IsDropDownOpen = true;
                e.Handled = true; // Prevent default handling
            }

        }

        private void DeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeliverDate.SelectedDate.HasValue)
            {
                DateTime selectedDelivery = DeliverDate.SelectedDate.Value;

                // Reset EndDatePicker
                EndDate.BlackoutDates.Clear();
                EndDate.SelectedDate = null;

                // Block all dates before selectedDelivery
                if (selectedDelivery > DateTime.MinValue)
                {
                    EndDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, selectedDelivery.AddDays(-1)));
                }

                // Block all dates that overlap existing reservations
                foreach (var period in blockedPeriods)
                {
                    // If period overlaps with the new delivery, block the overlapping end dates
                    if (period.start > selectedDelivery)
                    {
                        EndDate.BlackoutDates.Add(new CalendarDateRange(period.start, DateTime.MaxValue));
                    }
                }

                // Optional: allow end date to be the day before the next blocked period
                // This ensures only valid end dates are selectable
            }
        }
    }
}
