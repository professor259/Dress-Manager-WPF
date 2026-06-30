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
using System.Collections.ObjectModel;
using Attendance.Model;
using System.Globalization;

namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class AttendanceMenu : UserControl
    {
        public enum FilterOption : uint
        {
            ID = 1,
            Name = 2,
            Index = 3,
        }
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.ReportModel> ReportsShow = new ObservableCollection<ReportModel>();
        public AttendanceMenu()
        {
            try
            {
                InitializeComponent();
                Program.Application.AttendanceWindow = this;
                var Converter = new BrushConverter();
                if (ColorsList.Count == 0)
                {
                    for (int x = 0; x < 50; x++)
                    {

                        ColorsList.Add(x, (Brush)Converter.ConvertFromString(GetColor()));
                    }
                }
                int Puchased = 0;
                int Reserved = 0;
                foreach (var item in Program.ReservePool.Values)
                {
                    ReportModel model = new ReportModel();
                    model.Cost = item.Paid;
                    DateTime parsedDate = DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.Date = parsedDate;
                    model.DateShow = item.StartDate;
                    model.Name = item.Name;
                    model.OrderID = item.ID;
                    model.Type = "Reservation";
                    model.User = item.User;
                    this.ReportsPool.Add(model);
                    Reserved += int.Parse(item.Paid);
                }
                foreach (var item in Program.PurchasePool.Values)
                {
                    ReportModel model = new ReportModel();
                    model.Cost = item.Paid;
                    DateTime parsedDate = DateTime.ParseExact(item.Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.Date = parsedDate;
                    model.DateShow = item.Time;
                    model.Type = "Purchase";
                    model.Name = item.Name;
                    model.OrderID = item.ID;
                    model.User = item.User;
                    this.ReportsPool.Add(model);
                    Puchased += int.Parse(item.Paid);
                }
                this.Puchased.Text = Puchased.ToString() + " L.E";
                this.Reserved.Text = Reserved.ToString() + " L.E";             
                this.Balance.Text = Reserved - Puchased + " L.E";
            }
            catch
            {

            }
        }
        public static Dictionary<int, Brush> ColorsList = new Dictionary<int, Brush>();
        public static string GetColor()
        {
            return Extentions.Kernel.RandFromGivingStrings("#1098ad", "#1e88e5", "#ff8f00", "#ff5252", "#0ca678", "#ff6d00");
        }
        public int GetMaxPagging()
        {
            return 12;
        }

        public void ShowPageRaws(string filter = "", FilterOption filtertype = FilterOption.Index)
        {
          
            this.Dispatcher.Invoke(() =>
            {
                var Converter = new BrushConverter();
                ReportsShow = new ObservableCollection<ReportModel>();
                this.memberDatagrid.ItemsSource = ReportsShow;
                System.SafeDictionary<int, ReportModel> AllAtten = new System.SafeDictionary<int, ReportModel>();

                //var Array = Program.AttendancePool.Values.Where(i => i.Class == CurrentClass.ClassName);
                var Array = ReportsPool.OrderByDescending(r => r.Date).ToArray();
                if (FromDate.SelectedDate != null && ToDate.SelectedDate != null)
                {
                     Array = FliterPool.OrderByDescending(r => r.Date).ToArray();
                }
               
                int offset = Page * GetMaxPagging();
                int count = Math.Min(GetMaxPagging(), Array.Length);
                int ColorIndexer = 0;
                if (filter == string.Empty)
                {
                    for (byte x = 0; x < count; x++)
                    {
                        if (x + offset >= Array.Length)
                            break;
                        if (x > 0 && x % 50 == 0)
                            ColorIndexer = 0;
                        var entity = Array[x + offset];
                        AddToCollection(entity, (x + offset + 1), ColorIndexer);
                        ColorIndexer++;
                    }
                }
                else
                {
                    int x = 0;
                    foreach (var entity in Array)
                    {
                        if ((entity.Name != null &&entity.Name.ToLower().Contains(filter.ToLower())) ||(entity.Type != null && entity.Type.ToLower().Contains(filter.ToLower())) || (entity.OrderID != null && entity.OrderID.Contains(filter)) || (entity.User != null && entity.User.ToLower().Contains(filter.ToLower())))
                        {
                            if (x > 0 && x % 50 == 0)
                                ColorIndexer = 0;
                            AddToCollection(entity, (x + 1), ColorIndexer);
                            ColorIndexer++;
                            x++;
                        }
                    }
                }
            });
        }
        public void AddToCollection(ReportModel entity, int Index = 0, int ColorIndexer = 0)
        {
            entity.Index = Index;
            entity.ColorBg = ColorsList[ColorIndexer];
            ReportsShow.Add(entity);
        }
        public void Start_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Page1_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as RadioButton;
            var PageNumber = int.Parse(Button.Content.ToString());
            if (PageNumber - 1 != Page)
            {
                Page = PageNumber - 1;
                ShowPageRaws();
            }
        }

        private void Page2_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as RadioButton;
            var PageNumber = int.Parse(Button.Content.ToString());
            if (PageNumber - 1 != Page)
            {
                Page = PageNumber - 1;
                ShowPageRaws();
            }
        }
        private void Page3_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as RadioButton;
            var PageNumber = int.Parse(Button.Content.ToString());
            if (PageNumber - 1 != Page)
            {
                Page = PageNumber - 1;
                ShowPageRaws();
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            Page1.Content = "1";
            Page2.Content = "2";
            Page3.Content = "3";
            Page = 0;
            Page1.IsChecked = true;
            Page2.IsChecked = false;
            Page3.IsChecked = false;
            ShowPageRaws();
        }
        public int GetLastPageNumber()
        {
            return ReportsShow.Count / GetMaxPagging();
        }
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            int LastPage = GetLastPageNumber();
            if (LastPage > 2)
            {
                Page1.Content = LastPage - 1;
                Page2.Content = LastPage;
                Page3.Content = LastPage + 1;
            }
            else
            {
                Page1.Content = 1;
                Page2.Content = 2;
                Page3.Content = 3;
            }
            Page = LastPage;
            Page1.IsChecked = false;
            Page2.IsChecked = false;
            Page3.IsChecked = true;
            ShowPageRaws();
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (Page1.IsChecked == true)
            {
                var Button = Page1;
                var Number = int.Parse(Button.Content.ToString());
                if (int.Parse(Button.Content.ToString()) > 1)
                {
                    Page1.Content = Number - 1;
                    Page = Number - 2;
                    Page2.Content = Number;
                    Page3.Content = Number + 1;
                    ShowPageRaws();
                }
            }
            else if (Page2.IsChecked == true)
            {
                Page2.IsChecked = false;
                Page1.IsChecked = true;
                Page -= 1;
                ShowPageRaws();
            }
            else if (Page3.IsChecked == true)
            {
                Page3.IsChecked = false;
                Page2.IsChecked = true;
                Page -= 1;
                ShowPageRaws();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (Page1.IsChecked == true)
            {
                Page1.IsChecked = false;
                Page2.IsChecked = true;
                Page += 1;
                ShowPageRaws();

            }
            else if (Page2.IsChecked == true)
            {
                Page2.IsChecked = false;
                Page3.IsChecked = true;
                Page += 1;
                ShowPageRaws();
            }
            else if (Page3.IsChecked == true)
            {
                var Button = Page3;
                var Number = int.Parse(Button.Content.ToString());//3
                if (int.Parse(Button.Content.ToString()) < GetLastPageNumber() + 1)
                {
                    Page1.Content = Number - 1;
                    Page += 1;
                    Page2.Content = Number;
                    Page3.Content = Number + 1;
                    ShowPageRaws();
                }
            }
        }
        public async Task SearchTask(string txt)
        {
            await Task.Run(() =>
            {
                ShowPageRaws(txt, SelectedFilter);
                //SearchElement(txt);
            });
        }
        public FilterOption SelectedFilter = FilterOption.Name;
        public Task Search;
        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Text = sender as TextBox;
            if (Text.Text != string.Empty)
            {
                if (Search != null && !Search.IsCompleted)
                {
                    try
                    {
                        Search.Dispose();
                    }
                    catch
                    {

                    }
                }
                Search = SearchTask(Text.Text);
            }
            else
            {
                ShowPageRaws();
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Program.Application.HandleSize();
            }
        }

        private void FilterTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {                     
            Page1.IsChecked = true;
            Page = 0;
            ShowPageRaws();
        }
        public Model.PurchaseModel CurrentClass;
       

        private void IsStudentPresent_Click(object sender, RoutedEventArgs e)
        {
           
        }
        public long DateMinTicks = 0;
        public long DateMaxTicks = 0;
        public long MinTicks = 0;
        public long MaxTicks = 0;
        private void Dates_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender;
            if (item == null)
            {
                return;
            }
            var DatePicker = sender as DatePicker;
            MinTicks = DatePicker.SelectedDate.Value.Ticks;
            MaxTicks = DateTime.FromBinary(MinTicks).AddHours(23).AddMinutes(59).Ticks;
            ShowPageRaws();
        }
        public string CurrentFacultyID;
        private void FacultySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void FromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public List<Model.ReportModel> FilterByDateRange(List<Model.ReportModel> pool,DateTime fromDate, DateTime toDate)
        {
            return pool.Where(x => x.Date.Date >= fromDate.Date && x.Date.Date <= toDate.Date).ToList();
        }
        public List<Model.ReportModel> ReportsPool = new List<Model.ReportModel>();
        public List<Model.ReportModel> FliterPool = new List<Model.ReportModel>();
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            if(FromDate.SelectedDate == null || ToDate.SelectedDate == null)
            {
                Program.MyApp.ShowBox("Please verify from & to date!");
                return;
            }
            DateTime from = FromDate.SelectedDate.Value.Date;
            DateTime to = ToDate.SelectedDate.Value.Date;
            FliterPool =  FilterByDateRange(ReportsPool, from, to);
            ShowPageRaws();
            int Puchased = 0;
            int Reserved = 0;
            foreach (var item in FliterPool)
                if (item.Type == "Purchase")
                    Puchased += int.Parse(item.Cost);
                else Reserved += int.Parse(item.Cost);
            this.Puchased.Text = Puchased.ToString() + " L.E";
            this.Reserved.Text = Reserved.ToString() + " L.E";
            this.Balance.Text = Reserved - Puchased + " L.E";
        }
    }
}
