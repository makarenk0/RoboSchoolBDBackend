using RoboSchoolBDProjectBackend.DataBaseModel;
using System.ComponentModel.DataAnnotations;

namespace RoboSchoolBDProjectBackend.DataBaseModel
{
    public class Request_items
    {
        [Key]
        public int id_request_items { get; set; }
        public int items_num { get; set; }
        public int id_request { get; set; }
        public int id_item { get; set; }

        //Foreign objects
        public Requests Request { get; set; }
        public Items Item { get; set; }
    }
}
