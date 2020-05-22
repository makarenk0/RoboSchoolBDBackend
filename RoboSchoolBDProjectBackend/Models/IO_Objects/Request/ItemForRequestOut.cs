using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Request
{
    public class ItemForRequestOut
    {

        public ItemForRequestOut(int Id_item, String Name, int amount)
        {
            id_item = Id_item;
            name = Name;
        }
        public int id_item { get; set; }
        public String name { get; set; }
        public int amount { get; set; }
    }
}
