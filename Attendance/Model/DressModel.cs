using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Attendance.Model
{
    public class DressModel
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }

        public int Size { get; set; }
        public string SizeS { get; set; }

        public string Color { get; set; }
        public int Status { get; set; }
        public string StatusView
        {
            get
            {
                string val = "Available";
                if (Status == 1)
                {
                    val = "Reserved";
                }
                else if (Status == 2)
                {
                    val = "Delivered";
                }
                return val;
            }
        }
        public string TotalIncome
        {
            get
            {
                int PriceRent = int.Parse(RentPrice);
                int rented = TotalRent;

                return (PriceRent * rented).ToString();
            }
        }
        public string RentPrice { get; set; }
        public string CostPrice { get; set; }

        public int TotalRent { get; set; }

        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
    }
}
