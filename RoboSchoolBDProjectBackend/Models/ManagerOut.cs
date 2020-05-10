using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models
{
    public class ManagerOut
    {
        [Key]
        public int Manager_id { get; set; }
        public String Manager_name { get; set; }
        public String Manager_surname { get; set; }
        public String Manager_lastname { get; set; }
        public String Email { get; set; }
    }
}
