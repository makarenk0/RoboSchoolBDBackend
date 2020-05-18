using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class ItemForSchool
    {
        public ItemForSchool(int Id_item, String Name, int Items_num)
        {
            id_item = Id_item;
            name = Name;
            items_num = Items_num;
        }
        public int id_item { get; set; }
        public String name { get; set; }
        public int items_num { get; set; }
    }
}
