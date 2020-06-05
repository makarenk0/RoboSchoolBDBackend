using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Groups
    {
        [Key]
        public int id_group { get; set; }
        public int pupils_num { get; set; }
        public String name_course { get; set; }
        public int id_school { get; set; }
    }
}
