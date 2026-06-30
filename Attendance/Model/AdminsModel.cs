using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Attendance.Model
{
    public class AdminsModel
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string StateView { get; set; }
        public byte State { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Charater { get; set; }
        public Brush ColorBg { get; set; }
        public bool EditedColor { get; set; }
    }
}
