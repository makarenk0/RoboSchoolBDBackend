using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.IO_Objects.Provider
{
    public class ProviderOut
    {
       public ProviderOut(Providers providers)
        {
            Provider_name = providers.prov_name;
            Contact_number = providers.contact_number;
            Site_link = providers.site_link;
        }
        public String Provider_name { get; set; }
        public String Contact_number { get; set; }
        public String Site_link { get; set; }
    }
}
