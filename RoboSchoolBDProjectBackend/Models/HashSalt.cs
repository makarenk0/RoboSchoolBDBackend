using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models
{
    public class HashSalt
    {
        [Key]
        public String Manager_hash { get; set; }
        public String Manager_salt { get; set; }
    }
}
