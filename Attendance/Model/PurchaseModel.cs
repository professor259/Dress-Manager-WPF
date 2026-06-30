using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Attendance.Model
{
    public class PurchaseModel
    {
        public Visibility EditButtonVisibility
        {
            get => Program.Account.State == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public int Index { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string Paid { get; set; }
        public string Remain
        {
            get
            {
                int Val = int.Parse(Price.Split(' ')[0]) - int.Parse(Paid.Split(' ')[0]);
                return Val + " L.E";
            }
        }
        public string User { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public string Time { get; set; }     
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
    }
}
