using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Courses
    {
        public Courses()
        {
            items = new HashSet<Course_items>();
        }
        [Key]
        public String name_course { get; set; }
        public virtual ICollection<Course_items> items { get; set; }
    }
}
