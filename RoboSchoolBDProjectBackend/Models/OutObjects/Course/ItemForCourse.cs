using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Course
{
    public class ItemForCourse
    {
        public ItemForCourse(int Id_item, String Name)
        {
            id_item = Id_item;
            name = Name;
        }
        public int id_item { get; set; }
        public String name { get; set; }
    }
}
