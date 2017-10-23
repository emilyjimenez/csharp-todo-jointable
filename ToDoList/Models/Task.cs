using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Task
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public int Id {get; private set;}

        public Task (string name, string description, int id = 0)
        {
            Name = name;
            Description = description;
            Id = id;
        }
        public static List<Task> GetAll()
        {
            List<Task> allTasks = new List<Task> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
              int taskId = rdr.GetInt32(0);
              string taskName = rdr.GetString(1);
              string taskDescription = rdr.GetString(2);
              Task newTask = new Task(taskName, taskDescription, taskId);
              allTasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return allTasks;
        }
        public static void ClearAll()
        {

        }
        public static Task Find(int searchId)
        {
            return new Task("name", "description", 0);
        }
    }
}
