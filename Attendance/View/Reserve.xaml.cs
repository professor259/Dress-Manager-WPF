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
namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Reserve : UserControl
    {
        public enum FilterOption : uint
        {
            ID = 1,
            Name = 2,
            Index = 3,
        }
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.ReserveModel> StudentsPool = new ObservableCollection<ReserveModel>();
        public Reserve()
        {
            try
            {
                InitializeComponent();               
                Program.Application.ReservationWindow = this;
                var Converter = new BrushConverter();
                for (int x = 0; x < 50; x++)
                {
                    ColorsList.Add(x, (Brush)Converter.ConvertFromString(GetColor()));
                }              
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
        public ReserveModel[] GetArray()
        {
        
            if(CurrentLevelView == "All")
            {
                return Program.ReservePool.Values.Where(p => p.ID == CurrentFacultyID).ToArray(); 
            }
            else
            {
                return Program.ReservePool.Values.Where(p => p.ID == CurrentFacultyID).ToArray();
            }
        }
        public void ShowPageRaws()
        {
           
            this.Dispatcher.Invoke(() =>
            {
                var Converter = new BrushConverter();
                StudentsPool = new ObservableCollection<ReserveModel>();
                this.memberDatagrid.ItemsSource = StudentsPool;
                var Array = Program.ReservePool.Values.ToArray();
                if (Array == null)
                    return;
                int offset = Page * GetMaxPagging();
                int count = Math.Min(GetMaxPagging(), Array.Length);
                int ColorIndexer = 0;
                for (byte x = 0; x < count; x++)
                {
                    if (x + offset >= Array.Length)
                        break;
                    if (x > 0 && x % 50 == 0)
                        ColorIndexer = 0;
                    var entity = Array[x + offset];
                    ReserveModel student = new ReserveModel();
                    student.Charater = entity.Charater;
                    student.ColorBg = ColorsList[ColorIndexer];
                    student.Name = entity.Name;
                    student.Phone = entity.Phone;
                    student.ID = entity.ID;
                    student.Note = entity.Note;
                    student.Status = entity.Status;
                    student.Index = (x + offset + 1);
                    Program.ReservePool[int.Parse(entity.ID)].Index = student.Index;
                    student.Address = entity.Address;
                    student.Paid = entity.Paid;
                    student.DressModel = entity.DressModel;
                    student.StartDate = entity.StartDate;
                    student.DeliverDate = entity.DeliverDate;
                    student.EndDate = entity.EndDate;
                    StudentsPool.Add(student);
                    ColorIndexer++;
                }
            });
            //this.memberDatagrid.ItemsSource = StudentsPool;
        }
        public System.Collections.ObjectModel.ObservableCollection<Model.ReserveModel> Items
        {
            get { return StudentsPool; }
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
            return Program.ReservePool.Count / GetMaxPagging();
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
                SearchElement(txt);
            });
        }
        public void SearchElement(string txt)
        {
           
              var Converter = new BrushConverter();
            StudentsPool = new ObservableCollection<ReserveModel>();
            this.Dispatcher.Invoke(() =>
            {
                this.memberDatagrid.ItemsSource = StudentsPool;
            });
            var Array = Program.ReservePool.Values.OrderBy(o=>o.DeliverDate).ToArray();
            if (Array == null)
                return;
            int ColorIndexer = 0;
            foreach (var stud in Array)
            {
                if ((stud.EndDate != null && stud.EndDate.ToLower().Contains(txt.ToLower())) || (stud.DeliverDate != null && stud.DeliverDate.ToLower().Contains(txt.ToLower())) || (stud.StartDate != null && stud.StartDate.ToLower().Contains(txt.ToLower())) || (stud.Name != null && stud.Name.ToLower().Contains(txt.ToLower())) || (stud.ID != null && stud.ID.ToLower().Contains(txt.ToLower())) || (stud.DressID != null && stud.DressID.ToLower().Contains(txt.ToLower())) || (stud.Address != null && stud.Address.ToLower().Contains(txt.ToLower())) || (stud.Phone != null && stud.Phone.ToLower().Contains(txt.ToLower())))
                {
                    ReserveModel student = new ReserveModel();
                    student.Charater = stud.Charater;
                    student.Name = stud.Name;
                    student.ID = stud.ID;
                    student.Index = stud.Index;
                    student.Phone = stud.Phone;
                    student.Address = stud.Address;
                    student.Status = stud.Status;
                    student.Note = stud.Note;
                    student.DressModel = stud.DressModel;
                    student.Paid = stud.Paid;
                    student.StartDate = stud.StartDate;
                    student.DeliverDate = stud.DeliverDate;
                    student.EndDate = stud.EndDate;
                    this.Dispatcher.Invoke(() =>
                    {
                        if (ColorIndexer > 0 && ColorIndexer % 50 == 0)
                            ColorIndexer = 0;
                        student.ColorBg = ColorsList[ColorIndexer];
                        StudentsPool.Add(student);
                        ColorIndexer++;
                    });
                }


            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            var Converter = new BrushConverter();
            Window window = new Window
            {
                Title = "Add New Reservation",
                Content = new AddReserve(),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),               
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose reservation to remove");
                return;
            }
            if (Program.Account.State != 1)
            {
                Program.MyApp.ShowBox("only admin can remove reservation!");
                return;
            }
            var Student = item as Model.ReserveModel;
            Program.ReservePool.Remove(int.Parse(Student.ID));
            {
                ShowPageRaws();
                Database.DB.RemoveStudent(Student.ID);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose reservation to edit");
                return;
            }
            if (Program.Account.State != 1)
            {
                Program.MyApp.ShowBox("only admin can edit reservation!");
                return;
            }
            var Student = item as Model.ReserveModel;
            Window window = new Window
            {
                Title = "Edit Reservation",
                Content = new AddReserve(true,Student),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPageRaws();
        }
    
    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Program.Application.HandleSize();
            }
        }      

        public string CurrentFacultyID;
        public string CurrentLevelView;

        private void EndBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose reservation");
                return;
            }           
            var Student = item as Model.ReserveModel;
            Window window = new Window
            {
                Title = "Complete Reservation",
                Content = new EndReserve(true, Student),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
        private void DeliverBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose reservation");
                return;
            }
            var Student = item as Model.ReserveModel;
            Window window = new Window
            {
                Title = "Deliver Reservation",
                Content = new EndReserve(false, Student),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
    }
}
