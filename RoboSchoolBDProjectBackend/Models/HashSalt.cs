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
        public String hash { get; set; }
        public String salt { get; set; }
    }
}
