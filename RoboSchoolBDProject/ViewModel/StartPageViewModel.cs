using RoboSchoolBDProject.Tools.MVVM;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data;
using RoboSchoolBDProject.Tools.DataBase;

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
            // TESTING (GET DATA FROM TABLE)

            using (DbDataReader reader = DBController.Execute("SELECT * FROM TestTable"))
            {
                 while (reader.Read())
                 {
                        
                        String str = reader.GetString(0);
                        TestString += str;
                        TestString += reader.GetDataTypeName(0).ToString();
                 }
            }
        }
    }
}
