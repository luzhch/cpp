using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework01
{
    class myItem
    {
        public string id;
        public string title;
        public string place;
        public string description;
        public bool completed;
        public DateTime date;

        public myItem(string id, string title, string place, string detail, bool completed, DateTime date)
        {
            this.id = id;
            this.title = title;
            this.place = place;
            this.description = detail;
            this.completed = completed;
            this.date = date;
        }
    }
}
