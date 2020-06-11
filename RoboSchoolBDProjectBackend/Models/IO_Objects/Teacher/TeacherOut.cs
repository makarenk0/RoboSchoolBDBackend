using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Teacher
{
    public class TeacherOut
    {
        public TeacherOut(Teachers teacher)
        {
            Id_teacher = teacher.id_teacher;
            Name = teacher.name;
            Surname = teacher.surname;
            Lastname = teacher.lastname;
            Email = teacher.email;

            if (teacher.phones != null)
            {
                phones = new List<Phone>();
                foreach (Teacher_phones phone in teacher.phones)
                {
                    phones.Add(new Phone(phone.phone_number));
                }
            }
            
        }
        public int Id_teacher { get; set; }
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
