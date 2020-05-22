using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models.OutObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Schools
    {
        public Schools()
        {
            items = new HashSet<School_items>();
        }

       [Key]
        public int id_school { get; set; }
        public String adress { get; set; }
        public DateTime open_date { get; set; }
        public int aud_number { get; set; }

        public int id_teacher { get; set; }
        public int id_manager { get; set; }


        public virtual ICollection<School_items> items { get; set; }
    }

    
}
