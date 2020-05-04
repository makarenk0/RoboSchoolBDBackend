using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Lastname { get; set; }
        public String Password_temp { get; set; }
        public String Hash { get; set; }
        public String Salt { get; set; }
    }
}
