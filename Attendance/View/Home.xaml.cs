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

namespace Attendance.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.TotalReservation.Text = Program.ReservePool.Count.ToString();
            this.ActiveReserve.Text = Program.ReservePool.Values.Where(p => p.Status == 1 || p.Status == 2).Count().ToString();
            this.TotalPurchase.Text = Program.ReservePool.Values.Where(p => p.Status == 2).Count().ToString();
            this.TotalDress.Text = Program.DressPool.Count.ToString();
            this.CompeletedReservation.Text = Program.ReservePool.Values.Where(p => p.Status == 3).Count().ToString();
            this.TotalPuchases.Text = Program.PurchasePool.Count.ToString();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {           
        }
    }
}
