using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace homework01.Models
{
    public class TodoItem
    {
        public string id;
        public string title { get; set; }
        public string place { get; set; }
        public string description { get; set; }
        public bool completed { set; get; }
        public DateTime date { set; get; }

        public TodoItem(string id, string title, string place, string description, bool completed, DateTime date)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.place = place;
            this.description = description;
            this.completed = false;
            this.date = date;
        }
        public string getid() { return this.id; }
        public void update(string title, string place, string description, DateTime date)
        {
            //this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.place = place;
            this.description = description;
            this.date = date;
        }
        public void delete() { this.id = "null"; this.title = "null"; this.place = "null"; this.description = "null";this.date = new DateTime(1000, 1, 1);   }
    }    
}
