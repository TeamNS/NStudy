using DotNetCoreService.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService
{
    public class DBConnection
    {
        public string ConnectionString { get; set; }

        public DBConnection(String connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<User> GetUserDataAll()
        {
            var UserList = new List<User>();

            string SQL = "SELECT * FROM UserInfo ";

            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                var cmd = new MySqlCommand(SQL, con);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserList.Add(new User()
                        {
                            UniqueID = Convert.ToInt64(reader["UniqueID"]),
                            LoginID = reader["LoginID"].ToString(),
                            LoginPassword = reader["LoginPassword"].ToString(),
                            UserName = reader["UserName"].ToString()
                        });
                    }
                }

                con.Close();
            }

            return UserList;
        }

        public Int32 RegistUserInfo(User user)
        {
            Int32 retVal = 0;
            using (MySqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    var cmd = new MySqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = "spUserRegistration";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("P_LoginID", user.LoginID);
                    cmd.Parameters["P_LoginID"].Direction = System.Data.ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("P_LoginPassword", user.LoginPassword);
                    cmd.Parameters["P_LoginPassword"].Direction = System.Data.ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("P_UserName", user.UserName);
                    cmd.Parameters["P_UserName"].Direction = System.Data.ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("RetVal", retVal);
                    cmd.Parameters["RetVal"].Direction = System.Data.ParameterDirection.Output;

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Proc Suc");
                    }
                    else
                    {
                        Console.WriteLine("Proc Fail");
                        Console.WriteLine("retVal: " + cmd.Parameters["RetVal"].Value);
                        retVal = Convert.ToInt32(cmd.Parameters["RetVal"].Value);
                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    Console.WriteLine("DB Connection Fail");
                    Console.WriteLine(e.ToString());
                }
                con.Close();
            }

            return retVal;
        }
    }
}
