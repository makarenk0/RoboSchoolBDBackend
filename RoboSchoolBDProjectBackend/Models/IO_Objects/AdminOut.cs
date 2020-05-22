using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models
{
    public class AdminOut
    {
        [Key]
        public int id_admin { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
    }
}
