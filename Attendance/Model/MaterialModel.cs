using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Attendance.Model
{
    public class MaterialModel
    {
        public int Index { get; set; }
        public string ReserveID { get; set; }
        public string DressID { get; set; }
        public string DressName { get; set; }
        public string Deliver { get; set; }       
        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
    }
}
