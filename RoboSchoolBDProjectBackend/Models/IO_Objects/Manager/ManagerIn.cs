using System;

namespace RoboSchoolBDProjectBackend.Models
{
    public class ManagerIn
    {  
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public String Password_temp { get; set; }
        public String hash { get; set; }
        public String salt { get; set; }
    }
}
