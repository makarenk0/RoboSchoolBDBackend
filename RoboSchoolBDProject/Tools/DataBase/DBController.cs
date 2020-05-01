using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace RoboSchoolBDProject.Tools.DataBase
{
    static class DBController
    {
        private static MySqlConnection conn;

        static DBController()
        {
            conn = DBUtils.GetDBConnection();

            try
            {
                Console.WriteLine("Openning Connection ...\n");
                conn.Open();

                Console.WriteLine("Connection successful!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message + "\n");
            }
        }

        public static DbDataReader Execute(String request)
        {
            MySqlCommand cmd = conn.CreateCommand();

            cmd.Connection = conn;
            cmd.CommandText = request;

            return cmd.ExecuteReader();
        }

    }
}
