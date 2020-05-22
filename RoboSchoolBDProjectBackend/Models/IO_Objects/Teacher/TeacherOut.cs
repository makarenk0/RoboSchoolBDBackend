using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Teacher
{
    public class TeacherOut
    {
        public TeacherOut(Teachers teacher)
        {
            Id = teacher.id_teacher;
            Name = teacher.name;
            Surname = teacher.surname;
            Lastname = teacher.lastname;
            Email = teacher.email;
        }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Lastname { get; set; }
        public String Email { get; set; }
    }
}
