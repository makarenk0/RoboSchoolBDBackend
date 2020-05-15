using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class TeacherOut
    {
        [Key]
        public int id_teacher { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public DateTime work_begin { get; set; }
        public int work_exp { get; set; }
    }
}
