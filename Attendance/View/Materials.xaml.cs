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
    public partial class Materials : UserControl
    {
        public enum FilterOption : uint
        {
            ID = 1,
            Name = 2,
            Index = 3,
        }
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.MaterialModel> MaterialsPool = new ObservableCollection<MaterialModel>();
        public Materials()
        {
            try
            {
                InitializeComponent();
                Program.Application.MateirlasWindow = this;
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
                MaterialsPool = new ObservableCollection<MaterialModel>();
                this.memberDatagrid.ItemsSource = MaterialsPool;
                string tomorrow = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                var Array = Program.ReservePool.Values.Where(p=>p.DeliverDate == tomorrow).ToArray();
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
                    MaterialModel fac = new MaterialModel();
                    fac.Charater = entity.Charater;
                    fac.ColorBg = ColorsList[ColorIndexer];
                    fac.ReserveID = entity.ID;
                    fac.Index = (x + offset + 1);
                    fac.DressID = entity.DressID;
                    fac.DressName = entity.DressModel.Name;
                    fac.Deliver = entity.DeliverDate;                                
                    MaterialsPool.Add(fac);
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {          
            Page1.IsChecked = true;
            Page = 0;
            ShowPageRaws();
        }
    }
}
