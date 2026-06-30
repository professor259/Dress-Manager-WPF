using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
namespace Attendance
{
    public class Program
    {
        public static bool LaunchedViaStartup { get; set; }
        public static Database.AccountTable Account;
        public static string Username;
        public static string Password;    
        public static WPF.GUI Application;
        public static App MyApp;
        public static System.SafeDictionary<int,string> PurchaseType = new System.SafeDictionary<int, string>();
        public static System.SafeDictionary<int, Model.MaterialModel> MaterialPool = new System.SafeDictionary<int, Model.MaterialModel>();
        public static System.SafeDictionary<int, Model.DressModel> DressPool = new System.SafeDictionary<int, Model.DressModel>();
      
        public static System.SafeDictionary<int, Model.PurchaseModel> PurchasePool = new System.SafeDictionary<int, Model.PurchaseModel>();
        public static System.Text.Encoding Encoding = System.Text.Encoding.ASCII;
        private static System.SafeDictionary<int, Model.ReserveModel> _studentspool = new System.SafeDictionary<int, Model.ReserveModel>();
        public static System.SafeDictionary<int,Model.ReserveModel> ReservePool
        {
            get
            {
                return _studentspool;
            }
            set
            {
                _studentspool = value;               
            }
        }
        private static System.SafeDictionary<int, Model.AdminsModel> _adminspool = new System.SafeDictionary<int, Model.AdminsModel>();
        public static System.SafeDictionary<int, Model.AdminsModel> AdminsPool
        {
            get
            {
                return _adminspool;
            }
            set
            {
                _adminspool = value;
            }
        }
        public static void Shutdown()
        {                      
           // Application.ExitAll();
        }
        public static bool IsConnectedToInternet()
        {           
            string host = "http://www.gstatic.com/generate_204";  
bool result = false;            
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(host);
                request.KeepAlive = false;
                request.Timeout = 5000;
                using (var response = (HttpWebResponse)request.GetResponse())
                    return true;
            }
            catch { }
            return result;
        }
    }
}
