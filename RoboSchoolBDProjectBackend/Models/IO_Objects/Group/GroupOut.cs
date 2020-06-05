using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Group
{
    public class GroupOut
    {
        public GroupOut(Groups groups)
        {
            Id = groups.id_group;
            pupils_number = groups.pupils_num;
            name_course = groups.name_course;
            id_school = groups.id_school;
        }
        public int Id { get; set; }
        public int pupils_number { get; set; }
        public String name_course { get; set; }
        public int id_school { get; set; }
    }
}
