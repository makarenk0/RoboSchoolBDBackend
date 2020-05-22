using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class School_items
    {
        [Key]
        public int id_school_items { get; set; }
        public int items_num { get; set; }
        public DateTime buying_date { get; set; }
        public int id_school { get; set; }
        public int id_item { get; set; }

        //Foreign objects
        public Schools School { get; set; }
        public Items Item { get; set; }


    }
}
