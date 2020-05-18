using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Course
{
    public class CourseOut
    {
        public CourseOut(Course_items course_items)
        {
            name_course = course_items.name_course;
    
            items = new List<ItemForCourse>();
            items.Add(new ItemForCourse(course_items.id_item, course_items.name));
        }

        public String name_course { get; set; }
        public List<ItemForCourse> items { get; set; }
    }
}
