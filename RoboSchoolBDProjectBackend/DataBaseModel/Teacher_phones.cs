using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Teacher_phones
    {
        [Key]
        public String phone_number { get; set; }
        public int id_teacher { get; set; }

        public Teachers teacher { get; set; }
    }
}
