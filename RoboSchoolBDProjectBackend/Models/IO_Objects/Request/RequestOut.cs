
using Microsoft.OData.Edm;
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
            Date = requests.date.ToShortDateString();
            
            Date_confirmed = requests.date_confirmed!=null ? requests.date_confirmed.Value.ToShortDateString() : "not yet";
            Date_finished = requests.date_finished!=null ? requests.date_finished.Value.ToShortDateString() : "not yet";

            Teacher_id = requests.id_teacher;
            Manager_id = requests.id_manager;

            items = new List<RequestItems>();
            foreach (Request_items request_items in requests.items)
            {
                items.Add(new RequestItems(request_items.id_item, request_items.items_num, request_items.Item.name));
            }
        }
        public int Id { get; set; }
        public String Date { get; set; }
        public String Date_confirmed { get; set; }
        public String Date_finished { get; set; }

        public int Teacher_id { get; set; }
        public int Manager_id { get; set; }

        public List<RequestItems> items { get; set; }


        public class RequestItems
        {
            public RequestItems(int id_item, int amount, String name)
            {
                this.id_item = id_item;
                this.name = name;
                this.amount = amount;
            }
            public int id_item { get; set; }
            public String name { get; set; }
            public int amount { get; set; }    
        }
      
    }
}
