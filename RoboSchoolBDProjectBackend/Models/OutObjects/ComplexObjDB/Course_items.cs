using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class Course_items
    {
        [Key]
       
        public String name_course { get; set; }

        public int id_item { get; set; }
        public String name { get; set; }
    }
}
