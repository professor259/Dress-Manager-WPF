using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
namespace Attendance.ViewModel
{
    public class SearchVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public SearchVM()
        {
            _pageModel = new PageModel();
        }
    }
}
