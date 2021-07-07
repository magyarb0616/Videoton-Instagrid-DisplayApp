using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DisplayApp;

namespace DisplayApp
{
    public class Database
    {
        //private MySqlConnection connection;
        //private string server;
        //private string database;
        //private string user;
        //private string password;
        //private string port;
        //private string connectionString;
        //public Database()
        //{
        //    server = "localhost";
        //    database = "instagrid";
        //    user = "magyarb0616";
        //    password = "jelszo";
        //    port = "3306";

        //    connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
        //    connection = new MySqlConnection(connectionString);
        //}


        ////Open connection
        //private void OpenConnection()
        //{
        //    try
        //    {
        //        connection.Open();
        //    }
        //    catch (MySqlException ex)
        //    {
        //        LogException(ex);
        //    }
        //}

        ////Close connection
        //private void CloseConnection()
        //{
        //    try
        //    {
        //        connection.Close();
        //    }
        //    catch (MySqlException ex)
        //    {
        //        LogException(ex);
        //    }
        //}

        //public string dbDate()
        //{
        //    try
        //    {
        //        OpenConnection();
        //        string date = "";
        //        string query = "SELECT NOW();";
        //        var cmd = new MySqlCommand(query, connection);
        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            date = reader.GetString(0);
        //            Console.WriteLine(date);
        //        }

        //        return date;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException(ex);
        //        return null;
        //    }

        //}

        //Exception Logging
        public static void LogException(Exception e)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Error.log", true))
            {
                file.WriteLine("******\r\n Source: " + e.Message + "\r\n");
            }

        }



    }
}
