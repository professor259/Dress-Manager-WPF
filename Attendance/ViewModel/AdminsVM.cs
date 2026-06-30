using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
namespace Attendance.ViewModel
{
    class AdminsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public AdminsVM()
        {
            _pageModel = new PageModel();
        }
    }
}
