using RoboSchoolBDProjectBackend.Models.OutObjects.ComplexObjDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Request
{
    public class RequestOut
    {

        public RequestOut(Request_items request_items)
        {
            id_request = request_items.id_request;
            date = request_items.date;

            confirmed = request_items.confirmed;
            date_confirmed = request_items.date_confirmed;
            finished = request_items.finished;
            date_finished = request_items.date_finished;

            id_teacher = request_items.id_teacher;
            id_manager = request_items.id_manager;
            items = new List<ItemForRequest>();
            items.Add(new ItemForRequest(request_items.id_item, request_items.name, request_items.items_num));
        }


        public int id_request { get; set; }
        public DateTime date { get; set; }
        public bool confirmed { get; set; }
        public DateTime? date_confirmed { get; set; }
        public bool finished { get; set; }
        public DateTime? date_finished { get; set; }

        public int id_teacher { get; set; }
        public int id_manager { get; set; }


        public List<ItemForRequest> items { get; set; }
    }
}
