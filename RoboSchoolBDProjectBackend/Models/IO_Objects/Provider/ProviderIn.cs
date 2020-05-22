using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects
{
    public class ProviderIn
    {
        [Key]
        public String prov_name { get; set; }
        public String contact_number { get; set; }
        public String site_link { get; set; }
    }
}
