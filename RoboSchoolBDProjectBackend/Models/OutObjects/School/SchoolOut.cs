using Microsoft.EntityFrameworkCore;
using RoboSchoolBDProjectBackend.Models.OutObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.Admin
{
    public class SchoolOut
    {
        
        public SchoolOut(School_items school_items)
        {
            id_school = school_items.id_school;
            adress = school_items.adress;
            open_date = school_items.open_date;
            aud_number = school_items.aud_number;
            id_teacher = school_items.id_teacher;
            id_manager = school_items.id_manager;
            items = new List<ItemForSchool>();
            items.Add(new ItemForSchool(school_items.id_item, school_items.name, school_items.items_num));
        }

       
        public int id_school { get; set; }
        public String adress { get; set; }
        public DateTime open_date { get; set; }
        public int aud_number { get; set; }
        public int id_teacher { get; set; }
        public int id_manager { get; set; }
        
       
        public List<ItemForSchool> items { get; set; }
}

    
}
