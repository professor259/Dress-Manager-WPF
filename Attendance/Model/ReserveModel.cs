using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Attendance.Model
{
    public class ReserveModel
    {
        public Visibility EndButtonVisibility
        {
            get => Status == 2 ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility DeliverButtonVisibility
        {
            get => Status == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility EditButtonVisibility
        {
            get => Program.Account.State == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public int Index { get; set; }
        public int Status { get; set; }
        public string StatusView
        {
            get
            {
               string val = "Reserved";
                if(Status == 3)
                {
                    val = "Completed";
                }
                else if (Status == 2)
                {
                    val = "Delivered";
                }
                return val;
            }
        }
        public string ID { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Size
        {
            get
            {
                return DressModel.SizeS;
            }
        }
        public string Color
        {
            get
            {
                return DressModel.Color;
            }
        }
        public string DressID
        {
            get
            {
                return DressModel.ID;
            }
        }
        public string Price
        {
            get
            {
                return DressModel.RentPrice;
            }
        }
        public string Remain
        {
            get
            {
                int Val = int.Parse(Price) - int.Parse(Paid);
                return Val + " L.E";
            }
        }
        public string Paid { get; set; }

        public string Note { get; set; }
        public string Phone { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DeliverDate { get; set; }
        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
        public Model.DressModel DressModel;
    }
}
