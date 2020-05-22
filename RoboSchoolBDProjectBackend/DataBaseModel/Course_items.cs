using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Course_items
    {
        [Key]
        public int id_course_items { get; set; }
        public String name_course { get; set; }
        public int id_item { get; set; }

        //Foreign objects
        public Courses Course { get; set; }
        public Items Item { get; set; }
    }
}
