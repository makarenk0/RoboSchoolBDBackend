
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Requests
    {
        public Requests()
        {
            items = new HashSet<Request_items>();
        }
        [Key]
        public int id_request { get; set; }
        public DateTime date { get; set; }
        public int sum { get; set; }
        public bool confirmed { get; set; }
        public DateTime? date_confirmed { get; set; }
        public bool finished { get; set; }
        public DateTime? date_finished { get; set; }
        public DateTime? date_rejected { get; set; }

        public int id_teacher { get; set; }
        public int id_manager { get; set; }

        public virtual ICollection<Request_items> items { get; set; }
    }
}
