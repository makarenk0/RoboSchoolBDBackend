using System;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Manager_phones
    {
        [Key]
        public String phone_number { get; set; }
        public int id_manager { get; set; }

        public Managers manager { get; set; }
    }
}
