using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace homework01.ViewModels
{
    public class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }
        private Models.TodoItem selectedItem;

        public TodoItemViewModel()
        {
            this.allItems.Add(new Models.TodoItem("0", "旅游", "北京", "想做的事情", false, DateTime.Today));
        }
        public void AddTodoTtem(string id, string title, string place, string description, bool completed, DateTime date)
        {
            this.allItems.Add(new Models.TodoItem(id, title, place, description, completed, date));
        }
        public void RemoveTodoItem()
        {
            Models.TodoItem temp = this.SelectedItem;
           // this.selecteditem = temp;
            temp.delete();
            //this.selecteditem = null;
        }
        public void UpdateTodoItem(string title, string place, string description,DateTime date)
        {
            Models.TodoItem temp = this.SelectedItem;
            temp.update(title,place,description,date);
            //temp = null;
        }
        public Models.TodoItem SelectedItem
        {
            get { return selectedItem; }
            set { this.selectedItem = value; }
        }

        internal void AddTodoTtem(string v1, string v2, DateTimeOffset now)
        {
            throw new NotImplementedException();
        }
        /*public Models.TodoItem SelectedItem(string id)
{

   for (int i = 0;i < this.allItems.Count;i++)
   {
       if(this.allItems[i].getid() == id)
       {
           return this.allItems[i];
       }
   }
   return new Models.TodoItem("","",new DateTime(1000,1,1));
}*/

    }
}
