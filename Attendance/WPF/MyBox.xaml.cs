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
using System.Windows.Shapes;

namespace Attendance.WPF
{
    /// <summary>
    /// Interaction logic for MyBox.xaml
    /// </summary>
    public partial class MyBox : Window
    {
        public bool Terminate = false;
        public MyBox()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            if (Terminate)
                Environment.Exit(0);
        }
        public void WriteLine(string msg,bool terminate = false)
        {
            Message.Text = msg;
            Terminate = terminate;
        }
    }
}
