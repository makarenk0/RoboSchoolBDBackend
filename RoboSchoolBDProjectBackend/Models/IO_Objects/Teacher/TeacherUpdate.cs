using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Teacher
{
    public class TeacherUpdate
    {
        public int id_teacher { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }

        public List<PhonesIn> phones { get; set; }
    }

    public class PhonesIn
    {
        public String phone;
    }
}
