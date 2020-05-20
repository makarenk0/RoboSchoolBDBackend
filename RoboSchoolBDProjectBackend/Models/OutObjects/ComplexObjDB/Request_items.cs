using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.ComplexObjDB
{
    public class Request_items
    {
      
        [Key]

        public int id_request { get; set; }
        public DateTime date { get; set; }
        public bool confirmed { get; set; }
        public DateTime? date_confirmed { get; set; }
        public bool finished { get; set; }
        public DateTime? date_finished { get ; set ; }

        public int id_teacher { get; set; }
        public int id_manager { get; set; }

        public int id_item { get; set; }
        public String name { get; set; }
        public int items_num { get; set; }

    }
}
