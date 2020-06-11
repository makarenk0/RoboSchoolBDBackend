using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.Teacher
{
    public class TeacherIn
    {
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public String Password_temp { get; set; }
        public String hash { get; set; }
        public String salt { get; set; }

        public List<PhonesIn> phones { get; set; }
    }

    public class PhonesIn
    {
        public String phone_number;
        public int amount;
    }
}
