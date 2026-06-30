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
    /// Interaction logic for Faculty.xaml
    /// </summary>
    public partial class Dress : UserControl
    {
        public enum FilterOption : uint
        {
            ID = 1,
            Name = 2,
            Index = 3,
        }
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.DressModel> DressPool = new ObservableCollection<DressModel>();
        public Dress()
        {
            try
            {
                InitializeComponent();
                Program.Application.DressManager = this;
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
            return 13;
        }

        public void ShowPageRaws()
        {
            this.Dispatcher.Invoke(() =>
            {
                var Converter = new BrushConverter();
                DressPool = new ObservableCollection<DressModel>();
                this.memberDatagrid.ItemsSource = DressPool;
                var Array = Program.DressPool.Values.ToArray();
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
                    DressModel fac = new DressModel();
                    fac.Charater = entity.Charater;
                    fac.ColorBg = ColorsList[ColorIndexer];               
                    fac.ID = entity.ID;
                    fac.Index = (x + offset + 1);
                    fac.SizeS = entity.SizeS;
                    fac.Size = entity.Size;
                    fac.Color = entity.Color;
                    fac.Status = entity.Status;
                    fac.TotalRent = entity.TotalRent;
                    fac.RentPrice = entity.RentPrice;
                    fac.CostPrice = entity.CostPrice;
                    fac.Name = entity.Name;
                    DressPool.Add(fac);
                    ColorIndexer++;
                }
            });          
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
            return Program.DressPool.Count / GetMaxPagging();
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
            DressPool = new ObservableCollection<DressModel>();
            this.Dispatcher.Invoke(() =>
            {
                this.memberDatagrid.ItemsSource = DressPool;
            });
            var Array = Program.DressPool.Values;
            int ColorIndexer = 0;
            foreach (var stud in Array)
            {
                if ((stud.Name != null && stud.Name.ToLower().Contains(txt.ToLower())) || (stud.ID != null && stud.ID.ToLower().Contains(txt.ToLower())))
                {
                    DressModel admin = new DressModel();
                    admin.Charater = stud.Charater;
                    admin.ID = stud.ID;
                    admin.Index = stud.Index;
                    admin.Status = stud.Status;
                    admin.Name = stud.Name;                 
                    admin.Color = stud.Color;
                    admin.SizeS = stud.SizeS;
                    admin.Size = stud.Size;
                    admin.TotalRent = stud.TotalRent;
                    admin.CostPrice = stud.CostPrice;
                    admin.RentPrice = stud.RentPrice;
                    this.Dispatcher.Invoke(() =>
                    {
                        if (ColorIndexer > 0 && ColorIndexer % 50 == 0)
                            ColorIndexer = 0;
                        admin.ColorBg = ColorsList[ColorIndexer];
                        DressPool.Add(admin);
                        ColorIndexer++;
                    });
                }

            }
        }
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
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose dress to edit");
                return;
            }
            if (Program.Account.State != 1)
            {
                Program.MyApp.ShowBox("only admin can edit dress!");
                return;
            }
            var Student = item as Model.DressModel;
            Window window = new Window
            {
                Title = "Edit Dress",
                Content = new AddDress(Student),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Program.Account.State != 1)
            {
                Program.MyApp.ShowBox("only admin can add new dress!");
                return;
            }
            var Converter = new BrushConverter();
            Window window = new Window
            {
                Title = "Add New Dress",
                Content = new AddDress(null),
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
            if (Program.Account.State != 1)
            {
                Program.MyApp.ShowBox("only admin can remove dress!");
                return;
            }
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose dress to remove");
                return;
            }
            var Admin = item as Model.DressModel;
            Program.DressPool.Remove(int.Parse(Admin.ID));
            {
                ShowPageRaws();
                Database.DB.RemoveFaculty(Admin.ID);
            }
        }   
        public FilterOption SelectedFilter = FilterOption.Name;
      
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Program.Application.HandleSize();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPageRaws();
        }
    }
}
