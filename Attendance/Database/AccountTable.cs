using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace Attendance.Database
{
   public class AccountTable
    {
        public string Username;
        public string Password = "5";
        public string Email;
        public string IP;
        public uint ID;           
        public bool exists = false;
        public string Phone;
        public string FullName;
        public byte State;
        public AccountTable(string username)
        {
            if (username == null || username == "NONE") return;
            this.Username = username;
            this.Password = "";
            this.IP = "";
            this.ID = 0;
            DB.WithConnection(connection =>
            {
                {
                    using (var cmd = new SQLiteCommand("SELECT * FROM admins WHERE Username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exists = true;
                                this.Password = reader["Password"]?.ToString();
                                // this.IP = reader["IP"]?.ToString();
                                this.ID = Convert.ToUInt32(reader["ID"]);
                                this.Email = reader["Email"]?.ToString();
                                this.Phone = reader["Phone"]?.ToString();
                                this.FullName = reader["FullName"]?.ToString();
                                this.State = Convert.ToByte(reader["State"]);
                            }
                        }
                    }
                }

            });
        }
        public bool VerifyPassword(string password)
        {
            if (Password == password)
                return true;
            return false;
        }
    }
}
