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

        public void InsertUserInfo(User user)
        {
            string SQL = "INSERT INTO UserTable (LoginID, LoginPassword, UserName) VALUES ('" + user.LoginID + ", '" + user.LoginPassword + "', '" + user.UserName + "') ";

            using (MySqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    var cmd = new MySqlCommand(SQL, con);

                    if(cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert Suc");
                    }
                    else
                    {
                        Console.WriteLine("Insert Fail");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("DB Connection Fail");
                    Console.WriteLine(e.ToString());
                }
                con.Close();
            }

        }
    }
}
