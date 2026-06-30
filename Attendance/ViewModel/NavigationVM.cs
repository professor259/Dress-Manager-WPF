using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Utilities;
using System.Windows.Input;

namespace Attendance.ViewModel
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }      
        public ICommand HomeCommand { get; set; }
        public ICommand MaterialCommand { get; set; }
        public ICommand FacultyCommand { get; set; }
        public ICommand StudentsCommand { get; set; }
        public ICommand AdminsCommand { get; set; }
        public ICommand ClassCommand { get; set; }
        public ICommand AttendanceCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public StudentsVM studentsVM;
        public void Students(object obj)
        {
            if(studentsVM == null)
            {
                studentsVM = new StudentsVM();
            }
            CurrentView = studentsVM;
        }
        public MaterialVM materialsVM;
        public void Materials(object obj)
        {
            if (materialsVM == null)
            {
                materialsVM = new MaterialVM();
            }
            CurrentView = materialsVM;
        }
        public FacultyVM facultysVM;
        public void Faculty(object obj)
        {
            if (facultysVM == null)
            {
                facultysVM = new FacultyVM();
            }
            CurrentView = facultysVM;
        }
        public AttendanceVM attendanceVM;
        public void Attendance(object obj)
        {
            if (attendanceVM == null)
            {
                attendanceVM = new AttendanceVM();
            }
            CurrentView = attendanceVM;
        }
        public SearchVM searchVM;
        public void Search(object obj)
        {
            if (searchVM == null)
            {
                searchVM = new SearchVM();
            }
            CurrentView = searchVM;
        }
        public ClassVM classVM;
        public void Classes(object obj)
        {
            if (classVM == null)
            {
                classVM = new ClassVM();
            }
            CurrentView = classVM;
        }
        private AdminsVM adminsVM;
        private void Admins(object obj)
        {
            if (adminsVM == null)
            {
                adminsVM = new AdminsVM();
            }
            CurrentView = adminsVM;
        }
        private SettingsVM settingVM;
        private void Settings(object obj)
        {
            if (settingVM == null)
            {
                settingVM = new SettingsVM();
            }
            CurrentView = settingVM;
        }
        public void Home(object obj) => CurrentView = new HomeVM();       
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            MaterialCommand = new RelayCommand(Materials);
            AdminsCommand = new RelayCommand(Admins);
            StudentsCommand = new RelayCommand(Students);
            ClassCommand = new RelayCommand(Classes);
            AttendanceCommand = new RelayCommand(Attendance);
            SearchCommand = new RelayCommand(Search);
            SettingsCommand = new RelayCommand(Settings);
            FacultyCommand = new RelayCommand(Faculty);
            CurrentView = new HomeVM();

        }
    }
}
