
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Items
    {
        public Items()
        {
            Course_items = new HashSet<Course_items>();
            School_items = new HashSet<School_items>();
            Request_items = new HashSet<Request_items>();
        }

        [Key]
        public int id_item { get; set; }
        public double cost { get; set; }
        public String prov_name { get; set; }
        public String name { get; set; }

        public virtual ICollection<Course_items> Course_items { get; set; }
        public virtual ICollection<School_items> School_items { get; set; }
        public virtual ICollection<Request_items> Request_items { get; set; }

    }
}
