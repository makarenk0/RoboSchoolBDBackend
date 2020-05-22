using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.School
{
    public class SchoolOut
    {
        public SchoolOut(Schools schools)
        {
            Id = schools.id_school;
            Adress = schools.adress;
            Open_date = schools.open_date;
            Classrooms_number = schools.aud_number;
            Manager_id = schools.id_manager;
            Teacher_id = schools.id_teacher;

            items = new List<SchoolItems>();

            foreach (School_items school_items in schools.items)
            {
                items.Add(new SchoolItems(school_items.id_item, school_items.Item.cost, school_items.Item.prov_name, school_items.Item.name));
            }
        }
        public int Id { get; set; }
        public String Adress { get; set; }
        public DateTime Open_date { get; set; }
        public int Classrooms_number { get; set; }
        public int Manager_id { get; set; }
        public int Teacher_id { get; set; }
        public List<SchoolItems> items { get; set; }


        public class SchoolItems
        {
            public SchoolItems(int id_item, int cost, String prov_name, String name)
            {
                this.id_item = id_item;
                this.cost = cost;
                this.prov_name = prov_name;
                this.name = name;
            }
            public int id_item { get; set; }
            public int cost { get; set; }
            public String prov_name { get; set; }
            public String name { get; set; }
        }
    }
}
