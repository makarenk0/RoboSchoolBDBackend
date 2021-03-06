﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Teachers
    {
        [Key]
        public int id_teacher { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public DateTime work_begin { get; set; }
        public int work_exp { get; set; }
        public String hash { get; set; }
        public String salt { get; set; }

        public virtual ICollection<Teacher_phones> phones { get; set; }
    }
}
