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
    public partial class Admins : UserControl
    {
        public enum FilterOption : uint
        {
            ID = 1,
            Name = 2,
            Index = 3,
        }
        public static int Page = 0;
        public static System.Collections.ObjectModel.ObservableCollection<Model.AdminsModel> AdminsPool = new ObservableCollection<AdminsModel>();
        public Admins()
        {
            try
            {
                InitializeComponent();
                Program.Application.AdminsWindow = this;
                var Converter = new BrushConverter();
                for (int x = 0; x < 50; x++)
                {
                    ColorsList.Add(x, (Brush)Converter.ConvertFromString(GetColor()));
                }
             //   ShowPageRaws();
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
                AdminsPool = new ObservableCollection<AdminsModel>();
                this.memberDatagrid.ItemsSource = AdminsPool;
                var Array = Program.AdminsPool.Values.ToArray();
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
                    AdminsModel admin = new AdminsModel();
                    admin.Charater = entity.Charater;
                    admin.ColorBg = ColorsList[ColorIndexer];
                    admin.Email = entity.Email;
                    admin.ID = entity.ID;
                    admin.Index = (x + offset + 1);
                    admin.Name = entity.Name;
                    admin.State = entity.State;
                    admin.StateView = GetState(admin.State);
                    admin.Phone = entity.Phone;
                    admin.Username = entity.Username;
                    admin.Password = entity.Password;
                    AdminsPool.Add(admin);
                    ColorIndexer++;
                }
            });
            //this.memberDatagrid.ItemsSource = AdminsPool;
        }
        public System.Collections.ObjectModel.ObservableCollection<Model.AdminsModel> Items
        {
            get { return AdminsPool; }
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
            return Program.AdminsPool.Count / GetMaxPagging();
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
            AdminsPool = new ObservableCollection<AdminsModel>();
            this.Dispatcher.Invoke(() =>
            {
                this.memberDatagrid.ItemsSource = AdminsPool;
            });
            var Array = Program.AdminsPool.Values;
            int ColorIndexer = 0;
            foreach (var stud in Array)
            {
                if (SelectedFilter == FilterOption.Name)
                {
                    if (stud.Name.ToLower().Contains(txt.ToLower()))
                    {
                        AdminsModel admin = new AdminsModel();
                        admin.Charater = stud.Charater;
                        admin.Email = stud.Email;
                        admin.ID = stud.ID;
                        admin.Index = stud.Index;
                        admin.Name = stud.Name;
                        admin.Username = stud.Username;
                        admin.Password = stud.Password;
                        admin.Phone = stud.Phone;
                        admin.State = stud.State;
                        admin.StateView = GetState(admin.State);
                        this.Dispatcher.Invoke(() =>
                        {
                            if (ColorIndexer > 0 && ColorIndexer % 50 == 0)
                                ColorIndexer = 0;
                            admin.ColorBg = ColorsList[ColorIndexer];
                            AdminsPool.Add(admin);
                            ColorIndexer++;
                        });
                    }
                }
                else
                {
                    if (stud.ID.Contains(txt))
                    {
                        AdminsModel admin = new AdminsModel();
                        admin.Charater = stud.Charater;
                        admin.Email = stud.Email;
                        admin.ID = stud.ID;
                        admin.Index = stud.Index;
                        admin.Username = stud.Username;
                        admin.Password = stud.Password;
                        admin.Name = stud.Name;
                        admin.State = stud.State;
                        admin.StateView = GetState(admin.State);
                        admin.Phone = stud.Phone;
                        this.Dispatcher.Invoke(() =>
                        {
                            if (ColorIndexer > 0 && ColorIndexer % 50 == 0)
                                ColorIndexer = 0;
                            admin.ColorBg = ColorsList[ColorIndexer];
                            AdminsPool.Add(admin);
                            ColorIndexer++;
                        });
                    }
                }
            }
        }
        public string GetState(byte state)
        {
            if (state == 1)
                return "Admin";
            else return "Member";
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            Window window = new Window
            {
                Title = "Add New Admin",
                Content = new AddAdmin(),
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
                Program.MyApp.ShowBox("Please choose admin to remove");
                return;
            }
            var AdminsCoutner = Program.AdminsPool.Values.Where(p => p.State == 1).Count();
            if (AdminsCoutner > 1)
            {
                var Admin = item as Model.AdminsModel;
                Program.AdminsPool.Remove(int.Parse(Admin.ID));
                {
                    ShowPageRaws();
                    Database.DB.RemoveAdmin(Admin.ID);
                }
            }
            else
            {
                Program.MyApp.ShowBox("You can`t remove all admins");
                return;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var Converter = new BrushConverter();
            var item = memberDatagrid.SelectedItem;
            if (item == null)
            {
                Program.MyApp.ShowBox("Please choose admin to edit");
                return;
            }
            var Admin = item as Model.AdminsModel;
            Window window = new Window
            {
                Title = "Edit Admin",
                Content = new AddAdmin(true, Admin),
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = (Brush)Converter.ConvertFromString("Transparent"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }
        public FilterOption SelectedFilter = FilterOption.Name;
        private void FilterTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeItem = (ComboBoxItem)FilterTypes.SelectedItem;
            string value = typeItem.Content.ToString();
            if (value == "ID")
                SelectedFilter = FilterOption.ID;
            else if (value == "Name") SelectedFilter = FilterOption.Name;
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
