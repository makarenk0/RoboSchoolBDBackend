using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Course
{
    public class CourseIn
    {
        public List<ItemForCourseIn> id_item { get; set; }  //items
        public String name_course { get; set; }
    }
}
