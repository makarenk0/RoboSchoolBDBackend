using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Managers
    {
        [Key]
        public int id_manager { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String lastname { get; set; }
        public String email { get; set; }
        public String hash { get; set; }
        public String salt { get; set; }

        public virtual ICollection<Manager_phones> phones { get; set; }
    }
}
