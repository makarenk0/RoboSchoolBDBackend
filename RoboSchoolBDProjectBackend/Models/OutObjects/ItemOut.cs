using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class ItemOut
    {
        [Key]
        public int id_item { get; set; }
        public int cost { get; set; }
        public String prov_name { get; set; }
        public String name { get; set; }
    }
}
