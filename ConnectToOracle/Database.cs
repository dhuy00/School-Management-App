using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlClient;
// <<<<<<< HEAD
using System.Security.Principal;
// =======
using System.Web;
// >>>>>>> origin/Huy-Merge-V2

namespace ConnectToOracle
{
    class Database
    {
        private OracleConnection con = null;
        private static Database _instance;
        bool isLogIn = false;
        List<string> roleName = new List<string>();
        private static string userName;

        //Tao ra doi tuong de thuc thi cac ham
        private Database()
        {
        
        }

        //Ngan chan viec tu dong don giai phong tai nguyen cua C#
        internal class OpenedContext : IDisposable
        {
            private OracleConnection _connection;

            public OpenedContext(OracleConnection conn)
            {
                _connection = conn;
                if (_connection.State != System.Data.ConnectionState.Open) _connection.Open();
            }

            public void Dispose()
            {
                if (_connection.State != System.Data.ConnectionState.Closed) _connection.Close();
            }

        }

        //Thuc hien viec login va set isLogin bang true nhung lan sau ham khong con hoat dong
        public List<String> logInToDataBse(string account, string password, ref Exception exOutput, string privilegeName)
        {
            // Nhớ hỏi nếu bỏ return thì lần sau vẫn log in bằng user khác được ko?
            if (isLogIn)
            {
                return roleName;
            }
            String connectionString;
            if (privilegeName == "SYSDBA")
            {
                connectionString = $@"Data Source= (DESCRIPTION =
        (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))
        (CONNECT_DATA =
        (SERVER = DEDICATED)
        (SERVICE_NAME = PDB_PJ_ATBM)
        )
        ); User Id = {account};password={password};DBA Privilege = SYSDBA;"; //PDB_PJ_ATBM
            }
            else
            {
                connectionString = $@"Data Source= (DESCRIPTION =
        (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))
        (CONNECT_DATA =
        (SERVER = DEDICATED)
        (SERVICE_NAME = PDB_PJ_ATBM)
        )
        ); User Id = {account};password={password};"; //PDB_PJ_ATBM
            }
            bool check = true;
            con = new OracleConnection(connectionString);

            try
            {
                con.Open();
                isLogIn = true;
                userName = account;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOutput = ex;
                check = false;
                isLogIn = false; // Trường hợp Log in lỗi
            }
            if (!check)
            {
                con = null;
                return roleName;
            }
            //Lấy danh sách role hiện có và lấy role cần kiếm tra(SINHVIEN,GIANGVIEN,....)
            if (privilegeName == "SYSDBA")
            {
                roleName.Add("SYSDBA");
            }
            else
            {
                OracleCommand cmd = new OracleCommand("select * from session_roles");

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roleName.Add(reader.GetString(0));
                }
            }
            return roleName;
        }
        public static Database getInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }
            return _instance;
        }
        //ham lay tat ca cac user neu chua dang nhap thi khong co du lieu
        public List<List<String>> getAllUsers(ref Exception  exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>>  result = new List<List<string>>();
            using(new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_ALL_USERS",con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<String> temp = new List<String>();
                        temp.Add(dr.Field<String>(0));
                        temp.Add(dr.Field<String>(1));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        //Lấy role hiện tại của user 
        public string getCurrentRole()
        {
            string currentRole = string.Empty;
            if (this.roleName.Contains("SINHVIEN"))
            {
                currentRole = "SNHVIEN";
            }
            else if (this.roleName.Contains("TRUONGKHOA"))
            {
                currentRole = "TRUONGKHOA";
            }
            else if (this.roleName.Contains("TRUONGDONVI"))
            {
                currentRole = "TRUONGDONVI";
            }
            else if (this.roleName.Contains("GIAOVU"))
            {
                currentRole = "GIAOVU";
            }
            else if (this.roleName.Contains("GIANGVIEN"))
            {
                currentRole = "GIANGVIEN";
            }
            else if (this.roleName.Contains("NHANVIENCOBAN"))
            {
                currentRole = "NHANVIENCOBAN";
            }
            return currentRole;
        }

        public List<String> getAllRoles(ref Exception exOutput)
        {
            if (isLogIn == false) return new List<String>();
            List<String> result = new List<String>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_ALL_ROLES", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(dr.Field<String>(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        public List<String> getAllSystemPrivis(Exception exOutput)
        {
            if(isLogIn == false) return new List<String>();
            List<String> result = new List<String>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_ALL_PRIVS", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(dr.Field<String>(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        public List<List<String>> getSysPrivsOfAAgent(String agent,Exception exOutput)
        {
            if (isLogIn == false) return new List<List<String>>();
            List<List<String>> result = new List<List<String>>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("CHECK_PRIVIS_A_ACTOR", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("USER_NAME",OracleDbType.Varchar2).Value = agent;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<String> temp = new List<String>();
                        temp.Add(dr.Field<String>(0));
                        temp.Add(dr.Field<String>(1));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }
        public List<List<String>> getALLPrivilegesOfRole(string name, ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand($"select DISTINCT PRIVILEGE,TABLE_NAME from ROLE_TAB_PRIVS WHERE ROLE = '{name}'");

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader.GetString(0));
                        temp.Add(reader.GetString(1));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        
        //Lấy quyền trên bảng của một user 
        public List<List<String>> getALLPrivilegesOfUser(string name, ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {   // Bảng DBA_TAB_PRIVS cho ra quyền trên bảng của bao gồm cả role và user
                    OracleCommand cmd = new OracleCommand($"select PRIVILEGE,TABLE_NAME from DBA_TAB_PRIVS  WHERE GRANTEE = '{name}'");// limit cho máy chạy

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader.GetString(0));
                        temp.Add(reader.GetString(1));
                        result.Add(temp);
                    }

                    OracleCommand cmd2 = new OracleCommand($"select DISTINCT PRIVILEGE,TABLE_NAME from All_COL_PRIVS  WHERE GRANTEE = '{name}'");// limit cho máy chạy

                    cmd2.Connection = con;
                    cmd2.CommandType = CommandType.Text;
                    OracleDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader2.GetString(0));
                        temp.Add(reader2.GetString(1));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }


        public void createUser(string name, string pass, ref Exception exOutput)
        {
            if (isLogIn == false) return;

            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "CREATE_USER";
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pi_username", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("pi_password", OracleDbType.Varchar2).Value = pass;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Create Success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
        }

        //Create role function
        public void createRole(string name, string pass, ref Exception exOutput)
        {
            if (isLogIn == false) return;

            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "CREATE_ROLE";
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pi_username", OracleDbType.Varchar2).Value = name;
                    cmd.Parameters.Add("pi_password", OracleDbType.Varchar2).Value = pass;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Create Success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
        }

        public List<List<String>> getAllColumn(string tableName, ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT column_name FROM USER_TAB_COLUMNS WHERE table_name = :table_name";
                    OracleCommand cmd = new OracleCommand(query);
                    cmd.Parameters.Add(new OracleParameter(":table_name", tableName));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader.GetString(0));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }

            return result;
        }

        //Câp quyền trên table cho user hoặc role 
        public void GrantPermissionToUserOrRole(string permissionType, string tableName, string targetName, int isGrantOption, string columnNamesSelect, string columnNamesUpdate)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("Sp_Grant_Permission_To_User_Or_Role", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("permission_type", OracleDbType.Varchar2)).Value = permissionType;
                        cmd.Parameters.Add(new OracleParameter("tablename", OracleDbType.Varchar2)).Value = tableName;
                        cmd.Parameters.Add(new OracleParameter("target_name", OracleDbType.Varchar2)).Value = targetName;
                        cmd.Parameters.Add(new OracleParameter("is_grant_option", OracleDbType.Int32)).Value = isGrantOption;
                        cmd.Parameters.Add(new OracleParameter("column_name_select", OracleDbType.Varchar2)).Value = columnNamesSelect;
                        cmd.Parameters.Add(new OracleParameter("column_name_update", OracleDbType.Varchar2)).Value = columnNamesUpdate;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Thực thi procedure cấp role cho user 
        public void GrantRoleToUser(string userName, string roleName)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("Sp_Grant_Role_To_User", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("rolename", OracleDbType.Varchar2)).Value = roleName;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Thay đổi mật khẩu của một user  
        public void EditUser(string userName, string password)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ChangeUserPassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("new_password", OracleDbType.Varchar2)).Value = password;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Thay đổi mật khẩu của một role  
        public void EditRole(string userName, string password)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ChangeRolePassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("new_password", OracleDbType.Varchar2)).Value = password;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Lấy các quyền của một user trên một table 
        public List<List<String>> GetUserPrivOnTable(string userName, string tableName, ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT PRIVILEGE FROM all_tab_privs WHERE table_name = :table_name AND grantee = :user_name";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":table_name", tableName));
                    cmd.Parameters.Add(new OracleParameter(":user_name", userName));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader.GetString(0));
                        result.Add(temp);
                    }

                    //Lay quyen update
                    string query2 = "SELECT DISTINCT PRIVILEGE FROM All_COL_PRIVS WHERE table_name = :table_name AND grantee = :user_name";
                    OracleCommand cmd2 = new OracleCommand(query2);

                    cmd2.Parameters.Add(new OracleParameter(":table_name", tableName));
                    cmd2.Parameters.Add(new OracleParameter(":user_name", userName));

                    cmd2.Connection = con;
                    cmd2.CommandType = CommandType.Text;
                    OracleDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        List<String> temp = new List<String>();
                        temp.Add(reader2.GetString(0));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
          return result;
        }



        //XÓA USER 
        public void dropUser(string name, ref Exception exOutput)
        {
            if (isLogIn == false) return;
            using (new OpenedContext(con))
            {
                try
                {

                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "DROP_USER";
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pi_username", OracleDbType.Varchar2).Value = name;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Drop Success!");
                  }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
        }

        public void dropRole(string name, ref Exception exOutput)
        {
            if (isLogIn == false) return;
            using (new OpenedContext(con))
            {
                try
                {

                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "DROP_ROLE";
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pi_username", OracleDbType.Varchar2).Value = name;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Drop Success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
        }


        public List<List<String>> getAllTables(ref Exception exOutput)
    {
        if (isLogIn == false) return new List<List<string>>();
        List<List<String>> result = new List<List<string>>();
        using (new OpenedContext(con))
        {
            try
            {
                OracleCommand cmd = new OracleCommand("SELECT table_name FROM tabs");

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    List<String> temp = new List<String>();
                    temp.Add(reader.GetString(0));
                    result.Add(temp);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOutput = ex;
            }
        }
        return result;

    }
        //Cấp quyền cho user hoặc role không có admin option 
        public void GrantPrileges(string userName, string privileges)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("GRANT_PRIVILEGES", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("USER_NAME", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("QUYEN", OracleDbType.Varchar2)).Value = privileges;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Cấp quyền cho user hoặc role có admin option 
        public void GrantPrilegesWithAdminOption(string userName, string privileges)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("GRANT_ADMIN_OPTION", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("USER_NAME", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("QUYEN", OracleDbType.Varchar2)).Value = privileges;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }


        //Thu hồi quyền không có admin option 
        public void RevokePrileges(string userName, string privileges)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("REVOKE_PRIVILEGES", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("USER_NAME", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("QUYEN", OracleDbType.Varchar2)).Value = privileges;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Thu hồi quyền trên table
        public void RevokePrilegesOnTable(string userName, string tableName)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("Sp_RevokePrivilegeOnTable", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("tablename", OracleDbType.Varchar2)).Value = tableName;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            // Hiển thị thông báo tương ứng
            if (success)
            {
              //  MessageBox.Show("Revoke Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Revoke Procedure executed failed");
            }
        }

        //Thu hồi quyền có admin option 
        public void RevokePrilegesWithAdminOption(string userName, string privileges)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("REVOKE_ADMIN_OPTION", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("USER_NAME", OracleDbType.Varchar2)).Value = userName;
                        cmd.Parameters.Add(new OracleParameter("QUYEN", OracleDbType.Varchar2)).Value = privileges;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            // Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Procedure executed successfully");
            }
            else
            {
                MessageBox.Show("Procedure executed failed");
            }
        }

        //Lấy thông tin của một nhân sự
        public DataRow GetEmployeeByID(string userName)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.view_NhanVienCoBan WHERE MANV = :user_name";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":user_name", userName));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd)) 
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Lấy role của một nhân viên
        public string GetEmployeeRole(string username)
        {
            string role = "Sinh viên";
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    
                    string query = "SELECT * FROM ADMIN_OLS.view_NhanVienCoBan";
                    OracleCommand cmd = new OracleCommand(query, con);

                    
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    if(dt == null)
                    {
                        MessageBox.Show("test");
                    }

                    if(dt.Rows.Count > 0)
                    {
                        role = dt.Rows[0]["VAITRO"].ToString();
                    }
                    else
                    {
                        role = "Sinh viên";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return role;
        }

        //Lấy thông tin bảng sinh viên
        public DataTable GetAllStudent()
        {
            DataTable dt = new DataTable();
            using(new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.SINHVIEN";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter= new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message );
                }
            }
            return dt;
        }


        //Tìm kiếm theo mã sinh viên 
        public DataTable GetStudentSearch(string studentID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.SINHVIEN WHERE MASV LIKE :studentID";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.Parameters.Add(new OracleParameter("studentID", "%" + studentID + "%"));

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Lấy thông tin tất cả đơn vị
        public DataTable GetAllDepartments()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.DONVI";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Lấy thông tin kế hoạch mở học phần 
        public DataTable GetAllOpenCourse()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.KHMO";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }


        //Lấy mã và tên sinh viên 
        public DataTable GetStudentIdName()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT MASV, HOTEN FROM ADMIN_OLS.SINHVIEN";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable GetAllOpenCourseWithTeacher()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.view_PhanCongJoin";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        //Lấy thông tin từ bảng học phần 
        public DataTable GetAllCourse()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.HOCPHAN";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Xem thông tin giảng dạy liên quan đến giảng viên
        public DataTable GetTeacherAssignment()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_GV_SELECT_PHANCONG";
// <<<<<<< HEAD
// =======
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Xem tất cả phân công
        public DataTable GetAllAssignment()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.PHANCONG";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Lấy các dòng phân công do văn phòng khoa phụ trách 
        public DataTable GetFacultyAssignment()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_PHANCONG_VANPHONGKHOA";
// >>>>>>> origin/Huy-Merge-V2
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Xem thông tin đăng ký liên quan đến các lớp học phần mà giảng viên được phân công giảng dạy 
        public DataTable GetTeacherRegister()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_GV_SELECT_DANGKY";
// <<<<<<< HEAD
// =======
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Xem thông tin bảng đăng ký 
        public DataTable GetRegister()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.DANGKY";
// >>>>>>> origin/Huy-Merge-V2
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        // Lấy thông tin đăng ký của Sinh Viên
        public DataTable GetStuRegister()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.DANGKY";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        // Sinh Viên Đăng Ký 
        public void RegCourse(string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "INSERT INTO ADMIN_OLS.DANGKY (MASV, MAGV, MAHP, HK, NAM, MACT) " +
                        "VALUES (:student_ID, :teacher_ID, :course_ID, :course_semester, :course_year, :curriculum_ID)";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":student_ID", OracleDbType.Varchar2)).Value = userName.ToUpper();
                    cmd.Parameters.Add(new OracleParameter(":teacher_ID", OracleDbType.Varchar2)).Value = teacherID;
                    cmd.Parameters.Add(new OracleParameter(":course_ID", OracleDbType.Varchar2)).Value = courseID;
                    cmd.Parameters.Add(new OracleParameter(":course_semester", OracleDbType.Int16)).Value = semester;
                    cmd.Parameters.Add(new OracleParameter(":course_year", OracleDbType.Int16)).Value = year;
                    cmd.Parameters.Add(new OracleParameter(":curriculum_ID", OracleDbType.Varchar2)).Value = curriculumID;


                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    int aff = cmd.ExecuteNonQuery();
                    MessageBox.Show(aff + " rows were affected.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return;
        }

        public void DeleteRegCourse(string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "DELETE FROM ADMIN_OLS.DANGKY " +
                        "WHERE MASV = :student_ID AND MAGV = :teacher_ID AND MAHP = :course_ID AND HK = :course_semester AND NAM = :course_year AND MACT = :curriculum_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":student_ID", OracleDbType.Varchar2)).Value = userName;
                    cmd.Parameters.Add(new OracleParameter(":teacher_ID", OracleDbType.Varchar2)).Value = teacherID;
                    cmd.Parameters.Add(new OracleParameter(":course_ID", OracleDbType.Varchar2)).Value = courseID;
                    cmd.Parameters.Add(new OracleParameter(":course_semester", OracleDbType.Int16)).Value = semester;
                    cmd.Parameters.Add(new OracleParameter(":course_year", OracleDbType.Int16)).Value = year;
                    cmd.Parameters.Add(new OracleParameter(":curriculum_ID", OracleDbType.Varchar2)).Value = curriculumID;


                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    int aff = cmd.ExecuteNonQuery();
                    MessageBox.Show(aff + " rows were affected.");
                    return;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        public void updateAddress(string address)
        {
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "UPDATE ADMIN_OLS.SINHVIEN " +
                        "SET DCHI = :address";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":address", OracleDbType.Varchar2)).Value = address;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    int aff = cmd.ExecuteNonQuery();
                    MessageBox.Show(aff + " rows were affected.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return;
        }

        public void updatePhoneNumber(string number)
        {
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "UPDATE ADMIN_OLS.SINHVIEN " +
                        "SET DT = :Phone";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":Phone", OracleDbType.Varchar2)).Value = number;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    int aff = cmd.ExecuteNonQuery();
                    MessageBox.Show(aff + " rows were affected.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return;
        }
        //Lấy 1 dòng thông tin trong bảng đăng ký
        public DataRow GetOneRegisterRow(string studentID, string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_GV_SELECT_DANGKY WHERE MASV = :student_ID AND MAGV = :teacher_ID AND MAHP = :course_ID AND HK = :course_semester AND NAM = :course_year AND MACT = :curriculum_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":student_ID", studentID));
                    cmd.Parameters.Add(new OracleParameter(":teacher_ID", teacherID));
                    cmd.Parameters.Add(new OracleParameter(":course_ID", courseID));
                    cmd.Parameters.Add(new OracleParameter(":course_semester", semester));
                    cmd.Parameters.Add(new OracleParameter(":course_year", year));
                    cmd.Parameters.Add(new OracleParameter(":curriculum_ID", curriculumID));


                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Chỉnh sửa điểm của sinh viên trên quan hệ đăng ký 
        public void EditStudentScore(string studentID, string teacherID, string courseID, string semester, string year, string curriculum, string labScore, string processScore, string examEndScore, string finalScore)
        {
            bool success = false;

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPD_V_GV_SELECT_DANGKY", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MASV_U", OracleDbType.Varchar2)).Value = studentID;
                        cmd.Parameters.Add(new OracleParameter("MAGV_U", OracleDbType.Varchar2)).Value = teacherID;
                        cmd.Parameters.Add(new OracleParameter("MAHP_U", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("HK_U", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("NAM_U", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("MACT_U", OracleDbType.Varchar2)).Value = curriculum;
                        cmd.Parameters.Add(new OracleParameter("DIEMTH_U", OracleDbType.Varchar2)).Value = labScore;
                        cmd.Parameters.Add(new OracleParameter("DIEMQT_U", OracleDbType.Varchar2)).Value = processScore;
                        cmd.Parameters.Add(new OracleParameter("DIEMCK_U", OracleDbType.Varchar2)).Value = examEndScore;
                        cmd.Parameters.Add(new OracleParameter("DIEMTK_U", OracleDbType.Varchar2)).Value = finalScore;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Chỉnh sửa thành công");
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thất bại");
            }
        }

        //Lấy standard audit 
        public List<List<String>> getStandardAuditList(ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_ALL_STANDARD_AUDIT", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<String> temp = new List<String>();
                        temp.Add(dr.Field<String>(0));
                        temp.Add(dr.Field<String>(1));
                        temp.Add(dr.Field<String>(2));
                        temp.Add(dr.Field<String>(3));
                        temp.Add(dr.Field<String>(4));
                        if (dr.Field<String>(5).Equals("0"))
                        {
                            temp.Add("Thành công");
                        }
                        else
                        {
                            temp.Add("Thất bại");
                        }

                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        //Xem thông báo OLS 
        public List<String> getAllNotifications(ref Exception exOutput)
        {
            List<String> list = new List<String>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("SELECT NOIDUNG FROM ADMIN_OLS.THONGBAO");

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> temp = new List<String>();
                        list.Add(reader.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return list;
        }

        //Fined Grained Audit
        public List<List<String>> getFGAuditList(ref Exception exOutput)
        {
            if (isLogIn == false) return new List<List<string>>();
            List<List<String>> result = new List<List<string>>();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_FG_AUDIT", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        List<String> temp = new List<String>();
                        temp.Add(dr.Field<String>(0));
                        temp.Add(dr.Field<String>(1));
                        temp.Add(dr.Field<String>(2));
                        temp.Add(dr.Field<String>(3));
                        result.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }

        //GET FINE GRAINED AUDIT
        public DataTable GetStandardAudit()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_ALL_STANDARD_AUDIT", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //GET FINE GRAINED AUDIT
        public DataTable GetFGA()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("GET_FG_AUDIT", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Lấy thông tin của một sinh viên theo ID
        public DataRow GetStudentByID(string userName)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.SINHVIEN WHERE MASV = :user_name";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":user_name", userName));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Lấy thông tin một đơn vị theo ID
        public DataRow GetDepartmentByID(string departmentID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.DONVI WHERE MADV = :department_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":department_ID", departmentID));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }


        //Lấy thông tin một học phần theo ID
        public DataRow GetCourseByID(string courseID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.HOCPHAN WHERE MAHP = :course_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":course_ID", courseID));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Lấy thông tin một kế hoạch mở 
        public DataRow GetOpenCourseByID(string courseID, string semester, string year, string curriculumID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.KHMO WHERE MAHP = :course_ID AND HK = :course_semester AND NAM = :course_year AND MACT = :curriculum_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":course_ID", courseID));
                    cmd.Parameters.Add(new OracleParameter(":course_semester", semester));
                    cmd.Parameters.Add(new OracleParameter(":course_year", year));
                    cmd.Parameters.Add(new OracleParameter(":curriculum_ID", curriculumID));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Lấy thông tin một phân công
        public DataRow GetAssignment(string teacherID, string courseID, string semester, string year, string curriculumID)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.PHANCONG WHERE MAGV = :teacher_ID AND MAHP = :course_ID AND HK = :course_semester AND NAM = :course_year AND MACT = :curriculum_ID";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":teacher_ID", teacherID));
                    cmd.Parameters.Add(new OracleParameter(":course_ID", courseID));
                    cmd.Parameters.Add(new OracleParameter(":course_semester", semester));
                    cmd.Parameters.Add(new OracleParameter(":course_year", year));
                    cmd.Parameters.Add(new OracleParameter(":curriculum_ID", curriculumID));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        //Cập nhật thông tin sinh viên 
        public void EditStudentInfo(string oldStudentID, string studentID, string studentName, string gender, string birthday, string address, string phoneNumber, string curriculum, string major, string credits, string gpa)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_SINHVIEN", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MASV_OLD", OracleDbType.Varchar2)).Value = oldStudentID;
                        cmd.Parameters.Add(new OracleParameter("MASV_U", OracleDbType.Varchar2)).Value = studentID;
                        cmd.Parameters.Add(new OracleParameter("HOTEN_U", OracleDbType.Varchar2)).Value = studentName;
                        cmd.Parameters.Add(new OracleParameter("PHAI_U", OracleDbType.Varchar2)).Value = gender;
                        cmd.Parameters.Add(new OracleParameter("NGSNH_U", OracleDbType.Varchar2)).Value = birthday;
                        cmd.Parameters.Add(new OracleParameter("DCHI_U", OracleDbType.Varchar2)).Value = address;
                        cmd.Parameters.Add(new OracleParameter("DT_U", OracleDbType.Varchar2)).Value = phoneNumber;
                        cmd.Parameters.Add(new OracleParameter("MACT_U", OracleDbType.Varchar2)).Value = curriculum;
                        cmd.Parameters.Add(new OracleParameter("MANGANH_U", OracleDbType.Varchar2)).Value = major;
                        cmd.Parameters.Add(new OracleParameter("SOTCTL_U", OracleDbType.Varchar2)).Value = credits;
                        cmd.Parameters.Add(new OracleParameter("DTBTL_U", OracleDbType.Varchar2)).Value = gpa;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        //Cập nhật đơn vị
        public void EditDepartmentInfo(string oldDepartmentID, string departmentID, string departmentName, string departmentHead)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_DONVI", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MADV_OLD", OracleDbType.Varchar2)).Value = oldDepartmentID;
                        cmd.Parameters.Add(new OracleParameter("MADV_U", OracleDbType.Varchar2)).Value = departmentID;
                        cmd.Parameters.Add(new OracleParameter("TENDV_U", OracleDbType.Varchar2)).Value = departmentName;
                        cmd.Parameters.Add(new OracleParameter("TRGDV_U", OracleDbType.Varchar2)).Value = departmentHead;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }


        //Cập nhật học phần
        public void EditCourseInfo(string oldCourseID, string courseID, string courseName, string credits, string courseTheory, string courseLab, string courseMaxStudent, string courseDepartment)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_HOCPHAN", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MAHP_OLD", OracleDbType.Varchar2)).Value = oldCourseID;
                        cmd.Parameters.Add(new OracleParameter("MAHP_U", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("TENHP_U", OracleDbType.Varchar2)).Value = courseName;
                        cmd.Parameters.Add(new OracleParameter("SOTC_U", OracleDbType.Varchar2)).Value = credits;
                        cmd.Parameters.Add(new OracleParameter("STLT_U", OracleDbType.Varchar2)).Value = courseTheory;
                        cmd.Parameters.Add(new OracleParameter("STTH_U", OracleDbType.Varchar2)).Value = courseLab;
                        cmd.Parameters.Add(new OracleParameter("SOSVTD_U", OracleDbType.Varchar2)).Value = courseMaxStudent;
                        cmd.Parameters.Add(new OracleParameter("MADV_U", OracleDbType.Varchar2)).Value = courseDepartment;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        //Cập nhật kế hoạch mở 
        public void EditOpenCourseInfo(string oldCourseID, string oldSemester, string oldYear, string oldCurriculum, string courseID, string semester, string year, string curriculum)
        {
            bool success = false;

            
            int oldSemesterNum, oldYearNum, semesterNum, yearNum;
            if (!int.TryParse(oldSemester, out oldSemesterNum) ||
                !int.TryParse(oldYear, out oldYearNum) ||
                !int.TryParse(semester, out semesterNum) ||
                !int.TryParse(year, out yearNum))
            {
                MessageBox.Show("Học kỳ và năm phải là số");
            }

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_KHMO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MAHP_OLD", OracleDbType.Varchar2)).Value = oldCourseID;
                        cmd.Parameters.Add(new OracleParameter("HK_OLD", OracleDbType.Varchar2)).Value = oldSemester;
                        cmd.Parameters.Add(new OracleParameter("NAM_OLD", OracleDbType.Varchar2)).Value = oldYear;
                        cmd.Parameters.Add(new OracleParameter("MACT_OLD", OracleDbType.Varchar2)).Value = oldCurriculum;
                        cmd.Parameters.Add(new OracleParameter("MAHP_U", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("HK_U", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("NAM_U", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("MACT_U", OracleDbType.Varchar2)).Value = curriculum;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }


        //Cập nhật phân công
        public void EditAssignment(string oldTeacherID, string oldCourseID, string oldSemester, string oldYear, string oldCurriculum, string teacherID, string courseID, string semester, string year, string curriculum)
        {
            bool success = false;


            int oldSemesterNum, oldYearNum, semesterNum, yearNum;
            if (!int.TryParse(oldSemester, out oldSemesterNum) ||
                !int.TryParse(oldYear, out oldYearNum) ||
                !int.TryParse(semester, out semesterNum) ||
                !int.TryParse(year, out yearNum))
            {
                MessageBox.Show("Học kỳ và năm phải là số");
            }

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_PHANCONG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MAGV_OLD", OracleDbType.Varchar2)).Value = oldTeacherID;
                        cmd.Parameters.Add(new OracleParameter("MAHP_OLD", OracleDbType.Varchar2)).Value = oldCourseID;
                        cmd.Parameters.Add(new OracleParameter("HK_OLD", OracleDbType.Varchar2)).Value = oldSemester;
                        cmd.Parameters.Add(new OracleParameter("NAM_OLD", OracleDbType.Varchar2)).Value = oldYear;
                        cmd.Parameters.Add(new OracleParameter("MACT_OLD", OracleDbType.Varchar2)).Value = oldCurriculum;
                        cmd.Parameters.Add(new OracleParameter("MAGV_U", OracleDbType.Varchar2)).Value = teacherID;
                        cmd.Parameters.Add(new OracleParameter("MAHP_U", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("HK_U", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("NAM_U", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("MACT_U", OracleDbType.Varchar2)).Value = curriculum;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }


        //Thêm một sinh viên  mới
        public void AddStudent(string studentID, string studentName, string gender, string birthday, string address, string phoneNumber, string curriculum, string major, string credits, string gpa)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_SINHVIEN", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MASV", OracleDbType.Varchar2)).Value = studentID;
                        cmd.Parameters.Add(new OracleParameter("P_HOTEN", OracleDbType.Varchar2)).Value = studentName;
                        cmd.Parameters.Add(new OracleParameter("P_PHAI", OracleDbType.Varchar2)).Value = gender;
                        cmd.Parameters.Add(new OracleParameter("P_NGSINH", OracleDbType.Varchar2)).Value = birthday;
                        cmd.Parameters.Add(new OracleParameter("P_DCHI", OracleDbType.Varchar2)).Value = address;
                        cmd.Parameters.Add(new OracleParameter("P_DT", OracleDbType.Varchar2)).Value = phoneNumber;
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = curriculum;
                        cmd.Parameters.Add(new OracleParameter("P_MANGANH", OracleDbType.Varchar2)).Value = major;
                        cmd.Parameters.Add(new OracleParameter("P_SOTCTL", OracleDbType.Varchar2)).Value = credits;
                        cmd.Parameters.Add(new OracleParameter("P_DTBTL", OracleDbType.Varchar2)).Value = gpa;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }


        //Thêm một đơn vị
        public void AddDepartment(string departmentID, string departmentName, string departmentHead)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_DONVI", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MADV", OracleDbType.Varchar2)).Value = departmentID;
                        cmd.Parameters.Add(new OracleParameter("P_TENDV", OracleDbType.Varchar2)).Value = departmentName;
                        cmd.Parameters.Add(new OracleParameter("P_TRGDV", OracleDbType.Varchar2)).Value = departmentHead;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        //Thêm một học phần
        public void AddCourse(string courseID, string courseName, string credits, string courseTheory, string courseLab, string courseMaxStudent, string courseDepartment)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_HOCPHAN", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("P_TENHP", OracleDbType.Varchar2)).Value = courseName;
                        cmd.Parameters.Add(new OracleParameter("P_SOTC", OracleDbType.Varchar2)).Value = credits;
                        cmd.Parameters.Add(new OracleParameter("P_STLT", OracleDbType.Varchar2)).Value = courseTheory;
                        cmd.Parameters.Add(new OracleParameter("P_STTH", OracleDbType.Varchar2)).Value = courseLab;
                        cmd.Parameters.Add(new OracleParameter("P_SOSVTD", OracleDbType.Varchar2)).Value = courseMaxStudent;
                        cmd.Parameters.Add(new OracleParameter("P_MADV", OracleDbType.Varchar2)).Value = courseDepartment;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        //Thêm một kế hoạch mở
        public void AddOpenCourse(string courseID, string semester, string year, string curriculum)
        {
            bool success = false;


            int semesterNum, yearNum;
            if (!int.TryParse(semester, out semesterNum) ||
                !int.TryParse(year, out yearNum))
            {
                MessageBox.Show("Học kỳ và năm phải là số");
            }

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_KHMO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = curriculum;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        //Thêm đăng ký
        public void AddResgiter(string studentID, string teacherID, string courseID, string semester, string year, string curriculum, string labScore, string processScore, string examEndScore, string finalScore)
        {
            bool success = false;


            int semesterNum, yearNum, labScoreNum, processScoreNum, examEndScoreNum, finalScoreNum;
            if (!int.TryParse(semester, out semesterNum) ||
                !int.TryParse(year, out yearNum) ||
                !int.TryParse(labScore, out labScoreNum) ||
                !int.TryParse(processScore, out processScoreNum) ||
                !int.TryParse(examEndScore, out examEndScoreNum) ||
                !int.TryParse(finalScore, out finalScoreNum ))
            {
                MessageBox.Show("Học kỳ và năm và điểm phải là số");
            }

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_DANGKY", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MASV", OracleDbType.Varchar2)).Value = studentID;
                        cmd.Parameters.Add(new OracleParameter("P_MAGV", OracleDbType.Varchar2)).Value = teacherID;
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = curriculum;
                        cmd.Parameters.Add(new OracleParameter("P_DIEMTH", OracleDbType.Varchar2)).Value = labScore;
                        cmd.Parameters.Add(new OracleParameter("P_DIEMQT", OracleDbType.Varchar2)).Value = processScore;
                        cmd.Parameters.Add(new OracleParameter("P_DIEMCK", OracleDbType.Varchar2)).Value = examEndScore;
                        cmd.Parameters.Add(new OracleParameter("P_DIEMTK", OracleDbType.Varchar2)).Value = finalScore;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        //Xóa đăng ký 
        public void DeleteResgiter(string studentID, string teacherID, string courseID, string semester, string year, string curriculum)
        {
            bool success = false;


            int semesterNum, yearNum;
            if (!int.TryParse(semester, out semesterNum) ||
                !int.TryParse(year, out yearNum))
            {
                MessageBox.Show("Học kỳ và năm phải là số");
            }

            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.DELETE_DANGKY", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MASV", OracleDbType.Varchar2)).Value = studentID;
                        cmd.Parameters.Add(new OracleParameter("P_MAGV", OracleDbType.Varchar2)).Value = teacherID;
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = courseID;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Varchar2)).Value = semester;
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Varchar2)).Value = year;
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = curriculum;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }


            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Xóa thành công");
            }
            else
            {
                MessageBox.Show("Xóa không thành công");
            }
        }

        //Xoa mot dong Phan Cong
        public void deleteARowPhanCong(string MAGV, string MAHP, string HK, string NAM, string MACT, ref Exception exOut)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.DELETE_A_PHANCONG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MAGV", OracleDbType.Varchar2)).Value = MAGV;
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = MAHP;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Int16)).Value = Int16.Parse(HK);
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Int64)).Value = Int16.Parse(NAM);
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = MACT;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOut = ex;
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Xóa thành công");
            }
            else
            {
                MessageBox.Show("Xóa không thành công");
            }
        }

        //Cập nhật 1 dòng Phân công
        public void updateARowPhanCong(string MAGV, string MAHP, string HK, string NAM, string MACT, string MAGVNEW, ref Exception exOut)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_A_PHANCONG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MAGV", OracleDbType.Varchar2)).Value = MAGV;
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = MAHP;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Int16)).Value = Int16.Parse(HK);
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Int64)).Value = Int16.Parse(NAM);
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = MACT;
                        cmd.Parameters.Add(new OracleParameter("P_MAGVNEW", OracleDbType.Varchar2)).Value = MAGVNEW;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOut = ex;
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }


        //Cập nhật số điện thoại
        public void EditPhoneNumber(string username, string newPhoneNumber)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_PHONE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("MANV_OLD", OracleDbType.Varchar2)).Value = username;
                        cmd.Parameters.Add(new OracleParameter("DT_U", OracleDbType.Varchar2)).Value = newPhoneNumber;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        //Lấy phân công theo quyền của trưởng đơn vị 
        public DataTable GetTDVAssignment()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {


                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TDV_PHANCONG_EXTRA";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        //Lấy phân công theo quyền của trưởng khoa 
        public DataTable GetTKAssignment()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.PHANCONG";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable getTDVPhanCongForEdit()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TDV_PHANCONG";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable getTDVAllKHMO()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TDV_KHMO";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable getTKAllKHMO()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TK_KHMO";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable getAllNhanSu()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TKTDV_NHANSU";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }


        public void addARowPhanCong(string MAGV, string MAHP, string HK, string NAM, string MACT, ref Exception exOut)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.INSERT_A_PHANCONG", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MAGV", OracleDbType.Varchar2)).Value = MAGV;
                        cmd.Parameters.Add(new OracleParameter("P_MAHP", OracleDbType.Varchar2)).Value = MAHP;
                        cmd.Parameters.Add(new OracleParameter("P_HK", OracleDbType.Varchar2)).Value = HK;
                        cmd.Parameters.Add(new OracleParameter("P_NAM", OracleDbType.Varchar2)).Value = NAM;
                        cmd.Parameters.Add(new OracleParameter("P_MACT", OracleDbType.Varchar2)).Value = MACT;



                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);

                exOut = ex;

            }

            //Hiển thị thông báo tương ứng
            if (success)
            {

                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm không thành công");
            }
        }

        public DataTable getTKPhanCongForEdit()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.V_TK_PHANCONG";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public DataTable getAllNhanSuForEdit()
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.NHANSU";
                    OracleCommand cmd = new OracleCommand(query, con);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        public void deleteARowNhanSu(string MANV)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.DELETE_A_NHANSU", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = MANV;

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Xóa thành công");
                success = false;
                try
                {
                    // Tạo kết nối
                    using (new OpenedContext(con))
                    {
                        // Tạo đối tượng command
                        using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.DROP_A_USER", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm các tham số vào command
                            cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = MANV;

                            // Thực thi command
                            cmd.ExecuteNonQuery();

                            // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Xóa không thành công");
            }
        }

        public string getRoleByID(string id, ref Exception exOutput)
        {
            string roleID = null;
            List<string> result = new List<string>();
            using (new OpenedContext(con))
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.Get_A_Role_Of_User", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào command
                        cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = id;

                        // Thực thi command
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            result.Add(reader.GetString(0));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] == "NHANVIENCOBAN" || result[i] == "GIANGVIEN" ||
                    result[i] == "GIAOVU" || result[i] == "TRUONGDONVI" || result[i] == "TRUONGKHOA")
                {
                    roleID = result[i];
                }
            }
            return roleID;
        }

        public DataRow GetEmployeeByIDForEdit(string userName)
        {
            DataTable dt = new DataTable();
            using (new OpenedContext(con))
            {
                try
                {
                    string query = "SELECT * FROM ADMIN_OLS.NHANSU WHERE MANV = :user_name";
                    OracleCommand cmd = new OracleCommand(query);

                    cmd.Parameters.Add(new OracleParameter(":user_name", userName));

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void updateARowNhanSu(string MAGV, string HOTEN, string PHAI, string NGSINH, string PHUCAP, string DT, string VAITRO, string DONVI, ref Exception exOut)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.UPDATE_A_NHANSU", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = MAGV;
                        cmd.Parameters.Add(new OracleParameter("P_HOTEN", OracleDbType.Varchar2)).Value = HOTEN;
                        cmd.Parameters.Add(new OracleParameter("P_PHAI", OracleDbType.Varchar2)).Value = PHAI;
                        cmd.Parameters.Add(new OracleParameter("P_NGSINH", OracleDbType.Varchar2)).Value = NGSINH;
                        cmd.Parameters.Add(new OracleParameter("P_PHUCAP", OracleDbType.Varchar2)).Value = PHUCAP;
                        cmd.Parameters.Add(new OracleParameter("P_DT", OracleDbType.Varchar2)).Value = DT;
                        cmd.Parameters.Add(new OracleParameter("P_VAITRO", OracleDbType.Varchar2)).Value = VAITRO;
                        cmd.Parameters.Add(new OracleParameter("P_MADV", OracleDbType.Varchar2)).Value = DONVI;
                        // Thêm các tham số vào command

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOut = ex;
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                MessageBox.Show("Cập nhật thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        public string getIDHighest()
        {
            string iD = "NV0000";

            using (new OpenedContext(con))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand("SELECT MANV FROM ADMIN_OLS.NHANSU WHERE ROWNUM = 1 ORDER BY MANV DESC", con);

                    cmd.CommandType = CommandType.Text;
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    foreach (DataRow dr in dt.Rows)
                    {
                        iD = dr.Field<String>(0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return iD;
        }

        public void addARowNhanSu(string MAGV, string HOTEN, string PHAI, string NGSINH, string PHUCAP, string DT, string VAITRO, string DONVI, ref Exception exOut)
        {
            bool success = false;
            try
            {
                // Tạo kết nối
                using (new OpenedContext(con))
                {
                    // Tạo đối tượng command
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.ADD_ROW_NHANSU", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = MAGV;
                        cmd.Parameters.Add(new OracleParameter("P_HOTEN", OracleDbType.Varchar2)).Value = HOTEN;
                        cmd.Parameters.Add(new OracleParameter("P_PHAI", OracleDbType.Varchar2)).Value = PHAI;
                        cmd.Parameters.Add(new OracleParameter("P_NGSINH", OracleDbType.Varchar2)).Value = NGSINH;
                        cmd.Parameters.Add(new OracleParameter("P_PHUCAP", OracleDbType.Varchar2)).Value = PHUCAP;
                        cmd.Parameters.Add(new OracleParameter("P_DT", OracleDbType.Varchar2)).Value = DT;
                        cmd.Parameters.Add(new OracleParameter("P_VAITRO", OracleDbType.Varchar2)).Value = VAITRO;
                        cmd.Parameters.Add(new OracleParameter("P_MADV", OracleDbType.Varchar2)).Value = DONVI;
                        // Thêm các tham số vào command

                        // Thực thi command
                        cmd.ExecuteNonQuery();

                        // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                exOut = ex;
            }

            //Hiển thị thông báo tương ứng
            if (success)
            {
                success = false;
                MessageBox.Show("Cập nhật thành công");
                try
                {
                    // Tạo kết nối
                    using (new OpenedContext(con))
                    {
                        // Tạo đối tượng command
                        using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.CREATE_A_USER", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new OracleParameter("P_MANV", OracleDbType.Varchar2)).Value = MAGV;
                            // Thêm các tham số vào command

                            // Thực thi command
                            cmd.ExecuteNonQuery();

                            // Nếu đến được đến đây mà không có lỗi, đánh dấu thành công
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOut = ex;
                }
            }
            else
            {
                MessageBox.Show("Cập nhật không thành công");
            }
        }

        public List<string> getAllDonVi(ref Exception exOutput)
        {
            List<string> result = new List<string>();
            using (new OpenedContext(con))
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand("ADMIN_OLS.GET_ALL_DONVI", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thực thi command
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            result.Add(reader.GetString(0));
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    exOutput = ex;
                }
            }
            return result;
        }


        public void logOut()
        {
             con = null;
             userName = null;
             isLogIn = false;
             roleName = new List<String>();   
            
        }
        ~Database()
        {
            con = null;
            if(_instance != null)
            {
                _instance = null;
            }
        }

    }
}
