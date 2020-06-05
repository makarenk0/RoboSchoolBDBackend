using RoboSchoolBDProjectBackend.DataBaseModel;
using System;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Item
{
    public class ItemOut
    {
        public ItemOut(Items items)
        {
            Id_item = items.id_item;
            Name = items.name;
            Provider_name = items.prov_name;
            Cost = items.cost;
        }
        public int Id_item { get; set; }
        public String Name { get; set; }
        public String Provider_name { get; set; }
        public int Cost { get; set; }
    }
}
