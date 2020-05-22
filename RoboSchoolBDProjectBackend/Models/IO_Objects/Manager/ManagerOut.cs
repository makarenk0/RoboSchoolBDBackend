using RoboSchoolBDProjectBackend.DataBaseModel;
using System;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Manager
{
    public class ManagerOut
    {
        public ManagerOut(Managers manager)
        {
            Id = manager.id_manager;
            Name = manager.name;
            Surname = manager.surname;
            Lastname = manager.lastname;
            Email = manager.email;
        }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Lastname { get; set; }
        public String Email { get; set; }
    }
}
