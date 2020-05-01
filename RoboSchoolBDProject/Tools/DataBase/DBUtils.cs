using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace RoboSchoolBDProject
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "remotemysql.com";
            int port = 3306;
            string database = "WiYjJC1dWh";
            string username = "WiYjJC1dWh";
            string password = "XQ8zI8Fe6x";

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
