using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Manager
{
    public class ManagerOut
    {
        public ManagerOut(Managers manager)
        {
            Id_manager = manager.id_manager;
            Name = manager.name;
            Surname = manager.surname;
            Lastname = manager.lastname;
            Email = manager.email;

            if (manager.phones != null)
            {
                phones = new List<Phone>();
                foreach (Manager_phones phone in manager.phones)
                {
                    phones.Add(new Phone(phone.phone_number));
                }
            } 
        }
        public int Id_manager { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Lastname { get; set; }
        public String Email { get; set; }

        public List<Phone> phones { get; set; }
    }

    public class Phone
    {
        public Phone(String phone)
        {
            this.phone = phone;
        }
        public String phone;
    }
}
