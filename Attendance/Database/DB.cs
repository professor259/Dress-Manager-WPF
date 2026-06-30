using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO;

namespace Attendance.Database
{
    public class DB
    {
        private static string encPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance.db.enc");
     
        public static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attendance.db");
        public static string connectionString = $"Data Source={dbPath}";       
        public static int Index = 1;
        /*  public static SQLiteConnection GetConnection()
          {
              var conn = new SQLiteConnection(connectionString);
              conn.Open();
              return conn;
          }*/
        public static string GetDbFolder()
        {
            // Path to system temp folder
            string temp = Path.GetTempPath();

            // Create subfolder "dbs"
            string dbFolder = Path.Combine(temp, "dbs");

            // Ensure the folder exists
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            return dbFolder;
        }
        public static void ClearDbFolder()
        {
            string dbFolder = GetDbFolder();

            if (Directory.Exists(dbFolder))
            {
                foreach (string file in Directory.GetFiles(dbFolder))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not delete {file}: {ex.Message}");
                    }
                }
            }
        }
        public static void WithConnection(Action<SQLiteConnection> action)
        {
            // Step 1: Decrypt
            if (!File.Exists(encPath))
                throw new FileNotFoundException("Encrypted DB not found", encPath);
            string tempPath = GetDbFolder() + Guid.NewGuid() + ".db";
           //tempPath = Path.Combine(Path.GetTempPath(), "attendance.db");
            DbProtector.DecryptFile(encPath, tempPath);

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={tempPath}"))
                {
                    conn.Open();
                    action(conn);                    
                }
            }
            finally
            {
                // Step 2: Encrypt again (after SQLite connection fully disposed)
                DbProtector.EncryptFile(tempPath, encPath);
                File.Delete(tempPath);
            }
        }
    
        public static void LoadReservation()
        {
            try
            {
                DB.WithConnection(conn =>
                {
                    using (var cmd = new SQLiteCommand("SELECT * FROM reserve", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var Dress = Program.DressPool[int.Parse(reader["DressID"].ToString())];
                                AddReserveCollection(reader["ID"].ToString(), Convert.ToInt32(reader["Status"]), reader["User"].ToString(),
                                reader["Name"].ToString(), reader["Phone"].ToString(), reader["Paid"].ToString(),
                                reader["Address"].ToString(), reader["StartDate"].ToString(), reader["DeliverDate"].ToString(),
                                reader["EndDate"].ToString(), Dress, reader["Note"].ToString());
                                AddRent(Dress.ID);
                            }
                            catch
                            {
                                RemoveStudent(reader["ID"].ToString());
                            }
                        }
                    }
                });
                PostAvailability();
            }
            catch { }
        }
        public static void LoadDress()
        {
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM dress", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            AddDressCollection(reader);
                        }
                        catch
                        {
                            RemoveFaculty(reader["Code"].ToString());
                        }
                    }
                }
            });
        }
        public static void LoadAdmins()
        {
            try
            {
                DB.WithConnection(conn =>
                {
                    using (var cmd = new SQLiteCommand("SELECT * FROM admins", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            { 
                            AddAdminCollection(reader["Username"].ToString(), reader["Password"].ToString(), reader["ID"].ToString(),
                            reader["Email"].ToString(), reader["FullName"].ToString(), reader["Phone"].ToString(), Convert.ToByte(reader["State"]));
                            }
                            catch { }
                        }
                    }
                });
            }
            catch { }
        }
        public static string GetUserFullName(string user)
        {
            string val = "";
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("SELECT FullName FROM admins WHERE Username=@User", conn))
                {
                    cmd.Parameters.AddWithValue("@User", user);
                    var result = cmd.ExecuteScalar();
                    val =  result?.ToString();
                }
            });
            return val;
        }
        public static void LoadPurchase()
        {
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM purchase", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            AddPuchaseCollection(reader["ID"].ToString(), reader["Name"].ToString(), reader["Type"].ToString(),
                                        reader["Price"].ToString(), reader["Paid"].ToString(), reader["Time"].ToString(),
                                        reader["Note"].ToString(), reader["User"].ToString());
                        }
                        catch
                        {
                            RemoveClass(reader["ID"].ToString());
                        }
                    }
                }
            });
        }
        public static int AddDress(string name, int size, string color, string code, string Price, string RentPrice)
        {
            int val = 0;
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand(@"INSERT INTO dress (CostPrice,RentPrice,Name,Size,Color,Code)
VALUES (@CostPrice,@RentPrice,@Name,@Size,@Color,@Code);
SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@CostPrice", Price);
                    cmd.Parameters.AddWithValue("@RentPrice", RentPrice);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Size", size);
                    cmd.Parameters.AddWithValue("@Color", color);
                    cmd.Parameters.AddWithValue("@Code", code);
                    val = Convert.ToInt32(cmd.ExecuteScalar());
                }
            });
            return val;
        }
        public static void EditDress(string Name, int Size, string Color, string Code, string CostPrice, string RentPrice)
        {
                                DB.WithConnection(conn =>
                                {
                                    using (var cmd = new SQLiteCommand(@"UPDATE dress SET Name=@Name, Size=@Size, Color=@Color, Code=@Code, CostPrice=@CostPrice, RentPrice=@RentPrice WHERE Code=@Code", conn))
            {
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Size", Size);
                cmd.Parameters.AddWithValue("@Color", Color);
                cmd.Parameters.AddWithValue("@Code", Code);
                cmd.Parameters.AddWithValue("@CostPrice", CostPrice);
                cmd.Parameters.AddWithValue("@RentPrice", RentPrice);             
 //               cmd.Parameters.AddWithValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
                                });
        }

        public static void EditDressCollection(string Name, int Size, string Color, string Code, string CostPrice, string RentPrice)
        {
            try
            {
                Model.DressModel dress = new Model.DressModel();
                dress.ID = Code;
                Program.DressPool[int.Parse(dress.ID)].Name = Name;
                Program.DressPool[int.Parse(dress.ID)].ID = Code;
                Program.DressPool[int.Parse(dress.ID)].Color = Color;
                Program.DressPool[int.Parse(dress.ID)].Size = Size;
                Program.DressPool[int.Parse(dress.ID)].SizeS = GetSize(dress.Size);
                Program.DressPool[int.Parse(dress.ID)].CostPrice = CostPrice;
                Program.DressPool[int.Parse(dress.ID)].RentPrice = RentPrice;
                if (!string.IsNullOrEmpty(dress.Name))
                    Program.DressPool[int.Parse(dress.ID)].Charater = dress.Name.Substring(0, 1);
            }
            catch { }
        }
        public static void AddDressCollection(SQLiteDataReader reader)
        {
            try
            {
                Model.DressModel dress = new Model.DressModel();
                dress.Name = reader["Name"].ToString();
                dress.ID = reader["Code"].ToString();
                dress.Color = reader["Color"].ToString();
                dress.Size = Convert.ToInt32(reader["Size"]);
                dress.SizeS = GetSize(dress.Size);
                dress.CostPrice = reader["CostPrice"].ToString();
                dress.RentPrice = reader["RentPrice"].ToString();
                if (!string.IsNullOrEmpty(dress.Name))
                    dress.Charater = dress.Name.Substring(0, 1);
                Program.DressPool.Add(int.Parse(dress.ID), dress);
            }
            catch
            { }
        }
        public static bool VerifyAdminID(string ID)
        {
            try
            {
                bool val = false;
                DB.WithConnection(conn =>
                {
                    using (var cmd = new SQLiteCommand("SELECT 1 FROM admins WHERE ID=@ID LIMIT 1", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                val = false;
                            else val = true;
                        }
                    }
                });
                return val;
            }
            catch { }
            return false;
        }
        public static void RemoveAdmin(string id)
        {
                                            DB.WithConnection(conn =>
                                            {
                                                using (var cmd = new SQLiteCommand("DELETE FROM admins WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                            });
        }
        public static void RemoveFaculty(string id)
        {
                                                DB.WithConnection(conn =>
                                                {
                                                    using (var cmd = new SQLiteCommand("DELETE FROM dress WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                                });
        }

        public static void RemoveClass(string id)
        {
                                                    DB.WithConnection(conn =>
                                                    {
                                                        using (var cmd = new SQLiteCommand("DELETE FROM purchase WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                                    });
        }

        public static void RemoveStudent(string id)
        {
                                                        DB.WithConnection(conn =>
                                                        {
                                                            using (var cmd = new SQLiteCommand("DELETE FROM reserve WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                                        });
        }
        public static void AddAdmin(string username, string name, string password, string email, string phone, string id, byte state)
        {
                                                            DB.WithConnection(conn =>
                                                            {
                                                                using (var cmd = new SQLiteCommand(@"INSERT INTO admins (Username,Password,Email,FullName,State,Phone,ID)
VALUES (@Username,@Password,@Email,@FullName,@State,@Phone,@ID)", conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@FullName", name);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                                            });
        }
        public static int AddPuchase(int ID, string Name, string Type, string Price, string Paid, string Remain, string Time, string Note, string User, bool edit = false)
        {
            int Val = 0;
            if (!edit)
            {
                DB.WithConnection(conn =>
                {
                    using (var cmd = new SQLiteCommand(@"INSERT INTO purchase (Name,Type,User,Price,Paid,Remain,Time,Note)
VALUES (@Name,@Type,@User,@Price,@Paid,@Remain,@Time,@Note);
SELECT last_insert_rowid();", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@User", User);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@Paid", Paid);
                        cmd.Parameters.AddWithValue("@Remain", Remain);
                        cmd.Parameters.AddWithValue("@Time", Time);
                        cmd.Parameters.AddWithValue("@Note", Note);
                        Val = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                });
            }
            else
            {
                DB.WithConnection(conn =>
                {
                    using (var cmd = new SQLiteCommand(@"UPDATE purchase SET Name=@Name,User=@User,Price=@Price,Type=@Type,Paid=@Paid,Remain=@Remain,Time=@Time,Note=@Note WHERE ID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@User", User);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@Paid", Paid);
                        cmd.Parameters.AddWithValue("@Remain", Remain);
                        cmd.Parameters.AddWithValue("@Time", Time);
                        cmd.Parameters.AddWithValue("@Note", Note);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.ExecuteNonQuery();
                        Val = 0;
                    }
                });
               
            }
            return Val;
        }

        public static string AddReservation(string Name, string Phone, string Paid, string Address, string StartDate, string DeliverDate, string EndDate, string DressIDContext, string NoteContext)
        {
            string ID = "";
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand(@"INSERT INTO reserve (Name,Phone,Status,User,Paid,Address,StartDate,Note,DeliverDate,DressID,EndDate)
VALUES (@Name,@Phone,1,@User,@Paid,@Address,@StartDate,@Note,@DeliverDate,@DressID,@EndDate);
SELECT last_insert_rowid();", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@User", Program.Account.Username);
                    cmd.Parameters.AddWithValue("@Paid", Paid);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Note", NoteContext);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@DeliverDate", DeliverDate);
                    cmd.Parameters.AddWithValue("@DressID", DressIDContext);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    ID = cmd.ExecuteScalar().ToString();
                }
            });
            return ID;
        }     
        public static void EditReserve(string ID, int Status, string DressID, string Name, string Phone, string Paid, string Address, string StartDate, string DeliverDate, string EndDate,string Note)
        {
                                                                            DB.WithConnection(conn =>
                                                                            {
                                                                                using (var cmd = new SQLiteCommand(@"UPDATE reserve SET Name=@Name, Status=@Status, User=@User, DressID=@DressID, Phone=@Phone, Paid=@Paid,
Address=@Address, StartDate=@StartDate, Note=@Note, DeliverDate=@DeliverDate, EndDate=@EndDate WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@User", Program.Account.Username);
                cmd.Parameters.AddWithValue("@DressID", DressID);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@Paid", Paid);
                cmd.Parameters.AddWithValue("@Note", Note);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                cmd.Parameters.AddWithValue("@DeliverDate", DeliverDate);
                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
                                                                            });
        }
        public static void EditAdmin(string name, string password, string email, string phone, string id)
        {
                                                                                DB.WithConnection(conn =>
                                                                                {
                                                                                    using (var cmd = new SQLiteCommand("UPDATE admins SET Password=@Password,Email=@Email,FullName=@FullName,Phone=@Phone WHERE ID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@FullName", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
                                                                                });
                                                                            }
        public static bool VerifyAdminUsername(string username)
        {
            bool val = false;
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("SELECT 1 FROM admins WHERE username=@username LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            val = false;
                        }
                        else val = true;
                                                                                            }
                }
            });           
            return val;
        }
        public static void CompleteReserve(string ID, string Paid)
        {
            Program.ReservePool[int.Parse(ID)].Status = 3;
            Program.ReservePool[int.Parse(ID)].Paid = Paid;          
            CheckForCompleteReservation(Program.ReservePool[int.Parse(ID)].DressID);
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("UPDATE reserve SET Status = 3, Paid=@Paid WHERE ID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Paid", Paid);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            });
        }
        public static void CheckForCompleteReservation(String DressID)
        {
            var Pool = Program.ReservePool.Values.Where(p => p.DressID == DressID && p.Status == 1).ToArray();
            if (Pool != null && Pool.Length > 0)
            {
                Program.DressPool[int.Parse(DressID)].Status = 1;
            }
            else
            {
                Program.DressPool[int.Parse(DressID)].Status = 0;
            }

        }
        public static void DeliverReserve(string ID, string Paid)
        {
            Program.ReservePool[int.Parse(ID)].Status = 2;
            Program.ReservePool[int.Parse(ID)].Paid = Paid;
            Program.DressPool[int.Parse(Program.ReservePool[int.Parse(ID)].DressID)].Status = 2;
            DB.WithConnection(conn =>
            {
                using (var cmd = new SQLiteCommand("UPDATE reserve SET Status = 2, Paid=@Paid WHERE ID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Paid", Paid);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            });
        }
        public static void PostAvailability()
        {
            foreach (var dress in Program.DressPool.Values)
            {
                var Pool = Program.ReservePool.Values.Where(p => p.DressID == dress.ID && p.Status == 1).ToArray();
                if (Pool != null && Pool.Length > 0)
                {
                    dress.Status = 1;
                }
                Pool = Program.ReservePool.Values.Where(p => p.DressID == dress.ID && p.Status == 2).ToArray();
                if (Pool != null && Pool.Length > 0)
                {
                    dress.Status = 2;
                }
            }
        }
        public static void UpdateAvailability(string ID, int Status)
        {
            if (Program.DressPool.ContainsKey(int.Parse(ID)))
            {
                if (Program.DressPool[int.Parse(ID)].Status != 2)
                    Program.DressPool[int.Parse(ID)].Status = Status;
            }
        }
        public static bool AddRent(string DressKey)
        {
            foreach (var Dres in Program.DressPool.Values)
            {
                if (Dres.ID == DressKey)
                {
                    Dres.TotalRent += 1;
                    return true;
                }
            }
            return false;
        }
     
        public static void AddReserveCollection(string ID, int Status, string User, string Name, string Phone, string Paid, string Address, string StartDate, string DeliverDate, string EndDate, Model.DressModel DressModel,string Note, bool edit = false)
        {
            Model.ReserveModel student = new Model.ReserveModel();
            student.Name = Name;
            student.Phone = Phone;
            student.Paid = Paid;
            student.Note = Note;
            student.ID = ID;
            student.Address = Address;
            student.StartDate = StartDate;
            student.User = User;
            student.DeliverDate = DeliverDate;
            student.Status = Status;
            student.EndDate = EndDate;
            student.DressModel = DressModel;
            student.Charater = student.Name.Substring(0, 1);
            if (!edit)
                Program.ReservePool.Add(int.Parse(ID), student);
            else
            {
                Program.ReservePool[int.Parse(ID)].Name = Name;
                Program.ReservePool[int.Parse(ID)].Phone = Phone;
                Program.ReservePool[int.Parse(ID)].Status = Status;
                Program.ReservePool[int.Parse(ID)].Paid = Paid;
                Program.ReservePool[int.Parse(ID)].ID = ID;
                Program.ReservePool[int.Parse(ID)].User = User;
                Program.ReservePool[int.Parse(ID)].Note = Note;
                Program.ReservePool[int.Parse(ID)].Address = Address;
                Program.ReservePool[int.Parse(ID)].StartDate = StartDate;
                Program.ReservePool[int.Parse(ID)].DeliverDate = DeliverDate;
                Program.ReservePool[int.Parse(ID)].EndDate = EndDate;
                Program.ReservePool[int.Parse(ID)].DressModel = DressModel;
                Program.ReservePool[int.Parse(ID)].Charater = student.Name.Substring(0, 1);
            }
        }      
        public static void AddAdminCollection(string username, string password, string id, string email, string name, string phone, byte state)
        {
            Model.AdminsModel admin = new Model.AdminsModel();
            admin.Username = username;
            admin.Password = password;
            admin.ID = id;
            admin.Email = email;
            admin.Name = name;
            admin.Phone = phone;
            admin.State = state;
            if (admin.Name != string.Empty)
                admin.Charater = admin.Name.Substring(0, 1);
            Program.AdminsPool.Add(int.Parse(id), admin);
        }

        public static string GetSize(int size)
        {
            if (size == 0)
                return "50-60";
            else if (size == 1)
                return "60-70";
            else if (size == 2)
                return "70-80";
            else if (size == 3)
                return "80-90";
            else if (size == 4)
                return "90-100";
            else if (size == 4)
                return "100+";
            return "100+";
        }
        public static void AddAttendanceCollection(string uid, string studentid, long ticks, string Class)
        {

        }
        public static Model.PurchaseModel AddPuchaseCollection(string Id, string Name, string Type, string Price, string Paid, string Time, string Note, string User, bool edit = false)
        {
            if (!edit)
            {
                Model.PurchaseModel Purchase = new Model.PurchaseModel();
                Purchase.ID = Id;
                Purchase.Name = Name;
                Purchase.Type = Type;
                Purchase.Price = Price;
                Purchase.User = User;
                Purchase.Paid = Paid;
                Purchase.Time = Time;
                Purchase.Note = Note;
                Purchase.UserName = Database.DB.GetUserFullName(User);
                Program.PurchasePool.Add(int.Parse(Purchase.ID), Purchase);
                return Purchase;
            }
            else if (Program.PurchasePool.ContainsKey(int.Parse(Id)))
            {
                int ID = int.Parse(Id);
                Program.PurchasePool[ID].Name = Name;
                Program.PurchasePool[ID].Type = Type;
                Program.PurchasePool[ID].Price = Price;
                Program.PurchasePool[ID].Paid = Paid;
                Program.PurchasePool[ID].Time = Time;
                Program.PurchasePool[ID].Note = Note;
                return Program.PurchasePool[ID];
            }
            return null;
        }


        public static void EditAdminCollection(string username, string password, string id, string email, string name, string phone)
        {
            if (Program.AdminsPool.ContainsKey(int.Parse(id)))
            {
                int ID = int.Parse(id);
                Program.AdminsPool[ID].Password = password;
                Program.AdminsPool[ID].Email = email;
                Program.AdminsPool[ID].Name = name;
                Program.AdminsPool[ID].Phone = phone;
                Program.AdminsPool[ID].Charater = name.Substring(0, 1);
            }
        }

        public static bool VerifyFacultyName(string name)
        {
            foreach (var faculty in Program.DressPool.Values)
            {
                if (faculty.ID.ToLower() == name.ToLower())
                    return false;
            }
            return true;
        }
       


    }
}
