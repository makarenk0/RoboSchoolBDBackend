using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Group
{
    public class GroupIn
    {
        public int pupils_number { get; set; }
        public String name_course { get; set; }
        public int? id_school { get; set; }
    }
}
