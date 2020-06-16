using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Course
{
    public class CourseOut
    {
        public CourseOut(Courses courses)
        {
            name_course = courses.name_course;
            items = new List<CourseItems>();
            foreach(Course_items course_items in courses.items)
            {
                items.Add(new CourseItems(course_items.id_item, course_items.Item.cost, course_items.Item.prov_name, course_items.Item.name));
            }
        }
        public String name_course { get; set; }
        public List<CourseItems> items { get; set; }


        public class CourseItems
        {
            public CourseItems(int id_item, double cost, String prov_name, String name)
            {
                this.id_item = id_item;
                this.cost = cost;
                this.prov_name = prov_name;
                this.name = name;
            }
            public int id_item { get; set; }
            public double cost { get; set; }
            public String prov_name { get; set; }
            public String name { get; set; }
        }

    }

    
}
