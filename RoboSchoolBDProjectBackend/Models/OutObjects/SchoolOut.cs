using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.Admin
{
    public class SchoolOut
    {
        [Key]
        public int id_school { get; set; }
        public String adress { get; set; }
        public DateTime open_date { get; set; }
        public int aud_number { get; set; }
        public int id_teacher { get; set; }
        public int id_manager { get; set; }
    }
}
