using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Task
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public int Id {get; private set;}
        private static List<Task> _instances = new List<Task> {};

        public Task (string name, string description)
        {
            Name = name;
            Description = description;
            _instances.Add(this);
            Id = _instances.Count;
        }
        public static List<Task> GetAll()
        {
            return _instances;
        }
        public static void ClearAll()
        {
            _instances.Clear();
        }
        public static Task Find(int searchId)
        {
            return _instances[searchId - 1];
        }
    }
}
