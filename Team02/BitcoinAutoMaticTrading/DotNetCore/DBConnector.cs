using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace DotNetCore
{
    public class DBConnector
    {
        public bool AccountLogin(string ID, string PW, string AccessKey, string SecretKey)
        {
            bool bRet = false;
            string OutAccessKey = "";
            string OutSecretKey = "";

            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=dotnetcore;port=3306;password=DataBase;"))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("spAccountLogin", con))
                {
                    cmd.Parameters.AddWithValue("_ID", ID);
                    cmd.Parameters.AddWithValue("_PW", PW);
                    cmd.Parameters.AddWithValue("_InAccessKey", AccessKey);
                    cmd.Parameters.AddWithValue("_InSecretKey", SecretKey);

                    cmd.Parameters.Add(new MySqlParameter("_OutAccessKey", MySqlDbType.VarChar));
                    cmd.Parameters["_OutAccessKey"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new MySqlParameter("_OutSecretKey", MySqlDbType.VarChar));
                    cmd.Parameters["_OutSecretKey"].Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        OutAccessKey = (string)cmd.Parameters["_OutAccessKey"].Value;
                        OutSecretKey = (string)cmd.Parameters["_OutSecretKey"].Value;

                        UpbitAPI.AddAccount(OutAccessKey, OutSecretKey);

                        bRet = true;
                    }
                    else
                    {
                        bRet = false;
                    }

                }
                con.Close();
                con.Dispose();
            }

            return bRet;
        }

        public bool AccountSignUp(string ID, string PW, string AccessKey, string SecretKey)
        {
            bool bRet = false;

            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=dotnetcore;port=3306;password=DataBase;"))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("spAccountSignUp", con))
                {
                    cmd.Parameters.AddWithValue("_ID", ID);
                    cmd.Parameters.AddWithValue("_PW", PW);
                    cmd.Parameters.AddWithValue("_AccessKey", AccessKey);
                    cmd.Parameters.AddWithValue("_SecretKey", SecretKey);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        bRet = true;
                    }
                    else
                    {
                        bRet = false;
                    }
                }

                con.Close();
                con.Dispose();
            }

            // AccessKey, SecretKey가 유효한 값인지는 어떻게 해야할지..?

            return bRet;
        }
    }
}
