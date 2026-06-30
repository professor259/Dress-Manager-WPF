using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Attendance.Model
{
    public class SearchModel
    {
        public int Index { get; set; }
        public string OrderID { get; set; }      
        public string DressName { get; set; }
        public string RenterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliverDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Cost { get; set; }
        public string User { get; set; }
        public string StartDateShow { get; set; }
        public string DeliverDateShow { get; set; }
        public string EndDateShow { get; set; }
        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
        public bool IsPresent { get; set; }
    }
}
