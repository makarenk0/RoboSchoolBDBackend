using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.School
{
    public class SchoolOut
    {
        public SchoolOut(Schools schools, String manager, String teacher)
        {
            Id = schools.id_school;
            Adress = schools.adress;
            Open_date = schools.open_date.ToShortDateString();
            Classrooms_number = schools.aud_number;
            
            Manager = manager;
            Teacher = teacher;

            items = new List<SchoolItems>();

            foreach (School_items school_items in schools.items)
            {
                items.Add(new SchoolItems(school_items.id_school_items, school_items.id_item, school_items.Item.cost, school_items.items_num, school_items.Item.name));
            }
        }
        public int Id { get; set; }
        public String Adress { get; set; }
        public String Open_date { get; set; }
        public int Classrooms_number { get; set; }
        
        public String Manager { get; set; }
        public String Teacher { get; set; }

        public List<SchoolItems> items { get; set; }


        public class SchoolItems
        {
            public SchoolItems(int code, int id_item, double cost, int amount, String name)
            {
                this.code = code;
                this.id_item = id_item;
                this.cost = cost;
                this.amount = amount;
                this.name = name;
            }
            public int code { get; set; }
            public int id_item { get; set; }
            public double cost { get; set; }
            public int amount { get; set; }
            public String name { get; set; }
        }
    }
}
