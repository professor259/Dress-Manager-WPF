using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Professor.View
{
    /// <summary>
    /// Логика взаимодействия для ToggleButtonUI.xaml
    /// </summary>
    public partial class ToggleButtonUI : UserControl, INotifyPropertyChanged
    {
        private string toggleBackground = "LightGray";
        private string toggleAlignment = "Left";
        private bool isTap = false;

        public bool IsTap
        {
            get
            {
                if (isTap)
                {
                    ToggleBackground = "DarkRed";
                    ToggleAlignment = "Right";
                }
                else
                {
                    ToggleAlignment = "Left";
                    ToggleBackground = "LightGray";
                }

                return isTap;
            }
            set
            {
                isTap = value;
                if (isTap)
                {
                    ToggleBackground = "DarkRed";
                    ToggleAlignment = "Right";
                }
                else
                {
                    ToggleAlignment = "Left";
                    ToggleBackground = "LightGray";
                }
                NotifyPropertyChanged();
            }
        }
        private bool? _isToggleButtonChecked;

        public bool? IsToggleButtonChecked
        {
            get => _isToggleButtonChecked;
            set
            {
                if (Equals(_isToggleButtonChecked, value))
                    return;

                _isToggleButtonChecked = value;
                //OnPropertyChanged();
            }
        }
        public string ToggleAlignment
        {
            get => toggleAlignment;
            set
            {
                toggleAlignment = value;
                NotifyPropertyChanged();
            }
        }

        public string ToggleBackground
        {
            get => toggleBackground;
            set
            {
                toggleBackground = value;
                NotifyPropertyChanged();
            }
        }
      

     

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ToggleButtonUI()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void ToggleTap(object sender, MouseButtonEventArgs e)
        {
            
            if (!IsTap)
                IsTap = true;
            else
                IsTap = false;
        }
    }
}
