using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Attendance.Model
{
    public class ReportModel
    {
        public int Index { get; set; }
        public string OrderID { get; set; }
        public string Type { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ServiceName { get; set; }

        public string Cost { get; set; }
        public string User { get; set; }
        public string DateShow { get; set; }
        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
        public bool IsPresent { get; set; }
    }
}
