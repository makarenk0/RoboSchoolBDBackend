using RoboSchoolBDProject.Tools.MVVM;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace RoboSchoolBDProject.ViewModel
{
    class StartPageViewModel : BaseViewModel
    {
        private String _testString = "";

        public String TestString
        {
            get { return _testString; }
            set
            {
                _testString = value;
                OnPropertyChanged();
            }
        }

        public StartPageViewModel()
        {
            TestString += "Getting Connection ...\n";
            MySqlConnection conn = DBUtils.GetDBConnection();

            try
            {
                TestString += "Openning Connection ...\n";

                conn.Open();

                TestString += "Connection successful!\n";
            }
            catch (Exception e)
            {
                TestString += ("Error: " + e.Message + "\n");
            }
            // TESTING (GET DATA FROM TABLE)
            string sql = "SELECT * FROM TestTable";
            MySqlCommand cmd = conn.CreateCommand();

            cmd.Connection = conn;
            cmd.CommandText = sql;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        
                        String str = reader.GetString(0);
                        TestString += str;
                    }
                }
            }
        }
    }
}
