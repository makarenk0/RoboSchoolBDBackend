using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class School_items
    {
        [Key]
        public int id_school { get; set; }
        public String adress { get; set; }
        public DateTime open_date { get; set; }
        public int aud_number { get; set; }
        public int id_teacher { get; set; }
        public int id_manager { get; set; }

        public int id_item { get; set; }
        public String name { get; set; }
        public int items_num { get; set; }

    }
}
