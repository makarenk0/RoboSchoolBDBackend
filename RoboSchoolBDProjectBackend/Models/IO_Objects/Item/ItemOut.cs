using RoboSchoolBDProjectBackend.DataBaseModel;
using System;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Item
{
    public class ItemOut
    {
        public ItemOut(Items items)
        {
            Id = items.id_item;
            Name = items.name;
            Provider_name = items.prov_name;
            Cost = items.cost;
        }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Provider_name { get; set; }
        public int Cost { get; set; }
    }
}
