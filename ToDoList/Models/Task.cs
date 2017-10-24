using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Task
    {
        public string Name {get; private set;}
        public string Description {get; private set;}
        public int Id {get; private set;}
        public int CategoryId {get; private set;}

        public Task (string name, string description, int categoryId, int id = 0)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
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
              int categoryId = rdr.GetInt32(3);
              Task newTask = new Task(taskName, taskDescription, categoryId,  taskId);
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
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO tasks (name, description, category_id) VALUES (@TaskName, @TaskDescription, @CategoryId);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@TaskName";
            name.Value = this.Name;
            cmd.Parameters.Add(name);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@TaskDescription";
            description.Value = this.Description;
            cmd.Parameters.Add(description);

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@CategoryId";
            categoryId.Value = this.CategoryId;
            cmd.Parameters.Add(categoryId);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static Task Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText =  @"SELECT * FROM `tasks` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = searchId;

            cmd.Parameters.Add(thisId);

            int taskId = 0;
            string taskName = "";
            string taskDescription = "";
            int taskCategoryId = 0;

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                taskId = rdr.GetInt32(0);
                taskName = rdr.GetString(1);
                taskDescription = rdr.GetString(2);
                taskCategoryId = rdr.GetInt32(3);
            }

            Task foundTask = new Task(taskName, taskDescription, taskCategoryId, taskId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundTask;
        }

        public override bool Equals(System.Object otherTask)
        {
            if (!(otherTask is Task))
            {
                return false;
            }
            else
            {
                Task newTask = (Task) otherTask;
                bool idEquality = (this.Id == newTask.Id);
                bool nameEquality = (this.Name == newTask.Name);
                bool descriptionEquality = (this.Description == newTask.Description);
                bool categoryEquality = this.CategoryId == newTask.CategoryId;
                return (idEquality && nameEquality && descriptionEquality && categoryEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
