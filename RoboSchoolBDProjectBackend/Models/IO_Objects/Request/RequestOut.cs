
using RoboSchoolBDProjectBackend.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboSchoolBDProjectBackend.Models.OutObjects.Request
{
    public class RequestOut
    {

        public RequestOut(Requests requests)
        {
            Id = requests.id_request;
            Date = requests.date;
            Confirmed = requests.confirmed;
            Date_confirmed = requests.date_confirmed;
            Finished = requests.finished;
            Date_finished = requests.date_finished;

            Teacher_id = requests.id_teacher;
            Manager_id = requests.id_manager;

            items = new List<RequestItems>();
            foreach (Request_items request_items in requests.items)
            {
                items.Add(new RequestItems(request_items.id_item, request_items.Item.cost, request_items.Item.prov_name, request_items.Item.name));
            }
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Confirmed { get; set; }
        public DateTime? Date_confirmed { get; set; }
        public bool Finished { get; set; }
        public DateTime? Date_finished { get; set; }

        public int Teacher_id { get; set; }
        public int Manager_id { get; set; }

        public List<RequestItems> items { get; set; }


        public class RequestItems
        {
            public RequestItems(int id_item, int cost, String prov_name, String name)
            {
                this.id_item = id_item;
                this.cost = cost;
                this.prov_name = prov_name;
                this.name = name;
            }
            public int id_item { get; set; }
            public int cost { get; set; }
            public String prov_name { get; set; }
            public String name { get; set; }
        }
      
    }
}
