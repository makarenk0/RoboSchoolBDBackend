## RoboSchoolBDProject <br>
Request to mysql database example:<br>
 ```
 using (DbDataReader reader = DBController.Execute("SELECT * FROM TestTable")) //"using" because async working with unmanaged resources
 {
     while (reader.Read())   //reading all rows
     {
         String str = reader.GetString(0); // 0 is number of column from resulting table
     }
 }
 ```
