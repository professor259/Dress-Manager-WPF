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
    public partial class Purchases : UserControl
    {        
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.PurchaseModel> ClassPool = new ObservableCollection<PurchaseModel>();
        public Purchases()
        {
            try
            {
                InitializeComponent();
                Program.Application.PurchaseWindow = this;
                var Converter = new BrushConverter();
                for (int x = 0; x < 50; x++)
                {
                    ColorsList.Add(x, (Brush)Converter.ConvertFromString(GetColor()));
                }
               // ShowPageRaws();
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
                ClassPool = new ObservableCollection<PurchaseModel>();
                this.memberDatagrid.ItemsSource = ClassPool;
                var Array = Program.PurchasePool.Values.ToArray();
                if(Program.Account.State != 1)
                {
                    Array = Program.PurchasePool.Values.Where(p=>p.User == Program.Account.Username).ToArray();
                }
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
                    PurchaseModel Class = new PurchaseModel();
                    Class.Name = entity.Name;
                    Class.Type = entity.Type;
                    Class.Paid = entity.Paid+ " L.E";
                    Class.Price = entity.Price + " L.E";
                    Class.Time = entity.Time;
                    Class.Note = entity.Note;
                    Class.User = entity.User;
                    Class.ID = entity.ID;
                    Class.UserName = entity.UserName;
                    Class.Index = (x + offset + 1);                  
                    Class.ColorBg = ColorsList[ColorIndexer];                                 
                    ClassPool.Add(Class);
                    ColorIndexer++;
                }
            });
            //this.memberDatagrid.ItemsSource = AdminsPool;
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
            return Program.PurchasePool.Count / GetMaxPagging();
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            Window window = new Window
            {
                Title = "Add New Purchase",
                Content = new AddPurchase(),
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
                Program.MyApp.ShowBox("Please choose purchase to remove");
                return;
            }
            var Class = item as Model.PurchaseModel;
            if (Program.Account.State == 0)
            {
                Program.MyApp.ShowBox("You can`t remove this purchase");
                return;
            }
            Program.PurchasePool.Remove(int.Parse(Class.ID));
            {
                ShowPageRaws();
                Database.DB.RemoveClass(Class.ID);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose purchase to edit");
                return;
            }            
            var Class = item as Model.PurchaseModel;
            if (Program.Account.State == 0)
            {
                Program.MyApp.ShowBox("You can`t edit require admin");
                return;
            }
            Window window = new Window
            {
                Title = "Edit Purchase",
                Content = new AddPurchase(true, Class),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }       
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
