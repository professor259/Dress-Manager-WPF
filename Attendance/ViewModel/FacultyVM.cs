using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
namespace Attendance.ViewModel
{
    public class FacultyVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public FacultyVM()
        {
            _pageModel = new PageModel();
        }
    }
}
